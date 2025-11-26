using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("Webhooks")]
    public class Webhook : BaseModel
    {
        [PrimaryKey("cWebhookId")]
        public string cWebhookId { get; set; } = string.Empty;

        [Column("cWebhookUrl")]
        [Required]
        [StringLength(500)]
        public string cWebhookUrl { get; set; } = string.Empty;

        [Column("cAppCodigo")]
        [Required]
        [StringLength(100)]
        public string cAppCodigo { get; set; } = string.Empty;

        [Column("cWebhookTipo")]
        [StringLength(50)]
        public string? cWebhookTipo { get; set; }

        [Column("cWebhookConfiguracion")]
        public string? cWebhookConfiguracion { get; set; }

        [Column("bWebhookActivo")]
        [Required]
        public bool bWebhookActivo { get; set; } = true;

        [Column("dFechaCreacion")]
        [Required]
        public DateTime dFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("dFechaModificacion")]
        public DateTime? dFechaModificacion { get; set; }

        [Column("cSecretToken")]
        [StringLength(255)]
        public string? cSecretToken { get; set; }

        [Column("nTimeoutSegundos")]
        public int nTimeoutSegundos { get; set; } = 30;

        [Column("nMaxReintentos")]
        public int nMaxReintentos { get; set; } = 3;

        [Column("bValidarCertificadoSSL")]
        public bool bValidarCertificadoSSL { get; set; } = true;
    }
}