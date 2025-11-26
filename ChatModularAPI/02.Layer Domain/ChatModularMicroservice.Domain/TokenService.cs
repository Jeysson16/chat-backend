using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Implementación del servicio de tokens
    /// </summary>
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenService> _logger;
        private readonly TokenDomain _tokenDomain;
        private readonly IAppRegistroRepository _appRegistroRepository;

        public TokenService(
            ITokenRepository tokenRepository,
            IConfiguration configuration,
            ILogger<TokenService> logger,
            IAppRegistroRepository appRegistroRepository)
        {
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _logger = logger;
            _tokenDomain = new TokenDomain(tokenRepository);
            _appRegistroRepository = appRegistroRepository;
        }

        public async Task<string> GenerateJwtTokenAsync(string appCode, string perJurCodigo, string perCodigo, string usuarioNombre)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? _configuration["Jwt:Key"] ?? "default-secret-key");
                
                var expirationMinutesStr = _configuration["Jwt:ExpirationMinutes"];
                int expirationMinutes = 60;
                if (int.TryParse(expirationMinutesStr, out var cfgMinutes)) expirationMinutes = cfgMinutes;
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                        new Claim("appCode", appCode),
                        new Claim("perJurCodigo", perJurCodigo),
                        new Claim("perCodigo", perCodigo),
                        new Claim("usuarioNombre", usuarioNombre),
                        new Claim(ClaimTypes.Name, usuarioNombre)
                    }),
                    Expires = DateTime.UtcNow.AddMinutes(expirationMinutes),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                    Issuer = _configuration["Jwt:Issuer"],
                    Audience = _configuration["Jwt:Audience"]
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating JWT token for app {AppCode}", appCode);
                throw;
            }
        }

        public async Task<ClaimsPrincipal?> ValidateJwtTokenAsync(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:SecretKey"] ?? _configuration["Jwt:Key"] ?? "default-secret-key");

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

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating JWT token");
                return null;
            }
        }

        public async Task<bool> ValidateApplicationTokenAsync(string appCode)
        {
            try
            {
                // Implementar validación de token de aplicación
                // Por ahora retornamos true como placeholder
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating application token for app {AppCode}", appCode);
                return false;
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            try
            {
                // Implementar revocación de token
                // Por ahora retornamos true como placeholder
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking token");
                return false;
            }
        }

        public async Task<bool> IsTokenActiveAsync(string token)
        {
            try
            {
                var principal = await ValidateJwtTokenAsync(token);
                return principal != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if token is active");
                return false;
            }
        }

        public async Task<TokensAplicacionDto> GenerarTokensAplicacionAsync(GenerarTokensRequest request)
        {
            try
            {
                // Implementar generación de tokens de aplicación
                // Por ahora retornamos un DTO básico como placeholder
                return await Task.FromResult(new TokensAplicacionDto
                {
                    cCodigoAplicacion = request.cCodigoAplicacion,
                    cTokenAcceso = Guid.NewGuid().ToString(),
                    cTokenSecreto = Guid.NewGuid().ToString(),
                    dFechaExpiracion = DateTime.UtcNow.AddHours(request.nHorasValidez),
                    bEsActivo = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating application tokens for {AppCode}", request.cCodigoAplicacion);
                throw;
            }
        }

        public async Task<ValidacionTokenDto> ValidarTokensAplicacionAsync(ValidarTokenRequest request)
        {
            try
            {
                if (request == null || string.IsNullOrWhiteSpace(request.cTokenAcceso))
                {
                    return new ValidacionTokenDto
                    {
                        bEsValido = false,
                        cMensajeValidacion = "Token de acceso requerido",
                        cEstado = "Invalido"
                    };
                }

                // Buscar AppRegistro por token de acceso
                var filter = new ChatModularMicroservice.Entities.AppRegistroFilter
                {
                    cAppRegistroToken = request.cTokenAcceso
                };

                ChatModularMicroservice.Entities.Models.AppRegistro app;
                try
                {
                    app = await _appRegistroRepository.GetItem(filter, ChatModularMicroservice.Entities.AppRegistroFilterItemType.ByToken);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "No se encontró AppRegistro por token de acceso");
                    return new ValidacionTokenDto
                    {
                        bEsValido = false,
                        cMensajeValidacion = "Aplicación no encontrada para el token",
                        cEstado = "NoEncontrado"
                    };
                }

                // Validar secreto si corresponde
                if (request.bValidarSecreto && !string.IsNullOrWhiteSpace(request.cTokenSecreto))
                {
                    var secretoOk = string.Equals(app.cSecretToken, request.cTokenSecreto, StringComparison.Ordinal);
                    if (!secretoOk)
                    {
                        return new ValidacionTokenDto
                        {
                            bEsValido = false,
                            cCodigoAplicacion = app.cAppCodigo,
                            cTokenAcceso = request.cTokenAcceso,
                            cMensajeValidacion = "Token secreto inválido",
                            cEstado = "SecretoInvalido",
                            bEsActivo = app.bAppActivo,
                            dFechaValidacion = DateTime.UtcNow
                        };
                    }
                }

                // Validar estado activo y expiración
                var activo = app.bAppActivo;
                var expiracion = app.dAppRegistroFechaExpiracion;
                var noExpirado = !request.bValidarExpiracion || expiracion == null || expiracion > DateTime.UtcNow;

                var diasRestantesNullable = expiracion == null ? (int?)null : (int)Math.Ceiling((expiracion.Value - DateTime.UtcNow).TotalDays);

                var esValido = activo && noExpirado;

                return new ValidacionTokenDto
                {
                    bEsValido = esValido,
                    cCodigoAplicacion = app.cAppCodigo,
                    cTokenAcceso = request.cTokenAcceso,
                    dFechaExpiracion = expiracion ?? DateTime.MaxValue,
                    nDiasRestantes = diasRestantesNullable ?? 0,
                    cMensajeValidacion = esValido ? "Tokens válidos" : (activo ? "Token expirado" : "Aplicación inactiva"),
                    bEsActivo = activo,
                    dFechaValidacion = DateTime.UtcNow,
                    cUsuarioValidador = request.cUsuarioValidador,
                    cEstado = esValido ? "Valido" : "Invalido"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating application tokens for {AppCode}", request.cCodigoAplicacion);
                throw;
            }
        }

        public async Task<TokensAplicacionDto> RenovarTokensAplicacionAsync(RenovarTokensRequest request)
        {
            try
            {
                // Implementar renovación de tokens de aplicación
                // Por ahora retornamos un DTO básico como placeholder
                return await Task.FromResult(new TokensAplicacionDto
                {
                    cCodigoAplicacion = request.cCodigoAplicacion,
                    cTokenAcceso = Guid.NewGuid().ToString(),
                    cTokenSecreto = Guid.NewGuid().ToString(),
                    dFechaExpiracion = DateTime.UtcNow.AddDays(365),
                    bEsActivo = true
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error renewing application tokens for {AppCode}", request.cCodigoAplicacion);
                throw;
            }
        }

        public async Task<bool> RevocarTokensAplicacionAsync(RevocarTokensRequest request)
        {
            try
            {
                // Implementar revocación de tokens de aplicación
                // Por ahora retornamos true como placeholder
                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error revoking application tokens for {AppCode}", request.cCodigoAplicacion);
                return false;
            }
        }

        public async Task<List<TokenActivoDto>> GetTokensActivosAplicacionAsync(string codigoApp)
        {
            try
            {
                // Implementar obtención de tokens activos
                // Por ahora retornamos una lista vacía como placeholder
                return await Task.FromResult(new List<TokenActivoDto>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting active tokens for app {AppCode}", codigoApp);
                throw;
            }
        }

        public async Task<TokenActivoDto> ObtenerEstadoTokenAsync(string codigoAplicacion, string tokenAcceso)
        {
            try
            {
                // Implementar obtención del estado de un token específico
                return await Task.FromResult(new TokenActivoDto
                {
                    cTokenId = Guid.NewGuid().ToString(),
                    cCodigoAplicacion = codigoAplicacion,
                    cTokenAcceso = tokenAcceso,
                    bEsActivo = true,
                    bEsValido = true,
                    dFechaExpiracion = DateTime.UtcNow.AddDays(30),
                    dFechaCreacion = DateTime.UtcNow.AddDays(-1),
                    nDiasRestantes = 30,
                    cEstado = "Activo"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting token status for {AppCode} - {Token}", codigoAplicacion, tokenAcceso);
                throw;
            }
        }

        public async Task<IEnumerable<TokenActivoDto>> ListarTokensActivosAsync(string codigoAplicacion)
        {
            try
            {
                // Implementar listado de tokens activos
                var tokens = new List<TokenActivoDto>
                {
                    new TokenActivoDto
                    {
                        cTokenId = Guid.NewGuid().ToString(),
                        cCodigoAplicacion = codigoAplicacion,
                        cTokenAcceso = "token_access_1",
                        bEsActivo = true,
                        bEsValido = true,
                        dFechaExpiracion = DateTime.UtcNow.AddDays(30),
                        dFechaCreacion = DateTime.UtcNow.AddDays(-1),
                        nDiasRestantes = 30,
                        cEstado = "Activo"
                    }
                };

                return await Task.FromResult(tokens);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing active tokens for {AppCode}", codigoAplicacion);
                throw;
            }
        }
    }
}