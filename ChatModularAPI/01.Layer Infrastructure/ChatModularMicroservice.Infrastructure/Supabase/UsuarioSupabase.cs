using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

/// <summary>
/// Modelo para la tabla Usuarios en Supabase
/// </summary>
[Table("Usuarios")]
public class UsuarioSupabase : BaseModel
{
    [PrimaryKey("nUsuariosId")]
    [Column("nUsuariosId")]
    public string nUsuariosId { get; set; } = string.Empty;

    [Column("cUsuariosNombre")]
    public string cUsuariosNombre { get; set; } = string.Empty;

    [Column("cUsuariosEmail")]
    public string cUsuariosEmail { get; set; } = string.Empty;

    [Column("cUsuariosAvatar")]
    public string? cUsuariosAvatar { get; set; }

    [Column("bUsuariosEstaEnLinea")]
    public bool bUsuariosEstaEnLinea { get; set; } = false;

    [Column("dUsuariosUltimaConexion")]
    public DateTime? dUsuariosUltimaConexion { get; set; }

    [Column("dUsuariosFechaCreacion")]
    public DateTime dUsuariosFechaCreacion { get; set; } = DateTime.UtcNow;

    [Column("cUsuariosPerCodigo")]
    public string? cUsuariosPerCodigo { get; set; }

    [Column("cUsuariosPerJurCodigo")]
    public string? cUsuariosPerJurCodigo { get; set; }

    [Column("cUsuariosUsername")]
    public string? cUsuariosUsername { get; set; }

    [Column("cUsuariosPassword")]
    public string? cUsuariosPassword { get; set; }

    [Column("bUsuariosActivo")]
    public bool bUsuariosActivo { get; set; } = true;

    [Column("bUsuarioVerificado")]
    public bool bUsuarioVerificado { get; set; } = false;

    [Column("cUsuarioTokenVerificacion")]
    public string? cUsuarioTokenVerificacion { get; set; }

    [Column("dUsuarioCambioPassword")]
    public DateTime? dUsuarioCambioPassword { get; set; }

    [Column("cUsuarioConfigPrivacidad")]
    public string? cUsuarioConfigPrivacidad { get; set; }

    [Column("cUsuarioConfigNotificaciones")]
    public string? cUsuarioConfigNotificaciones { get; set; }
}
}
