using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("Empresas")]
    public class Empresa : BaseModel
    {
        [PrimaryKey("nEmpresasId")]
        public int nEmpresasId { get; set; }

        [Column("cEmpresasNombre")]
        [Required]
        [StringLength(100)]
        public string cEmpresasNombre { get; set; } = string.Empty;

        [Column("cEmpresasCodigo")]
        [Required]
        [StringLength(100)]
        public string cEmpresasCodigo { get; set; } = string.Empty;

        [Column("nEmpresasAplicacionId")]
        [Required]
        public int nEmpresasAplicacionId { get; set; }

        [Column("dEmpresasFechaCreacion")]
        [Required]
        public DateTime dEmpresasFechaCreacion { get; set; } = DateTime.UtcNow;

        [Column("bEmpresasEsActiva")]
        [Required]
        public bool bEmpresasEsActiva { get; set; } = true;

        // Alias properties for compatibility with existing code
        public int Id 
        { 
            get => nEmpresasId; 
            set => nEmpresasId = value; 
        }

        public string Name 
        { 
            get => cEmpresasNombre; 
            set => cEmpresasNombre = value; 
        }

        public string Code 
        { 
            get => cEmpresasCodigo; 
            set => cEmpresasCodigo = value; 
        }

        public int ApplicationId 
        { 
            get => nEmpresasAplicacionId; 
            set => nEmpresasAplicacionId = value; 
        }

        public bool IsActive 
        { 
            get => bEmpresasEsActiva; 
            set => bEmpresasEsActiva = value; 
        }

        public DateTime CreatedAt 
        { 
            get => dEmpresasFechaCreacion; 
            set => dEmpresasFechaCreacion = value; 
        }
    }
}
