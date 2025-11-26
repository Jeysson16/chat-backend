using ChatModularMicroservice.Entities.Models;
using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{

[Table("usuarioschat")]
public class SupabaseChatUser : BaseModel
{
    [PrimaryKey("cusuarioschatid")]
    public string cUsuariosChatId { get; set; } = string.Empty;
    
    [Column("cusuarioschatnombre")]
    public string cUsuariosChatNombre { get; set; } = string.Empty;
    
    [Column("cusuarioschatemail")]
    public string? cUsuariosChatEmail { get; set; }
    
    [Column("cusuarioschatavatar")]
    public string? cUsuariosChatAvatar { get; set; }
    
    [Column("busuarioschatestaenlinea")]
    public bool bUsuariosChatEstaEnLinea { get; set; } = false;
    
    [Column("dusuarioschatultimavez")]
    public DateTime? dUsuariosChatUltimaVez { get; set; }
    
    [Column("dusuarioschatfechacreacion")]
    public DateTime dUsuariosChatFechaCreacion { get; set; } = DateTime.UtcNow;
}

}