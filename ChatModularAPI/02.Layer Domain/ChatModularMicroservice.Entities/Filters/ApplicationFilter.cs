namespace ChatModularMicroservice.Entities
{
    public class ApplicationFilter
    {
        public int? nAplicacionesId { get; set; }
        public string? cAplicacionesNombre { get; set; }
        public string? cAplicacionesCodigo { get; set; }
        public string? cAplicacionesDescripcion { get; set; }
        public string? cAplicacionesVersion { get; set; }
        public bool? bAplicacionesEsActiva { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}