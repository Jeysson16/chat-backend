using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{
    [Table("ConfiguracionEmpresa")]
    public class ConfiguracionEmpresaSupabase : BaseModel
    {
        [PrimaryKey("nConfiguracionEmpresaId")]
        [Column("nConfiguracionEmpresaId")]
        public int? nConfiguracionEmpresaId { get; set; }

        [Column("nConfiguracionEmpresaEmpresaId")]
        public int nConfiguracionEmpresaEmpresaId { get; set; }

        [Column("cConfiguracionEmpresaClave")]
        public string cConfiguracionEmpresaClave { get; set; } = string.Empty;

        [Column("cConfiguracionEmpresaValor")]
        public string? cConfiguracionEmpresaValor { get; set; }

        [Column("dConfiguracionEmpresaFechaCreacion")]
        public System.DateTime? dConfiguracionEmpresaFechaCreacion { get; set; }

        [Column("dConfiguracionEmpresaFechaActualizacion")]
        public System.DateTime? dConfiguracionEmpresaFechaActualizacion { get; set; }
    }
}
