using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Repository;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace ChatModularMicroservice.Api.Webhooks;

public class WebhookService : IWebhookService
{
    private readonly IAppRegistroRepository _appRepository;
    private readonly HttpClient _httpClient;
    private readonly ILogger<WebhookService> _logger;

    public WebhookService(
        IAppRegistroRepository appRepository,
        HttpClient httpClient,
        ILogger<WebhookService> logger)
    {
        _appRepository = appRepository;
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> EnviarWebhookAsync(string url, object payload, string tipoEvento)
    {
        try
        {
            var jsonPayload = JsonSerializer.Serialize(payload);
            
            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(jsonPayload, Encoding.UTF8, "application/json")
            };

            request.Headers.Add("X-Event-Type", tipoEvento);

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Webhook sent successfully to {Url}, event {EventType}", url, tipoEvento);
                return true;
            }
            else
            {
                _logger.LogWarning("Webhook failed to {Url}, event {EventType}. Status: {StatusCode}", 
                    url, tipoEvento, response.StatusCode);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending webhook to {Url}, event {EventType}", url, tipoEvento);
            return false;
        }
    }

    public async Task<bool> ValidarConfiguracionAsync(string codigoAplicacion)
    {
        try
        {
            var app = await _appRepository.GetByCodeAsync(codigoAplicacion);
            return app != null && !string.IsNullOrEmpty(app.cAppUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating configuration for app {AppCode}", codigoAplicacion);
            return false;
        }
    }

    public async Task<string> GenerarSignaturaAsync(string payload, string secreto)
    {
        return await Task.FromResult(GenerateSignature(payload, secreto));
    }

    public async Task<bool> ValidarSignaturaAsync(string payload, string signatura, string secreto)
    {
        try
        {
            var expectedSignature = await GenerarSignaturaAsync(payload, secreto);
            return signatura == expectedSignature;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating webhook signature");
            return false;
        }
    }

    private string GenerateSignature(string payload, string secret)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
        return Convert.ToHexString(hash).ToLower();
    }
}