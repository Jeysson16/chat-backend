using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatModularMicroservice.Domain
{

[Table("participanteschat")]
public class ChatParticipante
{
    [Key]
    public long Id { get; set; }

    [Column("nparticipanteschatid")]
    public long nParticipantesChatId { get; set; }

    [Column("nparticipanteschatconversacionid")]
    public long nParticipantesChatConversacionId { get; set; }

    [Column("cparticipanteschatusuarioid")]
    public string cParticipantesChatUsuarioId { get; set; } = string.Empty;

    [Column("cparticipanteschatrol")]
    public string cParticipantesChatRol { get; set; } = "member";

    [Column("dparticipanteschatfechaunion")]
    public DateTime dParticipantesChatFechaUnion { get; set; } = DateTime.UtcNow;

    [Column("bparticipanteschatestaactivo")]
    public bool bParticipantesChatEstaActivo { get; set; } = true;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ChatUsuario? ChatUsuario { get; set; }
    public virtual ChatConversacion? ChatConversacion { get; set; }
}

}