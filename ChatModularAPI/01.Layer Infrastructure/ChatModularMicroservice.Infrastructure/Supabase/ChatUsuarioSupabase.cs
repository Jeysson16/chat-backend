using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

/// <summary>
/// Modelo temporal para mantener compatibilidad con Supabase
/// Mapea a la tabla ChatUsuarios existente
/// </summary>
[Table("ChatUsuarios")]
public class ChatUsuarioSupabase : BaseModel
{
    [PrimaryKey("Id")]
    public Guid Id { get; set; }

    [Column("c_per_jur_codigo")]
    public string cPerJurCodigo { get; set; } = string.Empty;

    [Column("c_per_codigo")]
    public string cPerCodigo { get; set; } = string.Empty;

    [Column("c_app_codigo")]
    public string cAppCodigo { get; set; } = string.Empty;

    [Column("c_usuario_nombre")]
    public string cUsuarioNombre { get; set; } = string.Empty;

    [Column("c_usuario_email")]
    public string cUsuarioEmail { get; set; } = string.Empty;

    [Column("d_usuario_ultima_conexion")]
    public DateTime? dUsuarioUltimaConexion { get; set; }

    [Column("b_usuario_activo")]
    public bool bUsuarioActivo { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [Column("c_usuario_rol")]
    public string cUsuarioRol { get; set; } = "USER";

    [Column("c_usuario_username")]
    public string cUsuarioUsername { get; set; } = string.Empty;

    [Column("c_usuario_password")]
    public string cUsuarioPassword { get; set; } = string.Empty;

    [Column("c_usuario_avatar")]
    public string? cUsuarioAvatar { get; set; }

    [Column("d_usuario_creacion")]
    public DateTime dUsuarioCreacion { get; set; } = DateTime.UtcNow;

    [Column("b_usuario_online")]
    public bool bUsuarioOnline { get; set; } = false;

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
}
}
