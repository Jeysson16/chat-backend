using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.Extensions.Logging;
using ChatModularMicroservice.Repository;

namespace ChatModularMicroservice.Domain.Services;

/// <summary>
/// Servicio de autorización para gestionar roles y permisos
/// </summary>
public class AuthorizationService : IAuthorizationService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IChatRepository _chatRepository;
    private readonly IContactoRepository _contactoRepository;
    private readonly IAppRegistroRepository _appRegistroRepository;
    private readonly ILogger<AuthorizationService> _logger;

    // Definición de roles del sistema
    public static class Roles
    {
        public const string SuperAdmin = "SUPER_ADMIN";
        public const string Admin = "ADMIN";
        public const string Moderator = "MODERATOR";
        public const string User = "USER";
        public const string Guest = "GUEST";
    }

    // Definición de permisos del sistema
    public static class Permissions
    {
        public const string ManageUsers = "MANAGE_USERS";
        public const string ManageConversations = "MANAGE_CONVERSATIONS";
        public const string SendMessages = "SEND_MESSAGES";
        public const string ViewProfiles = "VIEW_PROFILES";
        public const string SendContactRequests = "SEND_CONTACT_REQUESTS";
        public const string ManageWebhooks = "MANAGE_WEBHOOKS";
        public const string ConfigureCompany = "CONFIGURE_COMPANY";
        public const string ViewReports = "VIEW_REPORTS";
        public const string ManageRoles = "MANAGE_ROLES";
    }

    public AuthorizationService(
        IUsuarioRepository usuarioRepository,
        IChatRepository chatRepository,
        IContactoRepository contactoRepository,
        IAppRegistroRepository appRegistroRepository,
        ILogger<AuthorizationService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _chatRepository = chatRepository;
        _contactoRepository = contactoRepository;
        _appRegistroRepository = appRegistroRepository;
        _logger = logger;
    }

    /// <summary>
    /// Verifica si un usuario tiene un rol específico
    /// </summary>
    public async Task<bool> HasRoleAsync(string userId, string role)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
                return false;

            return usuario.cUsuariosChatRol?.Equals(role, StringComparison.OrdinalIgnoreCase) == true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando rol {Role} para usuario {UserId}", role, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario tiene un permiso específico
    /// </summary>
    public async Task<bool> HasPermissionAsync(string userId, string permission)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
                return false;

            // Mapear roles a permisos
            var userRole = usuario.cUsuariosChatRol;
            return HasPermissionForRole(userRole, permission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso {Permission} para usuario {UserId}", permission, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede acceder a una conversación
    /// </summary>
    public async Task<bool> CanAccessConversationAsync(string userId, long conversationId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            // Los administradores pueden acceder a cualquier conversación
            if (await HasRoleAsync(userId, Roles.Admin) || await HasRoleAsync(userId, Roles.SuperAdmin))
                return true;

            // Verificar si el usuario es participante de la conversación
            var isParticipant = await _chatRepository.IsUserInConversationAsync(conversationId, userGuid);
            return isParticipant;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando acceso a conversación {ConversationId} para usuario {UserId}", conversationId, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede enviar mensajes a una conversación
    /// </summary>
    public async Task<bool> CanSendMessageToConversationAsync(string userId, long conversationId)
    {
        try
        {
            // Primero verificar si puede acceder a la conversación
            if (!await CanAccessConversationAsync(userId, conversationId))
                return false;

            // Verificar si tiene permisos para enviar mensajes
            return await HasPermissionAsync(userId, Permissions.SendMessages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso para enviar mensaje a conversación {ConversationId} para usuario {UserId}", conversationId, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede administrar una conversación
    /// </summary>
    public async Task<bool> CanManageConversationAsync(string userId, long conversationId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            // Los administradores pueden gestionar cualquier conversación
            if (await HasRoleAsync(userId, Roles.Admin) || await HasRoleAsync(userId, Roles.SuperAdmin))
                return true;

            // Verificar si es el creador de la conversación
            var conversation = await _chatRepository.GetConversationByIdAsync(conversationId);
            if (conversation != null && conversation.cConversacionesChatUsuarioCreadorId == userId)
                return true;

            // Los moderadores pueden gestionar conversaciones en las que participan
            if (await HasRoleAsync(userId, Roles.Moderator))
            {
                return await _chatRepository.IsUserInConversationAsync(conversationId, userGuid);
            }

            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso para gestionar conversación {ConversationId} para usuario {UserId}", conversationId, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede ver el perfil de otro usuario
    /// </summary>
    public async Task<bool> CanViewUserProfileAsync(string requestingUserId, string targetUserId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(requestingUserId) || string.IsNullOrWhiteSpace(targetUserId))
                return false;

            // Un usuario siempre puede ver su propio perfil
            if (requestingUserId.Equals(targetUserId, StringComparison.OrdinalIgnoreCase))
                return true;

            // Los administradores pueden ver cualquier perfil
            if (await HasRoleAsync(requestingUserId, Roles.Admin) || await HasRoleAsync(requestingUserId, Roles.SuperAdmin))
                return true;

            // Verificar si tienen permisos para ver perfiles
            if (!await HasPermissionAsync(requestingUserId, Permissions.ViewProfiles))
                return false;

            // Verificar si son contactos
            if (!Guid.TryParse(requestingUserId, out var requestingUserGuid) || 
                !Guid.TryParse(targetUserId, out var targetUserGuid))
                return false;

            var areContacts = await _contactoRepository.AreContactsAsync(requestingUserGuid, targetUserGuid);
            return areContacts;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso para ver perfil de usuario {TargetUserId} por usuario {RequestingUserId}", targetUserId, requestingUserId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede enviar solicitudes de contacto
    /// </summary>
    public async Task<bool> CanSendContactRequestAsync(string userId, string targetUserId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(targetUserId))
                return false;

            // No puede enviarse solicitud a sí mismo
            if (userId.Equals(targetUserId, StringComparison.OrdinalIgnoreCase))
                return false;

            // Verificar permisos
            if (!await HasPermissionAsync(userId, Permissions.SendContactRequests))
                return false;

            // Verificar si ya son contactos o hay solicitud pendiente
            if (!Guid.TryParse(userId, out var userGuid) || 
                !Guid.TryParse(targetUserId, out var targetUserGuid))
                return false;

            var areContacts = await _contactoRepository.AreContactsAsync(userGuid, targetUserGuid);
            if (areContacts)
                return false;

            var hasPendingRequest = await _contactoRepository.HasPendingRequestAsync(userGuid, targetUserGuid);
            return !hasPendingRequest;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso para enviar solicitud de contacto a {TargetUserId} por usuario {UserId}", targetUserId, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede acceder a los webhooks de una aplicación
    /// </summary>
    public async Task<bool> CanAccessWebhooksAsync(string userId, string appCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(appCode))
                return false;

            // Los super administradores pueden acceder a cualquier webhook
            if (await HasRoleAsync(userId, Roles.SuperAdmin))
                return true;

            // Verificar si tiene permisos para gestionar webhooks
            if (!await HasPermissionAsync(userId, Permissions.ManageWebhooks))
                return false;

            // Verificar si el usuario tiene acceso a la aplicación
            if (!Guid.TryParse(userId, out var userGuid))
                return false;

            var hasAccess = await _appRegistroRepository.UserHasAccessToAppAsync(userGuid, appCode);
            return hasAccess;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando acceso a webhooks de app {AppCode} para usuario {UserId}", appCode, userId);
            return false;
        }
    }

    /// <summary>
    /// Verifica si un usuario puede configurar una empresa
    /// </summary>
    public async Task<bool> CanConfigureCompanyAsync(string userId, Guid companyId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            // Los super administradores pueden configurar cualquier empresa
            if (await HasRoleAsync(userId, Roles.SuperAdmin))
                return true;

            // Verificar si tiene permisos para configurar empresa
            if (!await HasPermissionAsync(userId, Permissions.ConfigureCompany))
                return false;

            // Verificar si el usuario pertenece a la empresa
            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null)
                return false;

            // TODO: Implementar verificación de pertenencia a empresa
            // Por ahora, permitir si es admin
            return await HasRoleAsync(userId, Roles.Admin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso para configurar empresa {CompanyId} para usuario {UserId}", companyId, userId);
            return false;
        }
    }

    /// <summary>
    /// Obtiene los roles de un usuario
    /// </summary>
    public async Task<IEnumerable<string>> GetUserRolesAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return new List<string>();
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                return new List<string>();
            }

            var roles = new List<string>();
            if (!string.IsNullOrWhiteSpace(usuario.cUsuariosChatRol))
            {
                roles.Add(usuario.cUsuariosChatRol);
            }

            return roles;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo roles para usuario {UserId}", userId);
            return new List<string>();
        }
    }

    /// <summary>
    /// Obtiene los permisos de un usuario
    /// </summary>
    public async Task<IEnumerable<string>> GetUserPermissionsAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
            {
                return new List<string>();
            }

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
            {
                return new List<string>();
            }

            var permissions = GetPermissionsForRole(usuario.cUsuariosChatRol);
            return permissions;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo permisos para usuario {UserId}", userId);
            return new List<string>();
        }
    }

    /// <summary>
    /// Verifica si un usuario tiene permisos para acceder a un recurso específico
    /// </summary>
    public async Task<bool> HasPermissionAsync(string userId, string resource, string action)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId) || !Guid.TryParse(userId, out var userGuid))
                return false;

            var usuario = await _usuarioRepository.GetByIdAsync(userGuid);
            if (usuario == null || !usuario.bUsuariosChatEstaActivo)
                return false;

            // Construir el permiso basado en recurso y acción
            var permission = $"{resource}_{action}".ToUpperInvariant();
            
            // Mapear roles a permisos
            var userRole = usuario.cUsuariosChatRol;
            return HasPermissionForRole(userRole, permission);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error verificando permiso {Resource}_{Action} para usuario {UserId}", resource, action, userId);
            return false;
        }
    }

    /// <summary>
    /// Valida si un token tiene permisos para acceder a un recurso
    /// </summary>
    public async Task<bool> ValidateTokenPermissionAsync(string token, string resource, string action)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(token))
                return false;

            // TODO: Implementar validación de token y extracción de userId
            // Por ahora, retornamos false como placeholder
            _logger.LogWarning("ValidateTokenPermissionAsync no está completamente implementado");
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando permisos de token para {Resource}_{Action}", resource, action);
            return false;
        }
    }

    #region Private Methods

    /// <summary>
    /// Verifica si un rol tiene un permiso específico
    /// </summary>
    private bool HasPermissionForRole(string role, string permission)
    {
        if (string.IsNullOrWhiteSpace(role))
            return false;

        var permissions = GetPermissionsForRole(role);
        return permissions.Contains(permission);
    }

    /// <summary>
    /// Obtiene los permisos para un rol específico
    /// </summary>
    private List<string> GetPermissionsForRole(string role)
    {
        return role?.ToUpperInvariant() switch
        {
            Roles.SuperAdmin => new List<string>
            {
                Permissions.ManageUsers,
                Permissions.ManageConversations,
                Permissions.SendMessages,
                Permissions.ViewProfiles,
                Permissions.SendContactRequests,
                Permissions.ManageWebhooks,
                Permissions.ConfigureCompany,
                Permissions.ViewReports,
                Permissions.ManageRoles
            },
            Roles.Admin => new List<string>
            {
                Permissions.ManageConversations,
                Permissions.SendMessages,
                Permissions.ViewProfiles,
                Permissions.SendContactRequests,
                Permissions.ManageWebhooks,
                Permissions.ConfigureCompany,
                Permissions.ViewReports
            },
            Roles.Moderator => new List<string>
            {
                Permissions.ManageConversations,
                Permissions.SendMessages,
                Permissions.ViewProfiles,
                Permissions.SendContactRequests
            },
            Roles.User => new List<string>
            {
                Permissions.SendMessages,
                Permissions.ViewProfiles,
                Permissions.SendContactRequests
            },
            Roles.Guest => new List<string>
            {
                Permissions.ViewProfiles
            },
            _ => new List<string>()
        };
    }

    #endregion
}
