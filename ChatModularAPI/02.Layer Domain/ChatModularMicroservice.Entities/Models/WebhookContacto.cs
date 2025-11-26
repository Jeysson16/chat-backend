using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("WebhookContactos")]
    public class WebhookContacto : BaseModel
    {
        [PrimaryKey("cWebhookContactoId")]
        public string cWebhookContactoId { get; set; } = string.Empty;

        [Column("cWebhookId")]
        [Required]
        [StringLength(100)]
        public string cWebhookId { get; set; } = string.Empty;

        [Column("cContactoId")]
        [Required]
        [StringLength(100)]
        public string cContactoId { get; set; } = string.Empty;

        [Column("cTipoEvento")]
        [Required]
        [StringLength(100)]
        public string cTipoEvento { get; set; } = string.Empty;

        [Column("cPayload")]
        public string? cPayload { get; set; }

        [Column("bWebhookContactoActivo")]
        [Required]
        public bool bWebhookContactoActivo { get; set; } = true;

        [Column("dFechaCreacion")]
        [Required]
        public DateTime dFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("dFechaModificacion")]
        public DateTime? dFechaModificacion { get; set; }

        [Column("nIntentos")]
        public int nIntentos { get; set; } = 0;

        [Column("dUltimoIntento")]
        public DateTime? dUltimoIntento { get; set; }

        [Column("cEstado")]
        [StringLength(50)]
        public string cEstado { get; set; } = "PENDIENTE";

        [Column("cMensajeError")]
        public string? cMensajeError { get; set; }
    }
}