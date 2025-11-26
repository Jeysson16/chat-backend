using ChatModularMicroservice.Repository;
using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Api.Webhooks;
using ChatModularMicroservice.Domain;
using System.Text;

namespace ChatModularMicroservice.Api.Controllers;

[ApiController]
[Route("api/v1/webhook")]
public class WebhookController : ControllerBase
{
    private readonly IWebhookService _webhookService;
    private readonly IAppRegistroRepository _appRepository;
    private readonly ILogger<WebhookController> _logger;

    public WebhookController(
        IWebhookService webhookService,
        IAppRegistroRepository appRepository,
        ILogger<WebhookController> logger)
    {
        _webhookService = webhookService;
        _appRepository = appRepository;
        _logger = logger;
    }

    [HttpPost("receive")]
    public async Task<IActionResult> ReceiveWebhook()
    {
        try
        {
            var appCode = Request.Headers["X-App-Code"].FirstOrDefault();
            var signature = Request.Headers["X-Webhook-Signature"].FirstOrDefault();
            var eventType = Request.Headers["X-Webhook-Event"].FirstOrDefault();

            if (string.IsNullOrEmpty(appCode) || string.IsNullOrEmpty(signature))
            {
                _logger.LogWarning("Missing required webhook headers");
                return BadRequest("Missing required headers");
            }

            // Read the request body
            using var reader = new StreamReader(Request.Body, Encoding.UTF8);
            var payload = await reader.ReadToEndAsync();

            if (string.IsNullOrEmpty(payload))
            {
                _logger.LogWarning("Empty webhook payload");
                return BadRequest("Empty payload");
            }

            // Validate the application
            var app = await _appRepository.GetByCodeAsync(appCode);
            if (app == null || !app.bAppActivo)
            {
                _logger.LogWarning("Invalid application code in webhook: {AppCode}", appCode);
                return Unauthorized("Invalid application");
            }

            // Validate the signature
            var isValidSignature = await _webhookService.ValidarSignaturaAsync(payload, signature, app.cSecretToken);
            if (!isValidSignature)
            {
                _logger.LogWarning("Invalid webhook signature for app: {AppCode}", appCode);
                return Unauthorized("Invalid signature");
            }

            _logger.LogInformation("Webhook received successfully for app: {AppCode}", appCode);
            return Ok(new { Message = "Webhook received", Event = eventType, AppCode = appCode });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook");
            return StatusCode(500, "Error processing webhook");
        }
    }

    [HttpPost("test/{appCode}")]
    public async Task<IActionResult> TestWebhook(string appCode)
    {
        try
        {
            var testPayload = new
            {
                Message = "This is a test webhook",
                TestData = new { Value = "test", Number = 123 }
            };

            // Get app configuration to get webhook URL
            var app = await _appRepository.GetByCodeAsync(appCode);
            if (app == null || string.IsNullOrEmpty(app.cAppUrl))
            {
                return BadRequest("App not found or webhook URL not configured");
            }

            await _webhookService.EnviarWebhookAsync(app.cAppUrl, testPayload, "test");

            return Ok(new { Message = "Test webhook sent", AppCode = appCode });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending test webhook for app: {AppCode}", appCode);
            return StatusCode(500, "Error sending test webhook");
        }
    }
}