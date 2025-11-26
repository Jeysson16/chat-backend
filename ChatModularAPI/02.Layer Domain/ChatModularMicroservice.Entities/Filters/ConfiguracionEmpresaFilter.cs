namespace ChatModularMicroservice.Entities
{
    public class ConfiguracionEmpresaFilter
    {
        public int? nConfiguracionEmpresaId { get; set; }
        public int? nConfiguracionEmpresaEmpresaId { get; set; }
        public int? nConfiguracionEmpresaTipoConfiguracionId { get; set; }
        public string? cConfiguracionEmpresaValor { get; set; }
        public bool? bConfiguracionEmpresaEsActiva { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}