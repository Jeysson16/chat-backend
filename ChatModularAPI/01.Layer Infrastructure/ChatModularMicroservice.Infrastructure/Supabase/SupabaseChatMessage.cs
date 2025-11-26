using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

[Table("mensajeschat")]
public class SupabaseChatMessage : BaseModel
{
    [PrimaryKey("nmensajeschatid")]
    public int nMensajesChatId { get; set; }
    
    [Column("nmensajeschatconversacionid")]
    public int nMensajesChatConversacionId { get; set; }
    
    [Column("cmensajeschatremitenteid")]
    public string cMensajesChatRemitenteId { get; set; } = string.Empty;
    
    [Column("cmensajeschattexto")]
    public string cMensajesChatTexto { get; set; } = string.Empty;
    
    [Column("cmensajeschattipo")]
    public string cMensajesChatTipo { get; set; } = "text";
    
    [Column("dmensajeschatfechahora")]
    public DateTime dMensajesChatFechaHora { get; set; } = DateTime.UtcNow;
    
    [Column("bmensajeschatestaleido")]
    public bool bMensajesChatEstaLeido { get; set; } = false;
}
}
