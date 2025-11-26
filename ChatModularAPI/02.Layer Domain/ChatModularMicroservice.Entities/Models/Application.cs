using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("Aplicaciones")]
    public class Aplicacion : BaseModel
    {
        [PrimaryKey("nAplicacionesId")]
        public int nAplicacionesId { get; set; }

        [Column("cAplicacionesNombre")]
        [Required]
        [StringLength(100)]
        public string cAplicacionesNombre { get; set; } = string.Empty;

        [Column("cAplicacionesDescripcion")]
        [StringLength(500)]
        public string? cAplicacionesDescripcion { get; set; }

        [Column("cAplicacionesCodigo")]
        [Required]
        [StringLength(100)]
        public string cAplicacionesCodigo { get; set; } = string.Empty;

        [Column("bAplicacionesEsActiva")]
        [Required]
        public bool bAplicacionesEsActiva { get; set; } = true;

        [Column("dAplicacionesFechaCreacion")]
        [Required]
        public DateTime dAplicacionesFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("cAplicacionesUrlLogo")]
        [StringLength(500)]
        public string? cAplicacionesUrlLogo { get; set; }

        [Column("dAplicacionesFechaActualizacion")]
        [Required]
        public DateTime dAplicacionesFechaActualizacion { get; set; } = DateTime.UtcNow;

        [Column("cAplicacionesVersion")]
        [StringLength(100)]
        public string? cAplicacionesVersion { get; set; }

        // Alias properties for compatibility with existing code
        public int Id 
        { 
            get => nAplicacionesId; 
            set => nAplicacionesId = value; 
        }

        public string Name 
        { 
            get => cAplicacionesNombre; 
            set => cAplicacionesNombre = value; 
        }

        public string? Description 
        { 
            get => cAplicacionesDescripcion; 
            set => cAplicacionesDescripcion = value; 
        }

        public string Code 
        { 
            get => cAplicacionesCodigo; 
            set => cAplicacionesCodigo = value; 
        }

        public bool IsActive 
        { 
            get => bAplicacionesEsActiva; 
            set => bAplicacionesEsActiva = value; 
        }

        public DateTime CreatedAt 
        { 
            get => dAplicacionesFechaCreacion; 
            set => dAplicacionesFechaCreacion = value; 
        }

        public string? Version
        {
            get => cAplicacionesVersion;
            set => cAplicacionesVersion = value;
        }

        // Navigation properties
        public virtual ICollection<Empresa> Companies { get; set; } = new List<Empresa>();
        public virtual ConfiguracionAplicacion? Configuration { get; set; }
    }

    // Alias for backward compatibility
    public class Application : Aplicacion
    {
    }
}
