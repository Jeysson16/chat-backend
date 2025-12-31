using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace ChatModularMicroservice.Infrastructure.SupabaseModels
{
    [Table("Mensajes")]
    public class MensajeSupabaseFull : BaseModel
    {
        [PrimaryKey("nMensajesId", false)]
        [Column("nMensajesId")]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull)]
        public int? nMensajesId { get; set; }

        [Column("nMensajesConversacionId")]
        public int nMensajesConversacionId { get; set; }

        [Column("cMensajesRemitenteId")]
        public string? cMensajesRemitenteId { get; set; }

        [Column("cMensajesTexto")]
        public string? cMensajesTexto { get; set; }

        [Column("cMensajesTipo")]
        public string? cMensajesTipo { get; set; }

        [Column("dMensajesFechaCreacion")]
        public System.DateTime? dMensajesFechaCreacion { get; set; }

        [Column("bMensajesEsLeido")]
        public bool? bMensajesEsLeido { get; set; }
    }
}
