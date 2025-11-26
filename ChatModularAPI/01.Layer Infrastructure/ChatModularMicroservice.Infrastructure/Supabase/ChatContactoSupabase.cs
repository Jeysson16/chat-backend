using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{
    [Table("ChatContacto")]
    public class ChatContactoSupabase : BaseModel
    {
        [PrimaryKey("nContactoId", false)]
        [Column("nContactoId")]
        public string nContactoId { get; set; } = string.Empty;

        [Column("nAplicacionesId")]
        public int nAplicacionesId { get; set; }

        [Column("cUsuarioId")]
        public string cUsuarioId { get; set; } = string.Empty;

        [Column("cContactoUsuarioId")]
        public string cContactoUsuarioId { get; set; } = string.Empty;

        [Column("cNombreContacto")]
        public string? cNombreContacto { get; set; }

        [Column("cEmailContacto")]
        public string? cEmailContacto { get; set; }

        [Column("cTelefonoContacto")]
        public string? cTelefonoContacto { get; set; }

        [Column("cAvatarContacto")]
        public string? cAvatarContacto { get; set; }

        [Column("cEstadoContacto")]
        public string? cEstadoContacto { get; set; }

        [Column("bEsFavorito")]
        public bool? bEsFavorito { get; set; }

        [Column("cNotasContacto")]
        public string? cNotasContacto { get; set; }

        [Column("dFechaAgregado")]
        public DateTime? dFechaAgregado { get; set; }

        [Column("dFechaUltimaInteraccion")]
        public DateTime? dFechaUltimaInteraccion { get; set; }

        [Column("dFechaCreacion")]
        public DateTime? dFechaCreacion { get; set; }

        [Column("dFechaModificacion")]
        public DateTime? dFechaModificacion { get; set; }

        [Column("bEsActivo")]
        public bool? bEsActivo { get; set; }
    }
}