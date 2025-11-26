using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatModularMicroservice.Domain
{

[Table("conversacioneschat")]
public class ChatConversacion
{
    [Key]
    public long Id { get; set; }

    [Column("nconversacioneschatid")]
    public long nConversacionesChatId { get; set; }

    [Column("cconversacioneschatappcodigo")]
    public string cConversacionesChatAppCodigo { get; set; } = string.Empty;

    [Column("cconversacioneschatnombre")]
    public string? cConversacionesChatNombre { get; set; }

    [Column("cconversacioneschatdescripcion")]
    public string? cConversacionesChatDescripcion { get; set; }

    [Column("cconversacioneschattipo")]
    public string cConversacionesChatTipo { get; set; } = "individual";

    [Column("cconversacioneschatusuariocreadorid")]
    public string? cConversacionesChatUsuarioCreadorId { get; set; }

    [Column("dconversacioneschatfechacreacion")]
    public DateTime dConversacionesChatFechaCreacion { get; set; } = DateTime.UtcNow;

    [Column("dconversacioneschatultimaactividad")]
    public DateTime? dConversacionesChatUltimaActividad { get; set; }

    [Column("bconversacioneschatestaactiva")]
    public bool bConversacionesChatEstaActiva { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ICollection<ChatUsuarioConversacion> ChatUsuarioConversaciones { get; set; } = new List<ChatUsuarioConversacion>();
    public virtual ICollection<ChatMensaje> ChatMensajes { get; set; } = new List<ChatMensaje>();
}

}