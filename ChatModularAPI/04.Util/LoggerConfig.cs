using Microsoft.Extensions.Logging;

namespace ChatModularAPI._04.Util;

/// <summary>
/// Configuración para el sistema de logging
/// </summary>
public class LoggerConfig
{
    /// <summary>
    /// Nivel mínimo de logging
    /// </summary>
    public LogLevel minimumLevel { get; set; } = LogLevel.Information;

    /// <summary>
    /// Indica si se debe loggear en consola
    /// </summary>
    public bool logToConsole { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear en archivo
    /// </summary>
    public bool logToFile { get; set; } = true;

    /// <summary>
    /// Ruta del archivo de logs
    /// </summary>
    public string logFilePath { get; set; } = "logs/chatmodular-{Date}.log";

    /// <summary>
    /// Tamaño máximo del archivo de log en MB
    /// </summary>
    public int maxFileSizeMB { get; set; } = 100;

    /// <summary>
    /// Número máximo de archivos de log a mantener
    /// </summary>
    public int maxRetainedFiles { get; set; } = 10;

    /// <summary>
    /// Formato del timestamp en los logs
    /// </summary>
    public string timestampFormat { get; set; } = "yyyy-MM-dd HH:mm:ss.fff";

    /// <summary>
    /// Indica si se debe incluir información de contexto
    /// </summary>
    public bool includeContext { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear requests HTTP
    /// </summary>
    public bool logHttpRequests { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear responses HTTP
    /// </summary>
    public bool logHttpResponses { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear errores de base de datos
    /// </summary>
    public bool logDatabaseErrors { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear queries de base de datos (solo en desarrollo)
    /// </summary>
    public bool logDatabaseQueries { get; set; } = false;

    /// <summary>
    /// Headers HTTP que NO se deben loggear por seguridad
    /// </summary>
    public List<string> excludedHeaders { get; set; } = new()
    {
        "Authorization",
        "Cookie",
        "X-API-Key",
        "X-Auth-Token"
    };

    /// <summary>
    /// Campos que se deben enmascarar en los logs
    /// </summary>
    public List<string> maskedFields { get; set; } = new()
    {
        "password",
        "token",
        "secret",
        "key",
        "authorization"
    };

    /// <summary>
    /// Configuración específica por categoría de logger
    /// </summary>
    public Dictionary<string, LogLevel> categoryLevels { get; set; } = new()
    {
        { "Microsoft", LogLevel.Warning },
        { "Microsoft.Hosting.Lifetime", LogLevel.Information },
        { "Microsoft.EntityFrameworkCore", LogLevel.Warning },
        { "System", LogLevel.Warning }
    };

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public LoggerConfig() { }

    /// <summary>
    /// Obtiene el nivel de log para una categoría específica
    /// </summary>
    /// <param name="category">Categoría del logger</param>
    /// <returns>Nivel de log configurado</returns>
    public LogLevel GetLevelForCategory(string category)
    {
        foreach (var kvp in categoryLevels)
        {
            if (category.StartsWith(kvp.Key))
            {
                return kvp.Value;
            }
        }
        return minimumLevel;
    }

    /// <summary>
    /// Verifica si un header debe ser excluido del logging
    /// </summary>
    /// <param name="headerName">Nombre del header</param>
    /// <returns>True si debe ser excluido</returns>
    public bool ShouldExcludeHeader(string headerName)
    {
        return excludedHeaders.Any(h => 
            string.Equals(h, headerName, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Verifica si un campo debe ser enmascarado
    /// </summary>
    /// <param name="fieldName">Nombre del campo</param>
    /// <returns>True si debe ser enmascarado</returns>
    public bool ShouldMaskField(string fieldName)
    {
        return maskedFields.Any(f => 
            fieldName.Contains(f, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Enmascara un valor sensible
    /// </summary>
    /// <param name="value">Valor a enmascarar</param>
    /// <returns>Valor enmascarado</returns>
    public string MaskValue(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length <= 4)
        {
            return "***";
        }
        
        return $"{value[..2]}***{value[^2..]}";
    }
}