using Supabase.Postgrest.Attributes;
using Supabase.Postgrest.Models;

namespace ChatModularMicroservice.Domain
{
    [Table("ConfiguracionEmpresa")]
    public class ConfiguracionEmpresaInsertSupabase : BaseModel
    {
        [Column("nConfiguracionEmpresaEmpresaId")] public int nConfiguracionEmpresaEmpresaId { get; set; }
        [Column("cConfiguracionEmpresaClave")] public string cConfiguracionEmpresaClave { get; set; } = string.Empty;
        [Column("cConfiguracionEmpresaValor")] public string? cConfiguracionEmpresaValor { get; set; }
        [Column("dConfiguracionEmpresaFechaCreacion")] public System.DateTime? dConfiguracionEmpresaFechaCreacion { get; set; }
        [Column("dConfiguracionEmpresaFechaActualizacion")] public System.DateTime? dConfiguracionEmpresaFechaActualizacion { get; set; }
    }
}

