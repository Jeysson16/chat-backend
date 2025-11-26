namespace ChatModularMicroservice.Entities
{
    public class ChatFilter
    {
        public int? nConversacionesChatId { get; set; }
        public string? cConversacionesChatAppCodigo { get; set; }
        public string? cConversacionesChatUsuarioCreador { get; set; }
        public string? cConversacionesChatNombre { get; set; }
        public string? cConversacionesChatTipo { get; set; }
        public bool? bConversacionesChatEstaActiva { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}