namespace ChatModularMicroservice.Entities
{
    public class WebhookContactoFilter
    {
        public string? cWebhookContactoId { get; set; }
        public string? cWebhookId { get; set; }
        public string? cContactoId { get; set; }
        public string? cTipoEvento { get; set; }
        public bool? bWebhookContactoActivo { get; set; }
        public DateTime? dFechaCreacion { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}