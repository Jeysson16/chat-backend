using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para webhooks de mensajes con validación de aplicación y empresa
    /// </summary>
    [ApiController]
    [Route("api/v1/webhook/mensajes")]
    public class WebhookMensajesController : BaseController
    {
        private readonly IContactoService _contactoService;

        public WebhookMensajesController(
            IContactoService contactoService,
            ILogger<WebhookMensajesController> logger) : base(logger)
        {
            _contactoService = contactoService;
        }

        /// <summary>
        /// Endpoint básico para webhooks de mensajes
        /// </summary>
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
                _logger.LogError(ex, "Error al obtener estado del webhook de mensajes");
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }


    }
}