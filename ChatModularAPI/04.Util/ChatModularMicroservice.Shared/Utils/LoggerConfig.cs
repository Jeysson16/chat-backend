using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Shared.Utils;

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
    /// Indica si se debe loggear las peticiones HTTP
    /// </summary>
    public bool logHttpRequests { get; set; } = true;

    /// <summary>
    /// Indica si se debe loggear las respuestas HTTP
    /// </summary>
    public bool logHttpResponses { get; set; } = false;

    /// <summary>
    /// Tamaño máximo del cuerpo de petición a loggear
    /// </summary>
    public int maxRequestBodySize { get; set; } = 4096;

    /// <summary>
    /// Indica si se debe loggear información de performance
    /// </summary>
    public bool logPerformance { get; set; } = true;

    /// <summary>
    /// Umbral de tiempo en ms para considerar una operación lenta
    /// </summary>
    public int slowOperationThresholdMs { get; set; } = 1000;

    /// <summary>
    /// Indica si se debe loggear información de base de datos
    /// </summary>
    public bool logDatabase { get; set; } = false;

    /// <summary>
    /// Indica si se debe loggear información de autenticación
    /// </summary>
    public bool logAuthentication { get; set; } = true;

    /// <summary>
    /// Lista de endpoints que no se deben loggear
    /// </summary>
    public List<string> excludedEndpoints { get; set; } = new List<string>
    {
        "/metrics",
        "/favicon.ico"
    };

    /// <summary>
    /// Lista de headers que no se deben loggear por seguridad
    /// </summary>
    public List<string> excludedHeaders { get; set; } = new List<string>
    {
        "Authorization",
        "Cookie",
        "X-API-Key",
        "X-Auth-Token"
    };

    /// <summary>
    /// Campos que se deben enmascarar en los logs
    /// </summary>
    public List<string> maskedFields { get; set; } = new List<string>
    {
        "password",
        "token",
        "secret",
        "key",
        "authorization"
    };

    /// <summary>
    /// Configuración específica para diferentes entornos
    /// </summary>
    public Dictionary<string, object> environmentSettings { get; set; } = new Dictionary<string, object>();

    /// <summary>
    /// Valida la configuración del logger
    /// </summary>
    /// <returns>True si la configuración es válida</returns>
    public bool IsValid()
    {
        if (maxFileSizeMB <= 0)
            return false;

        if (maxRetainedFiles <= 0)
            return false;

        if (maxRequestBodySize < 0)
            return false;

        if (slowOperationThresholdMs < 0)
            return false;

        if (string.IsNullOrWhiteSpace(logFilePath))
            return false;

        if (string.IsNullOrWhiteSpace(timestampFormat))
            return false;

        return true;
    }

    /// <summary>
    /// Obtiene la configuración por defecto para desarrollo
    /// </summary>
    /// <returns>Configuración de desarrollo</returns>
    public static LoggerConfig GetDevelopmentConfig()
    {
        return new LoggerConfig
        {
            minimumLevel = LogLevel.Debug,
            logToConsole = true,
            logToFile = true,
            logHttpRequests = true,
            logHttpResponses = true,
            logPerformance = true,
            logDatabase = true,
            logAuthentication = true
        };
    }

    /// <summary>
    /// Obtiene la configuración por defecto para producción
    /// </summary>
    /// <returns>Configuración de producción</returns>
    public static LoggerConfig GetProductionConfig()
    {
        return new LoggerConfig
        {
            minimumLevel = LogLevel.Warning,
            logToConsole = false,
            logToFile = true,
            logHttpRequests = false,
            logHttpResponses = false,
            logPerformance = true,
            logDatabase = false,
            logAuthentication = true,
            maxFileSizeMB = 50,
            maxRetainedFiles = 30
        };
    }
}