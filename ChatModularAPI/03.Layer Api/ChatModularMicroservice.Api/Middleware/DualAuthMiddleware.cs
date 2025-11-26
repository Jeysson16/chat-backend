using ChatModularMicroservice.Domain;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ChatModularMicroservice.Api.Middleware;

public class DualAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<DualAuthMiddleware> _logger;

    public DualAuthMiddleware(RequestDelegate next, ILogger<DualAuthMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, ITokenService tokenService)
    {
        // Skip authentication for certain endpoints
        var path = context.Request.Path.Value?.ToLower();
        if (path != null && (path.Contains("/auth/login") || path.Contains("/webhook")))
        {
            await _next(context);
            return;
        }

        try
        {
            // Extract headers
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            var appCodeHeader = context.Request.Headers["x-app-code"].FirstOrDefault();

            if (string.IsNullOrEmpty(authHeader) || string.IsNullOrEmpty(appCodeHeader))
            {
                _logger.LogWarning("Missing required headers: Authorization or x-app-code");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Missing required headers");
                return;
            }

            // Extract JWT token
            var token = authHeader.Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Invalid Authorization header format");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid Authorization header");
                return;
            }

            // Validate application access token
            var isValidApp = await tokenService.ValidateApplicationTokenAsync(appCodeHeader);
            if (!isValidApp)
            {
                _logger.LogWarning("Invalid application code: {AppCode}", appCodeHeader);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid application");
                return;
            }

            // Validate JWT token
            var principal = await tokenService.ValidateJwtTokenAsync(token);
            if (principal == null)
            {
                _logger.LogWarning("Invalid JWT token");
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Invalid token");
                return;
            }

            // Verify token belongs to the application
            var tokenAppCode = principal.FindFirst("app_code")?.Value;
            if (tokenAppCode != appCodeHeader)
            {
                _logger.LogWarning("Token app code mismatch. Expected: {Expected}, Got: {Actual}", 
                    appCodeHeader, tokenAppCode);
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Token application mismatch");
                return;
            }

            // Add user information to context
            context.User = principal;
            context.Items["AppCode"] = appCodeHeader;
            context.Items["UserId"] = principal.FindFirst("user_id")?.Value;
            context.Items["PerJurCodigo"] = principal.FindFirst("per_jur_codigo")?.Value;
            context.Items["PerCodigo"] = principal.FindFirst("per_codigo")?.Value;

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in dual authentication middleware");
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync("Authentication error");
        }
    }
}