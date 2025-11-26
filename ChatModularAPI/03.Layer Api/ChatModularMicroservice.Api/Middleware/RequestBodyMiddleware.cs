using System.Text;
using ChatModularMicroservice.Shared.Utils;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Api.Middleware;

/// <summary>
/// Middleware para capturar y loggear el cuerpo de las peticiones HTTP
/// </summary>
public class RequestBodyMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestBodyMiddleware> _logger;
    private readonly LoggerConfig _loggerConfig;

    public RequestBodyMiddleware(RequestDelegate next, ILogger<RequestBodyMiddleware> logger, LoggerConfig loggerConfig)
    {
        _next = next;
        _logger = logger;
        _loggerConfig = loggerConfig;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!_loggerConfig.logHttpRequests)
        {
            await _next(context);
            return;
        }

        // Solo procesar requests con cuerpo
        if (context.Request.ContentLength > 0 && 
            (context.Request.Method == "POST" || context.Request.Method == "PUT" || context.Request.Method == "PATCH"))
        {
            await LogRequestBodyAsync(context);
        }

        await _next(context);
    }

    private async Task LogRequestBodyAsync(HttpContext context)
    {
        try
        {
            // Habilitar buffering para poder leer el stream múltiples veces
            context.Request.EnableBuffering();

            // Leer el cuerpo de la petición
            var buffer = new byte[Convert.ToInt32(context.Request.ContentLength)];
            await context.Request.Body.ReadAsync(buffer, 0, buffer.Length);
            var requestBody = Encoding.UTF8.GetString(buffer);

            // Resetear la posición del stream para que el siguiente middleware pueda leerlo
            context.Request.Body.Position = 0;

            // Enmascarar campos sensibles
            var maskedBody = MaskSensitiveData(requestBody);

            // Loggear la información de la petición
            _logger.LogInformation("HTTP Request - Method: {Method}, Path: {Path}, Body: {Body}", 
                context.Request.Method, 
                context.Request.Path, 
                maskedBody);

            // Agregar información adicional a los headers para tracking
            var trackingTicket = context.Request.Headers["X-Tracking-Ticket"].FirstOrDefault() ?? Guid.NewGuid().ToString();
            context.Items["TrackingTicket"] = trackingTicket;
            context.Items["RequestBody"] = maskedBody;
            context.Items["RequestStartTime"] = DateTime.UtcNow;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al procesar el cuerpo de la petición");
        }
    }

    private string MaskSensitiveData(string requestBody)
    {
        if (string.IsNullOrEmpty(requestBody))
            return requestBody;

        var maskedBody = requestBody;

        // Enmascarar campos sensibles definidos en la configuración
        foreach (var field in _loggerConfig.maskedFields)
        {
            // Buscar patrones como "password": "valor" o "password":"valor"
            var patterns = new[]
            {
                $"\"{field}\"\\s*:\\s*\"([^\"]+)\"",
                $"'{field}'\\s*:\\s*'([^']+)'",
                $"{field}=([^&\\s]+)"
            };

            foreach (var pattern in patterns)
            {
                maskedBody = System.Text.RegularExpressions.Regex.Replace(
                    maskedBody, 
                    pattern, 
                    match => match.Value.Replace(match.Groups[1].Value, "***"),
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
        }

        return maskedBody;
    }
}