using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de autenticación
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Autentica un usuario con email y contraseña
        /// </summary>
        /// <param name="loginDto">Datos de login</param>
        /// <returns>Respuesta con token de acceso</returns>
        Task<Utils.ItemResponseDT> LoginAsync(LoginDto loginDto);

        /// <summary>
        /// Registra un nuevo usuario
        /// </summary>
        /// <param name="registerDto">Datos de registro</param>
        /// <returns>Respuesta con información del usuario creado</returns>
        Task<Utils.ItemResponseDT> RegisterAsync(RegisterDto registerDto);

        /// <summary>
        /// Refresca el token de acceso
        /// </summary>
        /// <param name="refreshToken">Token de refresco</param>
        /// <returns>Nuevo token de acceso</returns>
        Task<Utils.ItemResponseDT> RefreshTokenAsync(string refreshToken);

        /// <summary>
        /// Cierra la sesión del usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Resultado de la operación</returns>
        Task<Utils.ItemResponseDT> LogoutAsync(string userId);

        /// <summary>
        /// Cambia la contraseña del usuario
        /// </summary>
        /// <param name="changePasswordDto">Datos para cambio de contraseña</param>
        /// <returns>Resultado de la operación</returns>
        Task<Utils.ItemResponseDT> ChangePasswordAsync(ChangePasswordDto changePasswordDto);

        /// <summary>
        /// Valida un token de acceso
        /// </summary>
        /// <param name="token">Token a validar</param>
        /// <returns>True si el token es válido</returns>
        Task<bool> ValidateTokenAsync(string token);

        /// <summary>
        /// Obtiene información del usuario por token
        /// </summary>
        /// <param name="token">Token de acceso</param>
        /// <returns>Información del usuario</returns>
        Task<Utils.ItemResponseDT> GetUserByTokenAsync(string token);
    }
}