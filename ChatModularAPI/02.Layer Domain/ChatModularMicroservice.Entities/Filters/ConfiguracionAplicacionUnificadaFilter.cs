namespace ChatModularMicroservice.Entities
{
    public class ConfiguracionAplicacionUnificadaFilter
    {
        public int? nConfiguracionAplicacionUnificadaId { get; set; }
        public int? nConfiguracionAplicacionUnificadaAplicacionId { get; set; }
        public string? cConfiguracionAplicacionUnificadaClave { get; set; }
        public string? cConfiguracionAplicacionUnificadaValor { get; set; }
        public string? cConfiguracionAplicacionUnificadaTipo { get; set; }
        public bool? bConfiguracionAplicacionUnificadaEsActiva { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}