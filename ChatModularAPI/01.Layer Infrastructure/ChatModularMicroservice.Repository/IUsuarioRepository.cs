using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de usuarios
/// </summary>
public interface IUsuarioRepository : IDeleteIntRepository, IInsertIntRepository<Usuario>, IUpdateRepository<Usuario>
{
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByIdAsync(Guid id);
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByEmailAsync(string email);
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByUsernameAsync(string username);
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByPerCodigoAsync(string perCodigo);
    Task<ChatModularMicroservice.Entities.Models.Usuario> CreateAsync(ChatModularMicroservice.Entities.Models.Usuario usuario);
    Task<ChatModularMicroservice.Entities.Models.Usuario> UpdateAsync(ChatModularMicroservice.Entities.Models.Usuario usuario);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByUsernameAsync(string username);
    Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> GetAllAsync();
    Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> GetByFilterAsync(UsuarioFilter filter);
    Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> SearchUsersAsync(string searchTerm);
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByUsuariosChatIdAsync(string cUsuariosChatId);
    /// <summary>
    /// Actualiza la última conexión del usuario
    /// </summary>
    Task<bool> UpdateLastConnectionAsync(string userId);

    /// <summary>
    /// Establece el estado activo del usuario
    /// </summary>
    Task<bool> SetActiveStatusAsync(string userId, bool isActive);

    /// <summary>
    /// Obtiene un usuario por su token de reset de contraseña
    /// </summary>
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByResetTokenAsync(string resetToken);

    /// <summary>
    /// Obtiene un usuario por su token de verificación
    /// </summary>
    Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByVerificationTokenAsync(string verificationToken);

    /// <summary>
    /// Genera un token de reset de contraseña para un usuario
    /// </summary>
    Task<string> GeneratePasswordResetTokenAsync(string email);

    /// <summary>
    /// Confirma el reset de contraseña usando un token
    /// </summary>
    Task<bool> ConfirmPasswordResetAsync(string token, string newPassword);

    /// <summary>
    /// Desactiva un usuario
    /// </summary>
    Task<bool> DeactivateUsuarioAsync(string usuarioId);

    /// <summary>
    /// Actualiza la contraseña de un usuario
    /// </summary>
    Task<bool> UpdatePasswordAsync(string usuarioId, string newPassword);

    /// <summary>
    /// Activa un usuario
    /// </summary>
    Task<bool> ActivateUsuarioAsync(string usuarioId);

    // === MÉTODOS ESTÁNDAR DEL PATRÓN ===
    
    /// <summary>
    /// Obtiene un usuario específico según el filtro y tipo de filtro
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <returns>Usuario encontrado</returns>
    Task<Usuario> GetItem(UsuarioFilter filter, UsuarioFilterItemType filterType);

    /// <summary>
    /// Obtiene una lista de usuarios según el filtro, tipo de filtro y paginación
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <param name="pagination">Configuración de paginación</param>
    /// <returns>Lista de usuarios encontrados</returns>
    Task<IEnumerable<Usuario>> GetLstItem(UsuarioFilter filter, UsuarioFilterListType filterType, Utils.Pagination pagination);
    
    /// <summary>
    /// Obtiene un usuario por su código de usuario
    /// </summary>
    /// <param name="userCode">Código del usuario</param>
    /// <returns>Usuario encontrado o null si no existe</returns>
    Task<Usuario?> GetByUserCodeAsync(string userCode);
}