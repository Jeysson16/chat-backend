using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Infrastructure.SupabaseModels
{
    [Table("Conversaciones")]
    public class ConversacionSupabase : BaseModel
    {
        [PrimaryKey("nConversacionesId")]
        [Column("nConversacionesId")]
        public int nConversacionesId { get; set; }

        [Column("cConversacionesNombre")]
        public string? cConversacionesNombre { get; set; }

        [Column("cConversacionesTipo")]
        public string cConversacionesTipo { get; set; } = "individual";

        [Column("dConversacionesFechaCreacion")]
        public System.DateTime dConversacionesFechaCreacion { get; set; }

        [Column("dConversacionesFechaActualizacion")]
        public System.DateTime? dConversacionesFechaActualizacion { get; set; }

        [Column("bConversacionesEsActiva")]
        public bool bConversacionesEsActiva { get; set; } = true;
    }
}

