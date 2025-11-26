using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    /// <summary>
    /// Representa una configuración clave-valor específica para una empresa y aplicación
    /// </summary>
    [Table("ConfiguracionEmpresa")]
    public class ConfiguracionEmpresaKeyValue : BaseModel
    {
        [PrimaryKey("nConfiguracionEmpresaId")]
        public int NConfiguracionEmpresaId { get; set; }

        [Column("nConfiguracionEmpresaEmpresaId")]
        [Required]
        public int NEmpresasId { get; set; }

        [Column("nConfiguracionEmpresaAplicacionId")]
        [Required]
        public int NAplicacionesId { get; set; }

        [Column("cConfiguracionEmpresaClave")]
        [Required]
        [StringLength(255)]
        public string CConfiguracionEmpresaClave { get; set; } = string.Empty;

        [Column("cConfiguracionEmpresaValor")]
        [Required]
        [StringLength(1000)]
        public string CConfiguracionEmpresaValor { get; set; } = string.Empty;

        [Column("cConfiguracionEmpresaTipo")]
        [Required]
        [StringLength(50)]
        public string CConfiguracionEmpresaTipo { get; set; } = "string";

        [Column("cConfiguracionEmpresaDescripcion")]
        [StringLength(500)]
        public string? CConfiguracionEmpresaDescripcion { get; set; }

        [Column("bConfiguracionEmpresaActiva")]
        [Required]
        public bool BConfiguracionEmpresaEsActiva { get; set; } = true;

        [Column("dConfiguracionEmpresaFechaCreacion")]
        public DateTime? DConfiguracionEmpresaFechaCreacion { get; set; }

        [Column("dConfiguracionEmpresaFechaActualizacion")]
        public DateTime? DConfiguracionEmpresaFechaActualizacion { get; set; }

        // Propiedades heredadas de BaseModel
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}