namespace ChatModularMicroservice.Entities
{
    public class EmpresaFilter
    {
        public int? nEmpresasId { get; set; }
        public string? cEmpresasCodigo { get; set; }
        public string? cEmpresasNombre { get; set; }
        public int? nEmpresasAplicacionId { get; set; }
        public string? TerminoBusqueda { get; set; }
        
        // Alias para compatibilidad
        public string? Codigo 
        { 
            get => cEmpresasCodigo; 
            set => cEmpresasCodigo = value; 
        }
    }
}