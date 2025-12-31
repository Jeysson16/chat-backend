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
    }
}
