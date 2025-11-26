namespace ChatModularMicroservice.Entities
{
    public class TokenRegistroFilter
    {
        public int? nTokenRegistroId { get; set; }
        public string? cTokenRegistroJwtToken { get; set; }
        public string? cTokenRegistroUsuarioId { get; set; }
        public string? cTokenRegistroCodigoApp { get; set; }
        public string? cTokenRegistroPerJurCodigo { get; set; }
        public string? cTokenRegistroPerCodigo { get; set; }
        public bool? bTokenRegistroEsActivo { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}