namespace ChatModularMicroservice.Entities.DTOs
{
    public class DashboardStatsDto
    {
        public int totalAplicaciones { get; set; }
        public int aplicacionesActivas { get; set; }
        public int totalEmpresas { get; set; }
        public int empresasActivas { get; set; }
        public int totalUsuarios { get; set; }
        public int usuariosActivos { get; set; }
        public int usuariosOnline { get; set; }
        public int totalConfiguraciones { get; set; }
        public int configuracionesAplicacion { get; set; }
        public int configuracionesEmpresa { get; set; }
    }
}
