namespace ChatModularMicroservice.Entities
{
    public class CategoriaActivoFilter
    {
        public int? nCategoriaActivoId { get; set; }
        public string? cCategoriaActivoNombre { get; set; }
        public string? cCategoriaActivoCodigo { get; set; }
        public string? cCategoriaActivoDescripcion { get; set; }
        public bool? bCategoriaActivoEsActiva { get; set; }
        public DateTime? dCategoriaActivoFechaEfectiva { get; set; }
        public string? cCategoriaActivoUsuarioRegistro { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}