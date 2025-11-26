using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

[Table("participanteschat")]
public class SupabaseChatParticipant : BaseModel
{
    [PrimaryKey("nparticipanteschatid")]
    public int nParticipantesChatId { get; set; }
    
    [Column("nparticipanteschatconversacionid")]
    public int nParticipantesChatConversacionId { get; set; }
    
    [Column("cparticipanteschatusuarioid")]
    public string cParticipantesChatUsuarioId { get; set; } = string.Empty;
    
    [Column("cparticipanteschatrol")]
    public string cParticipantesChatRol { get; set; } = "member";
    
    [Column("dparticipanteschatfechaunion")]
    public DateTime dParticipantesChatFechaUnion { get; set; } = DateTime.UtcNow;
    
    [Column("bparticipanteschatestaactivo")]
    public bool bParticipantesChatEstaActivo { get; set; } = true;
}
}
