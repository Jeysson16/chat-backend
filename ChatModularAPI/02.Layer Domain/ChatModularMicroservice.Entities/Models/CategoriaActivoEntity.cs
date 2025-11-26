using System.ComponentModel.DataAnnotations;
using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Entities.Models
{
    [Table("CategoriasActivos")]
    public class CategoriaActivoEntity : BaseModel
    {
        [PrimaryKey("nCategoriaActivoId")]
        public int nCategoriaActivoId { get; set; }

        [Column("cCategoriaActivoNombre")]
        [Required]
        [StringLength(100)]
        public string cCategoriaActivoNombre { get; set; } = string.Empty;

        [Column("cCategoriaActivoDescripcion")]
        [StringLength(500)]
        public string? cCategoriaActivoDescripcion { get; set; }

        [Column("cCategoriaActivoCodigo")]
        [Required]
        [StringLength(50)]
        public string cCategoriaActivoCodigo { get; set; } = string.Empty;

        [Column("bCategoriaActivoEsActiva")]
        [Required]
        public bool bCategoriaActivoEsActiva { get; set; } = true;

        [Column("dCategoriaActivoFechaEfectiva")]
        public DateTime? dCategoriaActivoFechaEfectiva { get; set; }

        [Column("cCategoriaActivoComentario")]
        [StringLength(1000)]
        public string? cCategoriaActivoComentario { get; set; }

        [Column("dCategoriaActivoFechaRegistro")]
        public DateTime dCategoriaActivoFechaRegistro { get; set; } = DateTime.UtcNow;

        [Column("cCategoriaActivoUsuarioRegistro")]
        [StringLength(100)]
        public string? cCategoriaActivoUsuarioRegistro { get; set; }

        // Propiedades alias para compatibilidad
        public int Id => nCategoriaActivoId;
        public string Nombre => cCategoriaActivoNombre;
        public string? Descripcion => cCategoriaActivoDescripcion;
        public string Codigo => cCategoriaActivoCodigo;
        public bool EsActiva => bCategoriaActivoEsActiva;
        public DateTime? FechaEfectiva => dCategoriaActivoFechaEfectiva;
        public string? Comentario => cCategoriaActivoComentario;
        public DateTime FechaRegistro => dCategoriaActivoFechaRegistro;
        public string? UsuarioRegistro => cCategoriaActivoUsuarioRegistro;
    }
}