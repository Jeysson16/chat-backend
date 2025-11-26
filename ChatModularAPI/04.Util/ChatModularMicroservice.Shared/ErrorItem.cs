namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Clase para representar un error detallado
/// </summary>
public class ErrorItem
{
    /// <summary>
    /// Código único del error
    /// </summary>
    public string code { get; set; } = string.Empty;

    /// <summary>
    /// Mensaje descriptivo del error
    /// </summary>
    public string message { get; set; } = string.Empty;

    /// <summary>
    /// Campo específico donde ocurrió el error (opcional)
    /// </summary>
    public string? field { get; set; }

    /// <summary>
    /// Información adicional del error
    /// </summary>
    public object? details { get; set; }

    /// <summary>
    /// Timestamp cuando ocurrió el error
    /// </summary>
    public DateTime timestamp { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public ErrorItem() { }

    /// <summary>
    /// Constructor con parámetros básicos
    /// </summary>
    /// <param name="code">Código del error</param>
    /// <param name="message">Mensaje del error</param>
    public ErrorItem(string code, string message)
    {
        this.code = code;
        this.message = message;
    }

    /// <summary>
    /// Constructor completo
    /// </summary>
    /// <param name="code">Código del error</param>
    /// <param name="message">Mensaje del error</param>
    /// <param name="field">Campo donde ocurrió el error</param>
    /// <param name="details">Detalles adicionales</param>
    public ErrorItem(string code, string message, string? field = null, object? details = null)
    {
        this.code = code;
        this.message = message;
        this.field = field;
        this.details = details;
    }

    /// <summary>
    /// Convierte el error a string
    /// </summary>
    /// <returns>Representación en string del error</returns>
    public override string ToString()
    {
        var result = $"[{code}] {message}";
        if (!string.IsNullOrEmpty(field))
        {
            result += $" (Field: {field})";
        }
        return result;
    }
}