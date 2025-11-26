using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de tokens de aplicación
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Genera nuevos tokens para una aplicación
        /// </summary>
        /// <param name="request">Datos para generar los tokens</param>
        /// <returns>Tokens generados</returns>
        Task<TokensAplicacionDto> GenerarTokensAplicacionAsync(GenerarTokensRequest request);

        /// <summary>
        /// Valida tokens de una aplicación
        /// </summary>
        /// <param name="request">Datos para validar los tokens</param>
        /// <returns>Resultado de la validación</returns>
        Task<ValidacionTokenDto> ValidarTokensAplicacionAsync(ValidarTokenRequest request);

        /// <summary>
        /// Renueva tokens de una aplicación
        /// </summary>
        /// <param name="request">Datos para renovar los tokens</param>
        /// <returns>Nuevos tokens generados</returns>
        Task<TokensAplicacionDto> RenovarTokensAplicacionAsync(RenovarTokensRequest request);

        /// <summary>
        /// Revoca tokens de una aplicación
        /// </summary>
        /// <param name="request">Datos para revocar los tokens</param>
        /// <returns>Resultado de la revocación</returns>
        Task<bool> RevocarTokensAplicacionAsync(RevocarTokensRequest request);

        /// <summary>
        /// Obtiene el estado de un token activo
        /// </summary>
        /// <param name="codigoAplicacion">Código de la aplicación</param>
        /// <param name="tokenAcceso">Token de acceso</param>
        /// <returns>Estado del token</returns>
        Task<TokenActivoDto> ObtenerEstadoTokenAsync(string codigoAplicacion, string tokenAcceso);

        /// <summary>
        /// Lista todos los tokens activos de una aplicación
        /// </summary>
        /// <param name="codigoAplicacion">Código de la aplicación</param>
        /// <returns>Lista de tokens activos</returns>
        Task<IEnumerable<TokenActivoDto>> ListarTokensActivosAsync(string codigoAplicacion);

        /// <summary>
        /// Obtiene los tokens activos de una aplicación (alias utilizado por controladores)
        /// </summary>
        /// <param name="codigoApp">Código de la aplicación</param>
        /// <returns>Lista de tokens activos</returns>
        Task<List<TokenActivoDto>> GetTokensActivosAplicacionAsync(string codigoApp);

        /// <summary>
        /// Valida un token JWT y devuelve el ClaimsPrincipal
        /// </summary>
        /// <param name="token">Token JWT a validar</param>
        /// <returns>ClaimsPrincipal si el token es válido, null si no</returns>
        Task<System.Security.Claims.ClaimsPrincipal?> ValidateJwtTokenAsync(string token);

        /// <summary>
        /// Valida el token/código de acceso de una aplicación
        /// </summary>
        /// <param name="appCode">Código de la aplicación</param>
        /// <returns>True si el código de aplicación es válido</returns>
        Task<bool> ValidateApplicationTokenAsync(string appCode);
    }
}