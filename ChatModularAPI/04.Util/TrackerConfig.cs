namespace ChatModularAPI._04.Util;

/// <summary>
/// Configuración para el sistema de tracking/rastreo
/// </summary>
public class TrackerConfig
{
    /// <summary>
    /// Indica si el tracking está habilitado
    /// </summary>
    public bool isEnabled { get; set; } = true;

    /// <summary>
    /// Formato del ticket de tracking
    /// </summary>
    public string ticketFormat { get; set; } = "CHAT-{0}";

    /// <summary>
    /// Prefijo para los tickets
    /// </summary>
    public string ticketPrefix { get; set; } = "CHAT";

    /// <summary>
    /// Longitud del identificador único
    /// </summary>
    public int ticketLength { get; set; } = 8;

    /// <summary>
    /// Indica si se debe incluir timestamp en el ticket
    /// </summary>
    public bool includeTimestamp { get; set; } = false;

    /// <summary>
    /// Indica si se debe trackear en base de datos
    /// </summary>
    public bool trackInDatabase { get; set; } = true;

    /// <summary>
    /// Indica si se debe trackear en logs
    /// </summary>
    public bool trackInLogs { get; set; } = true;

    /// <summary>
    /// Tiempo de retención de los tickets en días
    /// </summary>
    public int retentionDays { get; set; } = 30;

    /// <summary>
    /// Headers HTTP que se deben trackear
    /// </summary>
    public List<string> trackedHeaders { get; set; } = new()
    {
        "User-Agent",
        "X-Forwarded-For",
        "X-Real-IP",
        "Authorization"
    };

    /// <summary>
    /// Propiedades adicionales que se deben trackear
    /// </summary>
    public Dictionary<string, object> additionalProperties { get; set; } = new();

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public TrackerConfig() { }

    /// <summary>
    /// Genera un nuevo ticket único
    /// </summary>
    /// <returns>Ticket único generado</returns>
    public string GenerateTicket()
    {
        var uniqueId = Guid.NewGuid().ToString("N")[..ticketLength];
        
        if (includeTimestamp)
        {
            var timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            return string.Format(ticketFormat, $"{uniqueId}-{timestamp}");
        }
        
        return string.Format(ticketFormat, uniqueId);
    }

    /// <summary>
    /// Valida la configuración del tracker
    /// </summary>
    /// <returns>True si la configuración es válida</returns>
    public bool IsValid()
    {
        return !string.IsNullOrEmpty(ticketFormat) &&
               !string.IsNullOrEmpty(ticketPrefix) &&
               ticketLength > 0 &&
               retentionDays > 0;
    }
}