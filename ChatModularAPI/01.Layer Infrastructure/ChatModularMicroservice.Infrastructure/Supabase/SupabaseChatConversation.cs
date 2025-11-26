using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

[Table("conversacioneschat")]
public class SupabaseChatConversation : BaseModel
{
    [PrimaryKey("nconversacioneschatid")]
    public int nConversacionesChatId { get; set; }
    
    [Column("cconversacioneschatnombre")]
    public string? cConversacionesChatNombre { get; set; }
    
    [Column("cconversacioneschattipo")]
    public string cConversacionesChatTipo { get; set; } = "individual";
    
    [Column("dconversacioneschatfechacreacion")]
    public DateTime dConversacionesChatFechaCreacion { get; set; } = DateTime.UtcNow;
    
    [Column("dconversacioneschatfechaactualizacion")]
    public DateTime? dConversacionesChatFechaActualizacion { get; set; }
    
    [Column("bconversacioneschatestaactiva")]
    public bool bConversacionesChatEstaActiva { get; set; } = true;
}
}
