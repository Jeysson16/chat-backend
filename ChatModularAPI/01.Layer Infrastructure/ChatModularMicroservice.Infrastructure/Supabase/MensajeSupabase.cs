using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using System.Text.Json.Serialization;

namespace ChatModularMicroservice.Domain
{
    [Table("Mensajes")]
    public class MensajeSupabase : BaseModel
    {
        [PrimaryKey("nMensajesId", false)]
        [Column("nMensajesId")]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public int? nMensajesId { get; set; }

        [Column("nMensajesConversacionId")]
        public int nMensajesConversacionId { get; set; }

        [Column("dMensajesFechaCreacion")]
        public System.DateTime? dMensajesFechaCreacion { get; set; }
    }
}
