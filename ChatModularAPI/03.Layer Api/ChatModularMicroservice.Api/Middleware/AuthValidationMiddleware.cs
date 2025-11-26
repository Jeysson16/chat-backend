using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ChatModularMicroservice.Domain;

namespace ChatModularMicroservice.Api.Middleware;

/// <summary>
/// Middleware para validar autenticación y autorización
/// </summary>
public class AuthValidationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthValidationMiddleware> _logger;
    private readonly IConfiguration _configuration;

    // Rutas que no requieren autenticación
    private readonly string[] _publicPaths = {
        "/",
        "/api/v1/auth/login",
        "/api/v1/auth/register",
        "/api/v1/auth/status",
        "/api/v1/auth/generar-tokens-aplicacion",
        "/api/v1/auth/validar-access-token",
        "/api/v1/auth/renovar-tokens",
        "/api/v1/auth/revocar-tokens",
        "/api/v1/auth/tokens-aplicacion",
        "/api/v1/aplicaciones", // Permitir registro de aplicaciones
        "/swagger",
        "/swagger/index.html",
        "/swagger/v1/swagger.json",
        "/api/docs",
        "/chathub" // Permitir conexiones SignalR (nota: minúsculas)
    };

    public AuthValidationMiddleware(RequestDelegate next, ILogger<AuthValidationMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            // Log para debugging
            _logger.LogInformation("Verificando ruta: {Path}", context.Request.Path);
            _logger.LogInformation("Rutas públicas configuradas: {PublicPaths}", string.Join(", ", _publicPaths));
            
            // Verificar si la ruta requiere autenticación
            var isPublic = IsPublicPath(context.Request.Path);
            _logger.LogInformation("¿Es ruta pública? {IsPublic}", isPublic);
            
            if (isPublic)
            {
                _logger.LogInformation("Ruta pública detectada: {Path}", context.Request.Path);
                await _next(context);
                return;
            }
            
            _logger.LogInformation("Ruta privada detectada: {Path}", context.Request.Path);

            // Obtener el token de autorización
            var token = GetTokenFromRequest(context.Request);
            
            if (string.IsNullOrEmpty(token))
            {
                await HandleUnauthorized(context, "Token de autorización requerido");
                return;
            }

            // Validar el token
            var principal = await ValidateTokenAsync(token);
            
            if (principal == null)
            {
                await HandleUnauthorized(context, "Token de autorización inválido");
                return;
            }

            // Agregar información del usuario al contexto
            context.User = principal;
            context.Items["UserId"] = GetUserIdFromClaims(principal);
            context.Items["UserName"] = GetUserNameFromClaims(principal);
            context.Items["UserRole"] = GetUserRoleFromClaims(principal);

            // Extraer información adicional de headers para SignalR y chat
            var appCodeHeader = context.Request.Headers["X-App-Code"].FirstOrDefault();
            var accessTokenHeader = context.Request.Headers["X-Access-Token"].FirstOrDefault();
            var secretTokenHeader = context.Request.Headers["X-Secret-Token"].FirstOrDefault();
            var userCodeHeader = context.Request.Headers["X-User-Code"].FirstOrDefault();
            var userIdHeader = context.Request.Headers["X-User-Id"].FirstOrDefault();
            var personCodeHeader = context.Request.Headers["X-Person-Code"].FirstOrDefault();

            if (!string.IsNullOrEmpty(appCodeHeader))
                context.Items["AppCode"] = appCodeHeader;
            if (!string.IsNullOrEmpty(accessTokenHeader))
                context.Items["AccessToken"] = accessTokenHeader;
            if (!string.IsNullOrEmpty(secretTokenHeader))
                context.Items["SecretToken"] = secretTokenHeader;
            if (!string.IsNullOrEmpty(userCodeHeader))
                context.Items["UserCode"] = userCodeHeader;
            if (!string.IsNullOrEmpty(personCodeHeader))
                context.Items["PersonCode"] = personCodeHeader;

            // Loggear información de autenticación
            _logger.LogInformation("Usuario autenticado: {UserName} (ID: {UserId})", 
                context.Items["UserName"], 
                context.Items["UserId"]);

            await _next(context);
        }
        catch (SecurityTokenExpiredException)
        {
            await HandleUnauthorized(context, "Token expirado");
        }
        catch (SecurityTokenInvalidSignatureException)
        {
            await HandleUnauthorized(context, "Firma del token inválida");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en validación de autenticación");
            await HandleUnauthorized(context, "Error de autenticación");
        }
    }

    private bool IsPublicPath(string path)
    {
        // Normalize the path by removing trailing slashes and converting to lowercase
        var normalizedPath = path.TrimEnd('/').ToLowerInvariant();
        
        // Permitir TODAS las rutas de swagger sin restricción (incluyendo el archivo JSON)
        if (normalizedPath.Contains("swagger"))
        {
            return true;
        }
        
        return _publicPaths.Any(publicPath => 
        {
            var normalizedPublicPath = publicPath.ToLowerInvariant();
            return normalizedPath.Equals(normalizedPublicPath, StringComparison.OrdinalIgnoreCase) ||
                   normalizedPath.StartsWith(normalizedPublicPath + "/", StringComparison.OrdinalIgnoreCase);
        });
    }

    private string? GetTokenFromRequest(HttpRequest request)
    {
        // Buscar en el header Authorization
        var authHeader = request.Headers["Authorization"].FirstOrDefault();
        _logger.LogInformation("Authorization header: {AuthHeader}", authHeader ?? "(null)");
        
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            var token = authHeader.Substring("Bearer ".Length).Trim();
            _logger.LogInformation("Token found in Authorization header (length: {Length})", token.Length);
            return token;
        }

        // Buscar en query parameters (para WebSockets o casos especiales)
        var tokenFromQuery = request.Query["token"].FirstOrDefault();
        if (!string.IsNullOrEmpty(tokenFromQuery))
        {
            _logger.LogInformation("Token found in query parameter");
            return tokenFromQuery;
        }

        // Buscar en cookies
        var tokenFromCookie = request.Cookies["auth_token"];
        if (!string.IsNullOrEmpty(tokenFromCookie))
        {
            _logger.LogInformation("Token found in cookie");
            return tokenFromCookie;
        }

        _logger.LogInformation("No token found in request");
        return null;
    }

    private async Task<ClaimsPrincipal?> ValidateTokenAsync(string token)
    {
        try
        {
            _logger.LogInformation("Starting JWT token validation");
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"] ?? "");
            
            _logger.LogInformation("JWT Configuration - Issuer: {Issuer}, Audience: {Audience}", 
                _configuration["Jwt:Issuer"], _configuration["Jwt:Audience"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(5) // Allow 5 minutes clock skew to handle time differences
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
            _logger.LogInformation("JWT token validation successful");
            return principal;
        }
        catch (SecurityTokenExpiredException ex)
        {
            _logger.LogWarning("JWT token validation failed - Token expired: {Message}", ex.Message);
            return null;
        }
        catch (SecurityTokenInvalidSignatureException ex)
        {
            _logger.LogWarning("JWT token validation failed - Invalid signature: {Message}", ex.Message);
            return null;
        }
        catch (SecurityTokenInvalidIssuerException ex)
        {
            _logger.LogWarning("JWT token validation failed - Invalid issuer: {Message}", ex.Message);
            return null;
        }
        catch (SecurityTokenInvalidAudienceException ex)
        {
            _logger.LogWarning("JWT token validation failed - Invalid audience: {Message}", ex.Message);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "JWT token validation failed - General error");
            return null;
        }
    }

    private string? GetUserIdFromClaims(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               principal.FindFirst("sub")?.Value ??
               principal.FindFirst("userId")?.Value;
    }

    private string? GetUserNameFromClaims(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Name)?.Value ??
               principal.FindFirst("username")?.Value ??
               principal.FindFirst("name")?.Value;
    }

    private string? GetUserRoleFromClaims(ClaimsPrincipal principal)
    {
        return principal.FindFirst(ClaimTypes.Role)?.Value ??
               principal.FindFirst("role")?.Value;
    }

    private async Task HandleUnauthorized(HttpContext context, string message)
    {
        context.Response.StatusCode = 401;
        context.Response.ContentType = "application/json";

        var response = new
        {
            clientName = context.Request.Headers["X-Client-Name"].FirstOrDefault() ?? "Unknown",
            isSuccess = false,
            lstError = new[] { message },
            lstItem = new object[0],
            pagination = (object?)null,
            resultado = (object?)null,
            serverName = Environment.MachineName,
            ticket = context.Items["TrackingTicket"]?.ToString() ?? Guid.NewGuid().ToString(),
            userName = "Anonymous",
            warnings = new string[0]
        };

        var jsonResponse = System.Text.Json.JsonSerializer.Serialize(response, new System.Text.Json.JsonSerializerOptions
        {
            PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}