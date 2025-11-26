namespace ChatModularMicroservice.Entities
{
    public class ChatUsuarioFilter
    {
        public int? nChatUsuarioId { get; set; }
        public string? cChatUsuarioUsuarioId { get; set; }
        public int? nChatUsuarioConversacionId { get; set; }
        public string? cChatUsuarioRol { get; set; }
        public bool? bChatUsuarioEsActivo { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}