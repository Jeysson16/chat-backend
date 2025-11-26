namespace ChatModularMicroservice.Entities
{
    public class ContactoFilter
    {
        public int? nContactosId { get; set; }
        public string? cUsuarioSolicitanteId { get; set; }
        public string? cUsuarioDestinoId { get; set; }
        public string? cEstado { get; set; }
        public int? nEmpresaId { get; set; }
        public int? nAplicacionId { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}