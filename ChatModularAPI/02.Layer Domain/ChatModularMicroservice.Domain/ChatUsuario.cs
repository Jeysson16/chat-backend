using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatModularMicroservice.Domain
{

[Table("usuarioschat")]
public class ChatUsuario
{
    [Key]
    public Guid Id { get; set; }

    [Column("cusuarioschatid")]
    public string cUsuariosChatId { get; set; } = string.Empty;

    [Column("cusuarioschatperjurcodigo")]
    public string cUsuariosChatPerJurCodigo { get; set; } = string.Empty;

    [Column("cusuarioschatpercodigo")]
    public string cUsuariosChatPerCodigo { get; set; } = string.Empty;

    [Column("cusuarioschatappcodigo")]
    public string cUsuariosChatAppCodigo { get; set; } = string.Empty;

    [Column("cusuarioschatnombre")]
    public string cUsuariosChatNombre { get; set; } = string.Empty;

    [Column("cusuarioschatemail")]
    public string cUsuariosChatEmail { get; set; } = string.Empty;

    [Column("dusuarioschatultimaconexion")]
    public DateTime? dUsuariosChatUltimaConexion { get; set; }

    [Column("busuarioschatestaactivo")]
    public bool bUsuariosChatEstaActivo { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("cusuarioschatrol")]
    public string cUsuariosChatRol { get; set; } = "USER";

    [Column("cusuarioschatusername")]
    public string cUsuariosChatUsername { get; set; } = string.Empty;

    [Column("cusuarioschatpassword")]
    public string cUsuariosChatPassword { get; set; } = string.Empty;

    [Column("cusuarioschatavatar")]
    public string? cUsuariosChatAvatar { get; set; }

    [Column("dusuarioschatfechacreacion")]
    public DateTime dUsuariosChatFechaCreacion { get; set; } = DateTime.UtcNow;

    [Column("busuarioschatestaenlinea")]
    public bool bUsuariosChatEstaEnLinea { get; set; } = false;

    [Column("b_usuario_verificado")]
    public bool bUsuarioVerificado { get; set; } = false;

    [Column("c_usuario_token_verificacion")]
    public string? cUsuarioTokenVerificacion { get; set; }

    [Column("c_usuario_token_reset")]
    public string? cUsuarioTokenReset { get; set; }

    [Column("d_usuario_token_reset_expiracion")]
    public DateTime? dUsuarioTokenResetExpiracion { get; set; }

    [Column("d_usuario_cambio_password")]
    public DateTime? dUsuarioCambioPassword { get; set; }

    [Column("c_usuario_config_privacidad")]
    public string? cUsuarioConfigPrivacidad { get; set; }

    [Column("c_usuario_config_notificaciones")]
    public string? cUsuarioConfigNotificaciones { get; set; }

    // Propiedad para almacenar el ID original de la tabla Usuarios
    public string? nUsuariosId { get; set; }

    // Navigation properties
    public virtual ICollection<ChatUsuarioConversacion> ChatUsuarioConversaciones { get; set; } = new List<ChatUsuarioConversacion>();
    public virtual ICollection<ChatMensaje> ChatMensajes { get; set; } = new List<ChatMensaje>();
}

}