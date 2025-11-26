using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatModularMicroservice.Entities.Models
{

[Table("mensajeschat")]
public class ChatMensaje
{
    [Key]
    public long Id { get; set; }

    [Column("nmensajeschatid")]
    public long nMensajesChatId { get; set; }

    [Column("nmensajeschatconversacionid")]
    public long nMensajesChatConversacionId { get; set; }

    [Column("cmensajeschatremitenteid")]
    public string cMensajesChatRemitenteId { get; set; } = string.Empty;

    [Column("cmensajeschattexto")]
    public string cMensajesChatTexto { get; set; } = string.Empty;

    [Column("cmensajeschattipo")]
    public string cMensajesChatTipo { get; set; } = "text";

    [Column("dmensajeschatfechahora")]
    public DateTime dMensajesChatFechaHora { get; set; } = DateTime.UtcNow;

    [Column("bmensajeschatestaLeido")]
    public bool bMensajesChatEstaLeido { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual ChatUsuario? ChatUsuario { get; set; }
    public virtual ChatConversacion? ChatConversacion { get; set; }
}

}