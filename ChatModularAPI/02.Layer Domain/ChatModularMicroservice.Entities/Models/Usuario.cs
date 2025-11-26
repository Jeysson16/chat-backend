namespace ChatModularMicroservice.Entities.Models;

/// <summary>
/// Entidad Usuario
/// </summary>
public class Usuario
{
    /// <summary>
    /// ID único del usuario
    /// </summary>
    public int cUsuariosId { get; set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string cNombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public string cApellido { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario
    /// </summary>
    public string cEmail { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del usuario
    /// </summary>
    public string? cTelefono { get; set; }

    /// <summary>
    /// Avatar del usuario
    /// </summary>
    public string? cAvatar { get; set; }

    /// <summary>
    /// Estado del usuario
    /// </summary>
    public bool bActivo { get; set; } = true;

    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime dFechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID de la empresa
    /// </summary>
    public int nEmpresaId { get; set; }

    /// <summary>
    /// ID de la aplicación
    /// </summary>
    public int nAplicacionId { get; set; }

    /// <summary>
    /// Fecha de última conexión al chat
    /// </summary>
    public DateTime? dUsuariosChatUltimaConexion { get; set; }

    /// <summary>
    /// Indica si el usuario está activo en el chat
    /// </summary>
    public bool bUsuariosChatEstaActivo { get; set; } = true;

    /// <summary>
    /// Indica si el usuario está en línea
    /// </summary>
    public bool bUsuariosChatEstaEnLinea { get; set; } = false;

    /// <summary>
    /// Fecha de creación en el chat
    /// </summary>
    public DateTime dUsuariosChatFechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID del usuario en el chat
    /// </summary>
    public string cUsuariosChatId { get; set; } = string.Empty;

    /// <summary>
    /// Nombre del usuario en el chat
    /// </summary>
    public string cUsuariosChatNombre { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario en el chat
    /// </summary>
    public string cUsuariosChatEmail { get; set; } = string.Empty;

    /// <summary>
    /// Fecha de actualización
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Contraseña del usuario en el chat
    /// </summary>
    public string cUsuariosChatPassword { get; set; } = string.Empty;

    /// <summary>
    /// Fecha del último cambio de contraseña
    /// </summary>
    public DateTime? dUsuarioCambioPassword { get; set; }

    /// <summary>
    /// Configuración de privacidad del usuario
    /// </summary>
    public string? cUsuarioConfigPrivacidad { get; set; }

    /// <summary>
    /// Configuración de notificaciones del usuario
    /// </summary>
    public string? cUsuarioConfigNotificaciones { get; set; }

    /// <summary>
    /// Avatar del usuario en el chat
    /// </summary>
    public string? cUsuariosChatAvatar { get; set; }

    /// <summary>
    /// Indica si el usuario está verificado
    /// </summary>
    public bool? bUsuarioVerificado { get; set; }

    /// <summary>
    /// Nombre de usuario para el chat
    /// </summary>
    public string? cUsuariosChatUsername { get; set; }

    /// <summary>
    /// Rol del usuario en el chat
    /// </summary>
    public string? cUsuariosChatRol { get; set; }

    /// <summary>
    /// Token de verificación del usuario
    /// </summary>
    public string? cUsuarioTokenVerificacion { get; set; }

    /// <summary>
    /// Código de persona jurídica
    /// </summary>
    public string? cUsuariosChatPerJurCodigo { get; set; }

    /// <summary>
    /// Código de persona
    /// </summary>
    public string? cUsuariosChatPerCodigo { get; set; }

    /// <summary>
    /// Código de aplicación
    /// </summary>
    public string? cUsuariosChatAppCodigo { get; set; }

    /// <summary>
    /// Token de reset de password
    /// </summary>
    public string? cUsuarioTokenReset { get; set; }

    /// <summary>
    /// Fecha de expiración del token de reset
    /// </summary>
    public DateTime? dUsuarioTokenResetExpiracion { get; set; }
}