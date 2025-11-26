namespace ChatModularMicroservice.Entities.Filter
{
    public class AdquisicionFilter : FilterBase
    {
        public int? nAdquisicionCodigo { get; set; }
        public string? cPerJurCodigo { get; set; }
        public int? nActivoCodigo { get; set; }
        public string? TerminoBusqueda { get; set; }
        public AdquisicionFilter() { }
    }
}