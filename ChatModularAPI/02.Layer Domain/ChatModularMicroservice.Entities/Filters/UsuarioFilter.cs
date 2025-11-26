namespace ChatModularMicroservice.Entities
{
    public class UsuarioFilter
    {
        public string? cUsuariosId { get; set; }
        public string? cEmail { get; set; }
        public string? cNombre { get; set; }
        public string? cApellido { get; set; }
        public string? cTelefono { get; set; }
        public int? nEmpresaId { get; set; }
        public int? nAplicacionId { get; set; }
        public bool? bActivo { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}