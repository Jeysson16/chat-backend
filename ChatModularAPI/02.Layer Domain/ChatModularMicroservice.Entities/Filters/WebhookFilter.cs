namespace ChatModularMicroservice.Entities
{
    public class WebhookFilter
    {
        public string? cWebhookId { get; set; }
        public string? cWebhookUrl { get; set; }
        public string? cAppCodigo { get; set; }
        public string? cWebhookTipo { get; set; }
        public bool? bWebhookActivo { get; set; }
        public DateTime? dFechaCreacion { get; set; }
        public string? TerminoBusqueda { get; set; }
    }
}