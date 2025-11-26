namespace ChatModularMicroservice.Entities
{
    public class AppRegistroFilter
    {
        public int? nAppRegistroId { get; set; }
        public int? nAppRegistroAplicacionId { get; set; }
        public string? cAppRegistroToken { get; set; }
        public bool? bAppRegistroEsActivo { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}