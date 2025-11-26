using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de autorización
    /// </summary>
    public interface IAuthorizationService
    {
        /// <summary>
        /// Verifica si un usuario tiene permisos para acceder a un recurso
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="resource">Recurso al que se quiere acceder</param>
        /// <param name="action">Acción que se quiere realizar</param>
        /// <returns>True si tiene permisos</returns>
        Task<bool> HasPermissionAsync(string userId, string resource, string action);

        /// <summary>
        /// Obtiene todos los permisos de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de permisos</returns>
        Task<IEnumerable<string>> GetUserPermissionsAsync(string userId);

        /// <summary>
        /// Verifica si un token tiene permisos para acceder a un recurso
        /// </summary>
        /// <param name="token">Token de acceso</param>
        /// <param name="resource">Recurso al que se quiere acceder</param>
        /// <param name="action">Acción que se quiere realizar</param>
        /// <returns>True si tiene permisos</returns>
        Task<bool> ValidateTokenPermissionAsync(string token, string resource, string action);

        /// <summary>
        /// Obtiene los roles de un usuario
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <returns>Lista de roles</returns>
        Task<IEnumerable<string>> GetUserRolesAsync(string userId);

        /// <summary>
        /// Verifica si un usuario tiene un rol específico
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="role">Rol a verificar</param>
        /// <returns>True si tiene el rol</returns>
        Task<bool> HasRoleAsync(string userId, string role);
    }
}