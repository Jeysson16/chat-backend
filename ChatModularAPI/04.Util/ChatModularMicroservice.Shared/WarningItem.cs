namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Clase para representar una advertencia detallada
/// </summary>
public class WarningItem
{
    /// <summary>
    /// Código único de la advertencia
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    /// Mensaje descriptivo de la advertencia
    /// </summary>
    public string message { get; set; } = string.Empty;

    /// <summary>
    /// Campo específico donde ocurrió la advertencia (opcional)
    /// </summary>
    public string? field { get; set; }

    /// <summary>
    /// Información adicional de la advertencia
    /// </summary>
    public object? details { get; set; }

    /// <summary>
    /// Timestamp cuando ocurrió la advertencia
    /// </summary>
    public DateTime timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Nivel de severidad de la advertencia
    /// </summary>
    public WarningSeverity severity { get; set; } = WarningSeverity.Low;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public WarningItem() { }

    /// <summary>
    /// Constructor con parámetros básicos
    /// </summary>
    /// <param name="code">Código de la advertencia</param>
    /// <param name="message">Mensaje de la advertencia</param>
    public WarningItem(string code, string message)
    {
        this.code = code;
        this.message = message;
    }

    /// <summary>
    /// Constructor completo
    /// </summary>
    /// <param name="code">Código de la advertencia</param>
    /// <param name="message">Mensaje de la advertencia</param>
    /// <param name="severity">Nivel de severidad</param>
    /// <param name="field">Campo donde ocurrió la advertencia</param>
    /// <param name="details">Detalles adicionales</param>
    public WarningItem(string code, string message, WarningSeverity severity = WarningSeverity.Low, string? field = null, object? details = null)
    {
        this.code = code;
        this.message = message;
        this.severity = severity;
        this.field = field;
        this.details = details;
    }

    /// <summary>
    /// Convierte la advertencia a string
    /// </summary>
    /// <returns>Representación en string de la advertencia</returns>
    public override string ToString()
    {
        var result = $"[{code}] {message} (Severity: {severity})";
        if (!string.IsNullOrEmpty(field))
        {
            result += $" (Field: {field})";
        }
        return result;
    }
}

/// <summary>
/// Enumeración para los niveles de severidad de advertencias
/// </summary>
public enum WarningSeverity
{
    Low = 1,
    Medium = 2,
    High = 3
}