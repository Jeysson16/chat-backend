using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador para webhooks específicos de eventos de contactos
/// </summary>
[ApiController]
[Route("api/v1/webhook/contactos")]
[Authorize]
public class WebhookContactoController : BaseController
{
    private readonly IContactoService _contactoService;

    public WebhookContactoController(
        IContactoService contactoService,
        ILogger<WebhookContactoController> logger) : base(logger)
    {
        _contactoService = contactoService;
    }

    /// <summary>
    /// Endpoint básico para webhooks de contactos
    /// </summary>
    /// <returns>Estado del webhook</returns>
    [HttpGet("status")]
    public async Task<IActionResult> GetWebhookStatus()
    {
        try
        {
            var statusData = new { Status = "Active", Timestamp = DateTime.UtcNow };
            return Ok(CreateSuccessResponse(statusData, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estado del webhook");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }




}