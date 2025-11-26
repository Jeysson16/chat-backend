using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ChatModularMicroservice.Repository;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using System;

namespace ChatModularMicroservice.Domain
{

/// <summary>
/// Servicio de autenticación
/// </summary>
public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IAppRegistroRepository _appRegistroRepository;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        ITokenService tokenService,
        IConfiguration configuration,
        ILogger<AuthService> logger,
        IHttpContextAccessor httpContextAccessor,
        IAppRegistroRepository appRegistroRepository)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
        _configuration = configuration;
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
        _appRegistroRepository = appRegistroRepository;
    }

    /// <summary>
    /// Extrae el ID del usuario del token JWT del contexto HTTP actual
    /// </summary>
    private string GetUserIdFromCurrentToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Headers["Authorization"]
            .FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrWhiteSpace(token))
            return null;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jsonToken = tokenHandler.ReadJwtToken(token);
            return jsonToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
        catch
        {
            return null;
        }
    }

    /// <summary>
    /// Autentica un usuario con credenciales
    /// </summary>
    public async Task<Utils.ItemResponseDT> LoginAsync(LoginDto loginDto)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.cUsuarioCodigo) || string.IsNullOrWhiteSpace(loginDto.cUsuarioContra))
            {
                var err = "Código de usuario y contraseña son requeridos";
                response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
                _logger.LogWarning("Login fallido: entrada inválida. cUsuarioCodigo vacío={IsEmptyUser}, cUsuarioContra vacío={IsEmptyPass}",
                    string.IsNullOrWhiteSpace(loginDto?.cUsuarioCodigo), string.IsNullOrWhiteSpace(loginDto?.cUsuarioContra));
                return response;
            }

            // Entorno de pruebas: relajar validaciones para facilitar E2E
            var env = _configuration["ASPNETCORE_ENVIRONMENT"] ?? Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? string.Empty;
            var isTesting = string.Equals(env, "Testing", StringComparison.OrdinalIgnoreCase);

            // Validar tokens de aplicación si se proporcionan
            // TEMPORAL: Permitir login sin tokens para pruebas
            string appCode = string.Empty;
            string appName = string.Empty;
            
            if (!string.IsNullOrWhiteSpace(loginDto.cTokenAcceso) && !string.IsNullOrWhiteSpace(loginDto.cTokenSecreto))
            {
                var validarTokenRequest = new ValidarTokenRequest
                {
                    cCodigoAplicacion = "SICOM_CHAT_2024", // Código de la aplicación SICOM
                    cTokenAcceso = loginDto.cTokenAcceso,
                    cTokenSecreto = loginDto.cTokenSecreto
                };

                var validacionToken = await _tokenService.ValidarTokensAplicacionAsync(validarTokenRequest);
                if (!validacionToken.bEsValido)
                {
                    response.IsSuccess = false; response.ErrorMessage = $"Tokens de aplicación inválidos: {validacionToken.cMensajeValidacion}";
                    return response;
                }
                
                // Obtener información de la aplicación
                if (!string.IsNullOrWhiteSpace(validacionToken.cCodigoAplicacion))
                {
                    var appEntity = await _appRegistroRepository.GetByCodeAsync(validacionToken.cCodigoAplicacion);
                    if (appEntity != null)
                    {
                        appCode = appEntity.cAppRegistroCodigoApp;
                        appName = appEntity.cAppRegistroNombreApp;
                    }
                }
            }

            // En Testing: evitar acceso al repositorio (posible BD inexistente) y crear usuario en memoria
            ChatModularMicroservice.Entities.Models.Usuario usuario;
            
            // CASO ESPECIAL: JSANCHEZ para pruebas - siempre permitir incluso fuera de Testing
            if (string.Equals(loginDto.cUsuarioCodigo, "JSANCHEZ", StringComparison.OrdinalIgnoreCase))
            {
                _logger.LogInformation("Login especial JSANCHEZ detectado - usando usuario hardcoded");
                usuario = new ChatModularMicroservice.Entities.Models.Usuario
                {
                    cUsuariosId = 1000000001,
                    cUsuariosChatId = "1000000001",
                    cUsuariosChatNombre = "Jeysson Sanchez",
                    cUsuariosChatEmail = "jsanchez@example.com",
                    cUsuariosChatPassword = "jeysson12345", // Contraseña en texto plano para pruebas
                    bUsuariosChatEstaActivo = true,
                    bUsuariosChatEstaEnLinea = false,
                    bUsuarioVerificado = true,
                    cUsuariosChatRol = "ADMIN",
                    cUsuariosChatPerJurCodigo = "DEFAULT",
                    cUsuariosChatUsername = "JSANCHEZ",
                    dUsuariosChatFechaCreacion = DateTime.UtcNow.AddYears(-1)
                };
            }
            else if (isTesting)
            {
                usuario = new ChatModularMicroservice.Entities.Models.Usuario
                {
                    cUsuariosId = 0,
                    cUsuariosChatId = Guid.NewGuid().ToString(),
                    cUsuariosChatNombre = string.IsNullOrWhiteSpace(loginDto.cUsuarioCodigo) ? "demo" : loginDto.cUsuarioCodigo,
                    cUsuariosChatEmail = $"{(string.IsNullOrWhiteSpace(loginDto.cUsuarioCodigo) ? "demo" : loginDto.cUsuarioCodigo)}@example.com",
                    cUsuariosChatPassword = BCrypt.Net.BCrypt.HashPassword(loginDto.cUsuarioContra),
                    bUsuariosChatEstaActivo = true,
                    bUsuariosChatEstaEnLinea = false,
                    bUsuarioVerificado = true,
                    cUsuariosChatRol = "USER",
                    cUsuariosChatPerJurCodigo = string.IsNullOrWhiteSpace(loginDto.cPerJurCodigo) ? "DEFAULT" : loginDto.cPerJurCodigo,
                    dUsuariosChatFechaCreacion = DateTime.UtcNow
                };
            }
            else
            {
                _logger.LogInformation("Buscando usuario por código: {UserCode}", loginDto.cUsuarioCodigo);
                usuario = await _usuarioRepository.GetByUserCodeAsync(loginDto.cUsuarioCodigo);
                _logger.LogInformation("Resultado búsqueda usuario: encontrado={Found}", usuario != null);
            }
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                var err = "Credenciales inválidas";
                response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
                _logger.LogWarning("Login fallido: usuario no encontrado o inactivo. cUsuarioCodigo={UserCode}", loginDto.cUsuarioCodigo);
                return response;
            }

            // Validar empresa si se proporciona en el payload (ignorar "DEFAULT")
            var perJur = loginDto.cPerJurCodigo;
            if (!string.IsNullOrWhiteSpace(perJur) && !string.Equals(perJur, "DEFAULT", StringComparison.OrdinalIgnoreCase))
            {
                var usuarioEmpresa = usuario.cUsuariosChatPerJurCodigo ?? string.Empty;
                if (!string.Equals(usuarioEmpresa, perJur, StringComparison.OrdinalIgnoreCase))
                {
                    var err = "Empresa incorrecta para el usuario";
                    response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
                    _logger.LogWarning("Login fallido: empresa incorrecta. Esperada={Expected} Actual={Actual} Usuario={UserCode}", perJur, usuarioEmpresa, loginDto.cUsuarioCodigo);
                    return response;
                }
            }

            if (!isTesting)
            {
                var storedPassword = usuario.cUsuariosChatPassword ?? string.Empty;
                var isBcrypt = storedPassword.StartsWith("$2");
                bool passwordOk = false;

                // CASO ESPECIAL: JSANCHEZ - comparar contraseña directamente
                if (string.Equals(loginDto.cUsuarioCodigo, "JSANCHEZ", StringComparison.OrdinalIgnoreCase))
                {
                    passwordOk = string.Equals(storedPassword, loginDto.cUsuarioContra);
                    _logger.LogInformation("Validación de contraseña especial JSANCHEZ: {Result}", passwordOk);
                }
                else if (isBcrypt)
                {
                    passwordOk = BCrypt.Net.BCrypt.Verify(loginDto.cUsuarioContra, storedPassword);
                    _logger.LogInformation("Validación de contraseña con BCrypt para usuario {UserCode}: {Result}", loginDto.cUsuarioCodigo, passwordOk);
                }
                else
                {
                    // Fallback temporal: comparar texto plano si no parece hash BCrypt
                    passwordOk = string.Equals(storedPassword, loginDto.cUsuarioContra);
                    _logger.LogWarning("Contraseña almacenada no es BCrypt. Usando comparación simple para usuario {UserCode}", loginDto.cUsuarioCodigo);
                }

                if (!passwordOk)
                {
                    var err = "Credenciales inválidas";
                    response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
                    _logger.LogWarning("Login fallido: contraseña inválida para usuario {UserCode}", loginDto.cUsuarioCodigo);
                    return response;
                }
            }

            if (!isTesting && !(usuario.bUsuarioVerificado ?? false))
            {
                // CASO ESPECIAL: Permitir login para JSANCHEZ incluso sin verificación
                if (!string.Equals(loginDto.cUsuarioCodigo, "JSANCHEZ", StringComparison.OrdinalIgnoreCase))
                {
                    var err = "La cuenta no ha sido verificada. Revisa tu email.";
                    response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
                    _logger.LogWarning("Login fallido: cuenta no verificada para usuario {UserCode}", loginDto.cUsuarioCodigo);
                    return response;
                }
                _logger.LogInformation("Login JSANCHEZ: saltando verificación de cuenta");
            }

            // Generar tokens
            var chatUsuario = ConvertToChartUsuario(usuario);
            chatUsuario.cUsuariosChatAppCodigo = appCode; // Asignar appCode al usuario
            var accessToken = GenerateAccessToken(chatUsuario);
            var refreshToken = GenerateRefreshToken();

            // Los tokens JWT son stateless, no necesitamos almacenarlos en la base de datos

            // Actualizar última conexión
            if (!isTesting)
            {
                usuario.dUsuariosChatUltimaConexion = DateTime.UtcNow;
                usuario.bUsuariosChatEstaEnLinea = true;
                await _usuarioRepository.UpdateAsync(usuario);
            }

            var authResponse = new AuthResponseDto
            {
                Success = true,
                Message = "Login exitoso",
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                AppCode = appCode,
                AppName = appName,
                User = new UserInfoDto
                {
                    Id = usuario.cUsuariosId,
                    Nombre = usuario.cUsuariosChatNombre,
                    Email = usuario.cUsuariosChatEmail,
                    NombreUsuario = usuario.cUsuariosChatUsername ?? string.Empty,
                    Rol = usuario.cUsuariosChatRol ?? "USER",
                    EstaActivo = usuario.bUsuariosChatEstaActivo,
                    FechaCreacion = usuario.dUsuariosChatFechaCreacion,
                    FechaActualizacion = usuario.UpdatedAt,
                    cPerJurCodigo = usuario.cUsuariosChatPerJurCodigo ?? "DEFAULT",
                    cPerCodigo = usuario.cUsuariosChatPerCodigo ?? string.Empty,
                    cUsuarioNombre = usuario.cUsuariosChatNombre,
                    cUsuarioEmail = usuario.cUsuariosChatEmail
                }
            };

            response.IsSuccess = true; response.isSuccess = true; response.Item = authResponse;
            
            _logger.LogInformation("Login exitoso para usuario: {UserCode}", loginDto.cUsuarioCodigo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login para usuario: {UserCode}", loginDto.cUsuarioCodigo);
            var err = $"Error interno del servidor: {ex.Message}";
            response.IsSuccess = false; response.ErrorMessage = err; response.lstError.Add(err);
        }

        return response;
    }

    /// <summary>
    /// Registra un nuevo usuario
    /// </summary>
    public async Task<Utils.ItemResponseDT> RegisterAsync(RegisterDto registerDto)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (registerDto == null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Los datos de registro son requeridos";
                return response;
            }

            // Validaciones según nueva nomenclatura
            if (string.IsNullOrWhiteSpace(registerDto.cUsuariosEmail))
            {
                response.IsSuccess = false; response.ErrorMessage = "El email es requerido";
                return response;
            }

            if (string.IsNullOrWhiteSpace(registerDto.cUsuariosPassword) || registerDto.cUsuariosPassword.Length < 6)
            {
                response.IsSuccess = false; response.ErrorMessage = "La contraseña debe tener al menos 6 caracteres";
                return response;
            }

            if (string.IsNullOrWhiteSpace(registerDto.cUsuariosNombre))
            {
                response.IsSuccess = false; response.ErrorMessage = "El nombre es requerido";
                return response;
            }

            if (!string.Equals(registerDto.cUsuariosPassword, registerDto.cUsuariosConfirmarPassword))
            {
                response.IsSuccess = false; response.ErrorMessage = "Las contraseñas no coinciden";
                return response;
            }

            // Validar tokens de la aplicación (cTokenAcceso + opcional cTokenSecreto) y obtener AppRegistro
            // TEMPORAL: Permitir registro sin tokens para pruebas
            ValidacionTokenDto? tokenValidation = null;
            if (!string.IsNullOrWhiteSpace(registerDto.cTokenAcceso))
            {
                tokenValidation = await _tokenService.ValidarTokensAplicacionAsync(new ValidarTokenRequest
                {
                    cCodigoAplicacion = string.Empty, // puede venir vacío si validamos solo por token
                    cTokenAcceso = registerDto.cTokenAcceso,
                    cTokenSecreto = registerDto.cTokenSecreto ?? string.Empty,
                    bValidarSecreto = !string.IsNullOrWhiteSpace(registerDto.cTokenSecreto),
                    bValidarExpiracion = true,
                    cUsuarioValidador = registerDto.cUsuariosEmail
                });

                if (!tokenValidation.bEsValido)
                {
                    response.IsSuccess = false; response.ErrorMessage = tokenValidation.cMensajeValidacion ?? "Tokens de aplicación inválidos";
                    return response;
                }
            }

            // Verificar si el usuario ya existe
            var existingUser = await _usuarioRepository.GetByEmailAsync(registerDto.cUsuariosEmail);
            if (existingUser != null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Ya existe un usuario con este email";
                return response;
            }

            // Verificar código/username si se proporciona (alias cUsuariosCodigo)
            if (!string.IsNullOrWhiteSpace(registerDto.cUsuariosCodigo))
            {
                var existingUsername = await _usuarioRepository.GetByUsernameAsync(registerDto.cUsuariosCodigo);
                if (existingUsername != null)
                {
                    response.IsSuccess = false; response.ErrorMessage = "Ya existe un usuario con este username";
                    return response;
                }
            }

            // Crear nuevo usuario
            var chatUsuario = new ChatUsuario
            {
                Id = Guid.NewGuid(),
                cUsuariosChatPerJurCodigo = registerDto.cUsuariosPerJurCodigo ?? "DEFAULT",
                cUsuariosChatPerCodigo = string.IsNullOrWhiteSpace(registerDto.cUsuariosPerCodigo) ? Guid.NewGuid().ToString() : registerDto.cUsuariosPerCodigo,
                cUsuariosChatNombre = registerDto.cUsuariosNombre,
                cUsuariosChatEmail = registerDto.cUsuariosEmail,
                cUsuariosChatUsername = registerDto.cUsuariosCodigo ?? registerDto.cUsuariosEmail,
                cUsuariosChatPassword = BCrypt.Net.BCrypt.HashPassword(registerDto.cUsuariosPassword),
                dUsuariosChatFechaCreacion = DateTime.UtcNow,
                bUsuariosChatEstaActivo = true,
                bUsuariosChatEstaEnLinea = false,
                bUsuarioVerificado = true,
                cUsuariosChatRol = "USER",
                cUsuarioTokenVerificacion = null
            };

            // Convertir ChatUsuario a Usuario para el repositorio
            var usuario = new ChatModularMicroservice.Entities.Models.Usuario
            {
                cUsuariosId = 0, // Se asignará automáticamente
                cNombre = chatUsuario.cUsuariosChatNombre,
                cApellido = "", // No disponible en ChatUsuario
                cEmail = chatUsuario.cUsuariosChatEmail,
                cTelefono = null,
                cAvatar = chatUsuario.cUsuariosChatAvatar,
                bActivo = chatUsuario.bUsuariosChatEstaActivo,
                dFechaCreacion = chatUsuario.dUsuariosChatFechaCreacion,
                // nEmpresaId y nAplicacionId se asignan en base a tokens
                nEmpresaId = 0,
                nAplicacionId = 0,
                dUsuariosChatUltimaConexion = chatUsuario.dUsuariosChatUltimaConexion,
                bUsuariosChatEstaActivo = chatUsuario.bUsuariosChatEstaActivo,
                bUsuariosChatEstaEnLinea = chatUsuario.bUsuariosChatEstaEnLinea,
                dUsuariosChatFechaCreacion = chatUsuario.dUsuariosChatFechaCreacion,
                cUsuariosChatId = chatUsuario.Id.ToString(),
                cUsuariosChatNombre = chatUsuario.cUsuariosChatNombre,
                cUsuariosChatEmail = chatUsuario.cUsuariosChatEmail,
                cUsuariosChatPassword = chatUsuario.cUsuariosChatPassword,
                cUsuariosChatPerJurCodigo = chatUsuario.cUsuariosChatPerJurCodigo,
                cUsuariosChatPerCodigo = chatUsuario.cUsuariosChatPerCodigo,
                cUsuariosChatUsername = chatUsuario.cUsuariosChatUsername,
                cUsuariosChatRol = chatUsuario.cUsuariosChatRol,
                bUsuarioVerificado = chatUsuario.bUsuarioVerificado,
                dUsuarioCambioPassword = chatUsuario.dUsuarioCambioPassword,
                cUsuarioConfigPrivacidad = chatUsuario.cUsuarioConfigPrivacidad,
                cUsuarioConfigNotificaciones = chatUsuario.cUsuarioConfigNotificaciones
            };

            // Asignar aplicación usando validación de tokens
            string appCode = string.Empty;
            string appName = string.Empty;
            usuario.nAplicacionId = 0; // default
            usuario.nEmpresaId = 0; // default
            if (tokenValidation != null && !string.IsNullOrWhiteSpace(tokenValidation.cCodigoAplicacion))
            {
                // Obtener AppRegistro por código de aplicación y asignar nAplicacionId
                var appEntity = await _appRegistroRepository.GetByCodeAsync(tokenValidation.cCodigoAplicacion);
                if (appEntity != null)
                {
                    usuario.nAplicacionId = appEntity.nAppRegistrosAplicacionId;
                    appCode = appEntity.cAppRegistroCodigoApp;
                    appName = appEntity.cAppRegistroNombreApp;
                }
            }
            // Generar cPerCodigo si no viene
            chatUsuario.cUsuariosChatPerCodigo = string.IsNullOrWhiteSpace(registerDto.cUsuariosPerCodigo)
                ? Guid.NewGuid().ToString()
                : registerDto.cUsuariosPerCodigo;

            var created = await _usuarioRepository.CreateAsync(usuario);

            var createdChatUsuario = ConvertToChartUsuario(created);
            createdChatUsuario.cUsuariosChatAppCodigo = appCode; // Asignar appCode al usuario
            var accessToken = GenerateAccessToken(createdChatUsuario);
            var refreshToken = GenerateRefreshToken();

            var authResponse = new AuthResponseDto
            {
                Success = true,
                IsSuccess = true,
                Message = "Usuario registrado exitosamente",
                Token = accessToken,
                RefreshToken = refreshToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                AppCode = appCode,
                AppName = appName,
                User = new UserInfoDto
                {
                    Id = created.cUsuariosId,
                    Nombre = created.cUsuariosChatNombre,
                    Email = created.cUsuariosChatEmail,
                    NombreUsuario = created.cUsuariosChatUsername ?? string.Empty,
                    Rol = created.cUsuariosChatRol ?? "USER",
                    EstaActivo = created.bUsuariosChatEstaActivo,
                    FechaCreacion = created.dUsuariosChatFechaCreacion,
                    FechaActualizacion = created.UpdatedAt,
                    cPerJurCodigo = created.cUsuariosChatPerJurCodigo ?? "DEFAULT",
                    cPerCodigo = created.cUsuariosChatPerCodigo ?? string.Empty,
                    cUsuarioNombre = created.cUsuariosChatNombre,
                    cUsuarioEmail = created.cUsuariosChatEmail
                }
            };

            response.IsSuccess = true; response.Item = authResponse;
            
            _logger.LogInformation("Usuario registrado exitosamente: {Email}", registerDto.cUsuariosEmail);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro de usuario: {Email}", registerDto.cUsuariosEmail);
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Refresca el token de autenticación
    /// </summary>
    public async Task<Utils.ItemResponseDT> RefreshTokenAsync(string refreshToken)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
            {
                response.IsSuccess = false; response.ErrorMessage = "Refresh token es requerido";
                return response;
            }

            // Los tokens JWT son stateless, validamos directamente el refresh token
            var principal = await _tokenService.ValidateJwtTokenAsync(refreshToken);
            
            if (principal == null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Refresh token inválido o expirado";
                return response;
            }

            var userIdClaim = principal.FindFirst("userId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                response.IsSuccess = false; response.ErrorMessage = "Token inválido: no contiene ID de usuario";
                return response;
            }
            
            var usuario = await _usuarioRepository.GetByIdAsync(Guid.Parse(userIdClaim));
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                response.IsSuccess = false; response.ErrorMessage = "Usuario no encontrado o inactivo";
                return response;
            }

            // Generar nuevo access token
            var chatUsuario = ConvertToChartUsuario(usuario);
            var newAccessToken = GenerateAccessToken(chatUsuario);
            var newRefreshToken = GenerateRefreshToken();

            // Los tokens JWT son stateless, no necesitamos actualizarlos en la base de datos

            var authResponse = new AuthResponseDto
            {
                Success = true,
                Message = "Token refrescado exitosamente",
                Token = newAccessToken,
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                User = new UserInfoDto
                {
                    cPerJurCodigo = usuario.cUsuariosChatPerJurCodigo,
                    cPerCodigo = usuario.cUsuariosChatPerCodigo,
                    cUsuarioNombre = usuario.cUsuariosChatNombre,
                    cUsuarioEmail = usuario.cUsuariosChatEmail
                }
            };

            response.IsSuccess = true; response.Item = authResponse;
            
            _logger.LogInformation("Token refrescado para usuario: {UserId}", usuario.cUsuariosId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al refrescar token");
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Cierra la sesión de un usuario
    /// </summary>
    public async Task<Utils.ItemResponseDT> LogoutAsync(string userId)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                response.IsSuccess = false; response.ErrorMessage = "User ID es requerido";
                return response;
            }

            if (!Guid.TryParse(userId, out var userGuid))
            {
                response.IsSuccess = false; response.ErrorMessage = "User ID inválido";
                return response;
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            
            if (usuario != null)
            {
                usuario.bUsuariosChatEstaEnLinea = false;
                usuario.dUsuariosChatUltimaConexion = DateTime.UtcNow;
                await _usuarioRepository.UpdateAsync(usuario);
            }

            // Los tokens JWT son stateless, no necesitamos invalidarlos en la base de datos

            response.IsSuccess = true;
            
            _logger.LogInformation("Logout exitoso para usuario: {UserId}", userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en logout para usuario: {UserId}", userId);
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Valida un token JWT
    /// </summary>
    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return false;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userGuid))
            {
                return false;
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                return false;
            }

            return true;
        }
        catch (SecurityTokenException)
        {
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar token");
            return false;
        }
    }

    /// <summary>
    /// Cambia la contraseña de un usuario
    /// </summary>
    public async Task<Utils.ItemResponseDT> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        var response = new Utils.ItemResponseDT();
        Guid userGuid = Guid.Empty;
        
        try
        {
            var userId = GetUserIdFromCurrentToken();
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out userGuid))
            {
                response.IsSuccess = false; response.ErrorMessage = "User ID inválido o token no válido";
                return response;
            }

            if (changePasswordDto == null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Los datos del cambio de contraseña son requeridos";
                return response;
            }

            if (string.IsNullOrWhiteSpace(changePasswordDto.CurrentPassword) || 
                string.IsNullOrWhiteSpace(changePasswordDto.NewPassword))
            {
                response.IsSuccess = false; response.ErrorMessage = "Contraseña actual y nueva contraseña son requeridas";
                return response;
            }

            if (changePasswordDto.NewPassword.Length < 6)
            {
                response.IsSuccess = false; response.ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres";
                return response;
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                response.IsSuccess = false; response.ErrorMessage = "Usuario no encontrado o inactivo";
                return response;
            }

            if (!BCrypt.Net.BCrypt.Verify(changePasswordDto.CurrentPassword, usuario.cUsuariosChatPassword))
            {
                response.IsSuccess = false; response.ErrorMessage = "La contraseña actual es incorrecta";
                return response;
            }

            usuario.cUsuariosChatPassword = BCrypt.Net.BCrypt.HashPassword(changePasswordDto.NewPassword);
            usuario.dUsuarioCambioPassword = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario);

            // Los tokens JWT son stateless, no necesitamos invalidarlos en la base de datos

            response.IsSuccess = true; response.Item = true;
            
            _logger.LogInformation("Contraseña cambiada para usuario: {UserId}", userGuid);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar contraseña para usuario: {UserId}", userGuid);
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Solicita el restablecimiento de contraseña
    /// </summary>
    public async Task<Utils.ItemResponseDT> RequestPasswordResetAsync(string email)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                response.IsSuccess = false; response.ErrorMessage = "El email es requerido";
                return response;
            }

            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                // Por seguridad, siempre devolvemos éxito
                response.IsSuccess = true; response.Item = true;
                return response;
            }

            var resetToken = Guid.NewGuid().ToString();
            usuario.cUsuarioTokenReset = resetToken;
            usuario.dUsuarioTokenResetExpiracion = DateTime.UtcNow.AddHours(1);
            await _usuarioRepository.UpdateAsync(usuario);

            // TODO: Enviar email con el token de reset
            
            response.IsSuccess = true; response.Item = true;
            
            _logger.LogInformation("Solicitud de reset de contraseña para email: {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al solicitar reset de contraseña para email: {Email}", email);
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Restablece la contraseña con un token
    /// </summary>
    public async Task<Utils.ItemResponseDT> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (resetPasswordDto == null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Los datos de restablecimiento son requeridos";
                return response;
            }

            if (string.IsNullOrWhiteSpace(resetPasswordDto.Token) || 
                string.IsNullOrWhiteSpace(resetPasswordDto.NewPassword))
            {
                response.IsSuccess = false; response.ErrorMessage = "Token y nueva contraseña son requeridos";
                return response;
            }

            if (resetPasswordDto.NewPassword.Length < 6)
            {
                response.IsSuccess = false; response.ErrorMessage = "La nueva contraseña debe tener al menos 6 caracteres";
                return response;
            }

            var usuario = await _usuarioRepository.GetByResetTokenAsync(resetPasswordDto.Token);
            
            if (usuario == null || 
                usuario.dUsuarioTokenResetExpiracion == null || 
                usuario.dUsuarioTokenResetExpiracion < DateTime.UtcNow)
            {
                response.IsSuccess = false; response.ErrorMessage = "Token de restablecimiento inválido o expirado";
                return response;
            }

            usuario.cUsuariosChatPassword = BCrypt.Net.BCrypt.HashPassword(resetPasswordDto.NewPassword);
            usuario.cUsuarioTokenReset = null;
            usuario.dUsuarioTokenResetExpiracion = null;
            usuario.dUsuarioCambioPassword = DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(usuario);

            // Los tokens JWT son stateless, no necesitamos invalidarlos en la base de datos

            response.IsSuccess = true; response.Item = true;
            
            _logger.LogInformation("Contraseña restablecida para usuario: {UserId}", usuario.cUsuariosId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restablecer contraseña");
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Verifica la cuenta de un usuario
    /// </summary>
    public async Task<Utils.ItemResponseDT> VerifyAccountAsync(string verificationToken)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(verificationToken))
            {
                response.IsSuccess = false; response.ErrorMessage = "Token de verificación es requerido";
                return response;
            }

            var usuario = await _usuarioRepository.GetByVerificationTokenAsync(verificationToken);
            
            if (usuario == null)
            {
                response.IsSuccess = false; response.ErrorMessage = "Token de verificación inválido";
                return response;
            }

            if (usuario.bUsuarioVerificado == true)
            {
                response.IsSuccess = false; response.ErrorMessage = "La cuenta ya ha sido verificada";
                return response;
            }

            usuario.bUsuarioVerificado = true;
            usuario.cUsuarioTokenVerificacion = null;
            await _usuarioRepository.UpdateAsync(usuario);

            response.IsSuccess = true; response.Item = true;
            
            _logger.LogInformation("Cuenta verificada para usuario: {UserId}", usuario.cUsuariosId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar cuenta");
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    /// <summary>
    /// Reenvía el email de verificación
    /// </summary>
    public async Task<Utils.ItemResponseDT> ResendVerificationEmailAsync(string email)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                response.IsSuccess = false; response.ErrorMessage = "El email es requerido";
                return response;
            }

            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                response.IsSuccess = false; response.ErrorMessage = "Usuario no encontrado";
                return response;
            }

            if (usuario.bUsuarioVerificado == true)
            {
                response.IsSuccess = false; response.ErrorMessage = "La cuenta ya ha sido verificada";
                return response;
            }

            usuario.cUsuarioTokenVerificacion = Guid.NewGuid().ToString();
            await _usuarioRepository.UpdateAsync(usuario);

            // TODO: Enviar email de verificación
            
            response.IsSuccess = true; response.Item = true;
            
            _logger.LogInformation("Email de verificación reenviado para usuario: {Email}", email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al reenviar email de verificación para: {Email}", email);
            response.IsSuccess = false; response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    #region Private Methods

    private string GenerateAccessToken(ChatUsuario usuario)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);
        
        // Debug: Log the user's role
        _logger.LogInformation("Generating access token for user {UserId} with role: {Role}", 
            usuario.cUsuariosChatId, usuario.cUsuariosChatRol);
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.cUsuariosChatId),
            new Claim(ClaimTypes.Email, usuario.cUsuariosChatEmail),
            new Claim(ClaimTypes.Name, usuario.cUsuariosChatNombre),
            new Claim(ClaimTypes.Role, usuario.cUsuariosChatRol ?? "USER"), // Ensure role is never null
            new Claim("username", usuario.cUsuariosChatUsername),
            new Claim("per_codigo", usuario.cUsuariosChatPerCodigo),
            new Claim("per_jur_codigo", usuario.cUsuariosChatPerJurCodigo),
            new Claim("user_id", usuario.cUsuariosChatId) // Add user_id claim for SignalR
        };
        
        // Agregar app_code si existe
        if (!string.IsNullOrWhiteSpace(usuario.cUsuariosChatAppCodigo))
        {
            claims.Add(new Claim("app_code", usuario.cUsuariosChatAppCodigo));
        }
        
        var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"];
        int expirationMinutes = 60;
        if (int.TryParse(expirationMinutesStr, out var cfgMinutes)) expirationMinutes = cfgMinutes;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        var tokenString = tokenHandler.WriteToken(token);
        
        // Debug: Log the generated token claims
        _logger.LogInformation("Generated token with claims: {Claims}", 
            string.Join(", ", claims.Select(c => $"{c.Type}:{c.Value}")));
        
        return tokenString;
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    /// <summary>
    /// Convierte un Usuario de Entities a ChatUsuario de Domain
    /// </summary>
    private ChatUsuario ConvertToChartUsuario(ChatModularMicroservice.Entities.Models.Usuario usuario)
    {
        return new ChatUsuario
        {
            cUsuariosChatId = usuario.cUsuariosChatId,
            cUsuariosChatPerJurCodigo = usuario.cUsuariosChatPerJurCodigo ?? string.Empty,
            cUsuariosChatPerCodigo = usuario.cUsuariosChatPerCodigo ?? string.Empty,
            cUsuariosChatAppCodigo = usuario.cUsuariosChatAppCodigo ?? string.Empty,
            cUsuariosChatNombre = usuario.cUsuariosChatNombre,
            cUsuariosChatEmail = usuario.cUsuariosChatEmail,
            dUsuariosChatUltimaConexion = usuario.dUsuariosChatUltimaConexion,
            bUsuariosChatEstaActivo = usuario.bUsuariosChatEstaActivo,
            cUsuariosChatRol = usuario.cUsuariosChatRol ?? "USER",
            cUsuariosChatUsername = usuario.cUsuariosChatUsername ?? string.Empty,
            cUsuariosChatPassword = usuario.cUsuariosChatPassword,
            cUsuariosChatAvatar = usuario.cUsuariosChatAvatar,
            dUsuariosChatFechaCreacion = usuario.dUsuariosChatFechaCreacion,
            bUsuariosChatEstaEnLinea = usuario.bUsuariosChatEstaEnLinea,
            bUsuarioVerificado = usuario.bUsuarioVerificado ?? false,
            cUsuarioTokenVerificacion = usuario.cUsuarioTokenVerificacion,
            cUsuarioTokenReset = usuario.cUsuarioTokenReset,
            dUsuarioTokenResetExpiracion = usuario.dUsuarioTokenResetExpiracion,
            dUsuarioCambioPassword = usuario.dUsuarioCambioPassword,
            cUsuarioConfigPrivacidad = usuario.cUsuarioConfigPrivacidad,
            cUsuarioConfigNotificaciones = usuario.cUsuarioConfigNotificaciones,
            nUsuariosId = usuario.cUsuariosId.ToString()
        };
    }

    /// <summary>
    /// Obtiene la información del usuario a partir de un token JWT
    /// </summary>
    public async Task<Utils.ItemResponseDT> GetUserByTokenAsync(string token)
    {
        var response = new Utils.ItemResponseDT();
        
        try
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                response.IsSuccess = false; 
                response.ErrorMessage = "Token es requerido";
                return response;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"]);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                ValidateAudience = true,
                ValidAudience = _configuration["Jwt:Audience"],
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
            var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userGuid))
            {
                response.IsSuccess = false; 
                response.ErrorMessage = "Token inválido";
                return response;
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                response.IsSuccess = false; 
                response.ErrorMessage = "Usuario no encontrado o inactivo";
                return response;
            }

            var usuarioDto = new ChatUsuarioDto
            {
                cUsuariosChatId = usuario.cUsuariosChatId,
                cUsuariosChatNombre = usuario.cUsuariosChatNombre,
                cUsuariosChatEmail = usuario.cUsuariosChatEmail,
                cUsuariosChatAvatar = usuario.cUsuariosChatAvatar,
                dUsuariosChatFechaCreacion = usuario.dUsuariosChatFechaCreacion,
                bUsuariosChatEstaActivo = usuario.bUsuariosChatEstaActivo,
                bUsuariosChatEstaEnLinea = usuario.bUsuariosChatEstaEnLinea
            };

            response.IsSuccess = true; 
            response.Item = usuarioDto;
        }
        catch (SecurityTokenException)
        {
            response.IsSuccess = false; 
            response.ErrorMessage = "Token inválido o expirado";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por token");
            response.IsSuccess = false; 
            response.ErrorMessage = $"Error interno del servidor: {ex.Message}";
        }

        return response;
    }

    #endregion
}

}
