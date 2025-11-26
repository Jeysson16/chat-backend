using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models;

[Table("participanteschat")]
public class ChatUsuarioConversacion : BaseModel
{
    [PrimaryKey("nparticipanteschatid")]
    public int Id { get; set; }
        
    [Column("cparticipanteschatusuarioid")]
    public string cParticipantesChatUsuarioId { get; set; } = string.Empty;
        
    [Column("nparticipanteschatconversacionid")]
    public int nParticipantesChatConversacionId { get; set; }
    
    [Column("dparticipanteschatfechaunion")]
    public DateTime dParticipantesChatFechaUnion { get; set; } = DateTime.UtcNow;
    
    [Column("dparticipanteschatultimalectura")]
    public DateTime? dParticipantesChatUltimaLectura { get; set; }
    
    [Column("bparticipanteschatestaactivo")]
    public bool bParticipantesChatEstaActivo { get; set; } = true;
    
    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Column("updated_at")]
    public DateTime? UpdatedAt { get; set; }
    
    // Propiedades de navegación
    public virtual ChatUsuario? ChatUsuario { get; set; }
    public virtual ChatConversacion? ChatConversacion { get; set; }
}
