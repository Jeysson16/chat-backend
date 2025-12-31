using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities.Attributes;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using ChatModularMicroservice.Entities;
using System.IO;
using System.Linq;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador moderno para la gestión de chat que usa ResponseBase
/// </summary>
[ApiController]
[Route("api/chat")]
[Microsoft.AspNetCore.Authorization.Authorize]
public class ChatModernController : BaseController
{
    private readonly IChatService _chatService;
    private readonly IHubContext<ChatModularMicroservice.Api.Hubs.ChatHub> _hubContext;
    private readonly IFileStorageService _fileStorageService;
    private readonly IConfiguracionAplicacionUnificadaService _configService;

    public ChatModernController(IChatService chatService, IFileStorageService fileStorageService, IConfiguracionAplicacionUnificadaService configService, IHubContext<ChatModularMicroservice.Api.Hubs.ChatHub> hubContext, ILogger<ChatModernController> logger) : base(logger)
    {
        _chatService = chatService;
        _fileStorageService = fileStorageService;
        _configService = configService;
        _hubContext = hubContext;
    }

    /// <summary>
    /// Obtiene las conversaciones del usuario actual
    /// </summary>
    /// <param name="appCode">Código de la aplicación</param>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>ResponseBase con lista de conversaciones</returns>
    [HttpGet("conversations")]
    public async Task<IActionResult> GetMyConversations(
        [FromQuery] string appCode,
        [FromQuery] string perJurCodigo,
        [FromQuery] string userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(appCode))
            {
                return BadRequest(CreateErrorResponse("El código de aplicación es requerido", GetClientName(), GetUserName()));
            }

            var userIdClaim = string.IsNullOrWhiteSpace(userId) ? GetCurrentUserId() : userId;
            var conversations = await _chatService.GetUserConversationsAsync(appCode, perJurCodigo, userIdClaim);
            
            // Aplicar paginación manual si es necesario
            var totalCount = conversations.Count;
            var paginatedConversations = conversations
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var pagination = new Utils.Pagination(page, pageSize, totalCount);
            
            return Ok(CreateSuccessResponseWithPagination(paginatedConversations, pagination, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener conversaciones");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    [HttpPost("attachments")]
    [RequestSizeLimit(104_857_600)]
    public async Task<IActionResult> UploadAttachment([FromForm] IFormFile file, [FromForm] long conversationId)
    {
        try
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest(CreateErrorResponse("Archivo requerido", GetClientName(), GetUserName()));
            }

            var appCode = Request.Headers["x-app-code"].FirstOrDefault() ?? (HttpContext.Items.ContainsKey("AppCode") ? HttpContext.Items["AppCode"]?.ToString() : "");
            var fileName = Path.GetFileName(file.FileName);
            var extension = Path.GetExtension(fileName).Trim('.').ToLowerInvariant();

            // Obtener configuración unificada y derivar configuración de adjuntos
            var cfg = !string.IsNullOrWhiteSpace(appCode)
                ? await _configService.ObtenerConfiguracionPorCodigoAsync(appCode)
                : null;

            var tipos = (cfg?.cAdjuntosTiposArchivosPermitidos ?? string.Empty)
                .Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.ToLowerInvariant())
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            if (tipos.Count > 0 && !tipos.Contains(extension))
            {
                return BadRequest(CreateErrorResponse($"Tipo de archivo no permitido por configuración: .{extension}", GetClientName(), GetUserName()));
            }

            // Determinar límites de tamaño
            var mime = (file.ContentType ?? string.Empty).ToLowerInvariant();
            bool esImagen = mime.StartsWith("image/") || new[] { "jpg","jpeg","png","gif","bmp","webp","svg" }.Contains(extension);
            bool esVideo = mime.StartsWith("video/") || new[] { "mp4","avi","mov","wmv","flv","webm" }.Contains(extension);
            bool esAudio = mime.StartsWith("audio/") || new[] { "mp3","wav","ogg","m4a","flac" }.Contains(extension);

            long maxGeneral = (cfg?.nAdjuntosMaxTamanoArchivo > 0 ? cfg.nAdjuntosMaxTamanoArchivo : 104_857_600);
            long maxPorTipo = maxGeneral;

            if (file.Length > maxPorTipo)
            {
                return BadRequest(CreateErrorResponse($"El archivo excede el tamaño máximo permitido ({maxPorTipo / (1024 * 1024)}MB)", GetClientName(), GetUserName()));
            }

            await using var stream = file.OpenReadStream();
            var contentType = string.IsNullOrWhiteSpace(file.ContentType) ? "application/octet-stream" : file.ContentType;
            var attachment = await _fileStorageService.UploadAsync(stream, fileName, contentType, appCode ?? string.Empty, conversationId.ToString());

            return Ok(CreateSuccessResponse(attachment, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error subiendo adjunto");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene los mensajes de una conversación
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>ResponseBase con lista de mensajes</returns>
    [HttpGet("conversations/{conversationId}/messages")]
    public async Task<IActionResult> GetConversationMessages(
        long conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var messages = await _chatService.GetConversationMessagesAsync(conversationId, page, pageSize);
            
            // Calcular paginación (esto debería venir del servicio en una implementación completa)
            var pagination = new Utils.Pagination(page, pageSize, messages.Count);
            
            return Ok(CreateSuccessResponseWithPagination(messages, pagination, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener mensajes de la conversación: {ConversationId}", conversationId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Envía un mensaje a una conversación
    /// </summary>
    /// <param name="messageDto">Datos del mensaje</param>
    /// <returns>ResponseBase con el mensaje enviado</returns>
    [HttpPost("messages")]
    public async Task<IActionResult> SendMessage([FromBody] EnviarMensajeDto messageDto)
    {
        try
        {
            if (messageDto == null)
            {
                return BadRequest(CreateErrorResponse("Los datos del mensaje son requeridos", GetClientName(), GetUserName()));
            }

            if (string.IsNullOrWhiteSpace(messageDto.cMensajesChatTexto))
            {
                return BadRequest(CreateErrorResponse("El texto del mensaje es requerido", GetClientName(), GetUserName()));
            }

            var userId = GetCurrentUserId();
            var message = await _chatService.SendMessageAsync(userId, messageDto);
            
            return Ok(CreateSuccessResponse(message, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al enviar mensaje");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Marca un mensaje como leído
    /// </summary>
    /// <param name="messageId">ID del mensaje</param>
    /// <returns>ResponseBase con el resultado de la operación</returns>
    [HttpPatch("messages/{messageId}/read")]
    public async Task<IActionResult> MarkMessageAsRead(long messageId)
    {
        try
        {
            var userId = GetCurrentUserId();
            
            // Aquí iría la lógica para marcar como leído
            // await _chatService.MarkMessageAsReadAsync(messageId, userId);
            
            return Ok(CreateSuccessResponse(true, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al marcar mensaje como leído: {MessageId}", messageId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene los participantes de una conversación
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <returns>ResponseBase con lista de participantes</returns>
    [HttpGet("conversations/{conversationId}/participants")]
    public async Task<IActionResult> GetConversationParticipants(long conversationId)
    {
        try
        {
            var participants = await _chatService.GetConversationParticipantsAsync(conversationId);
            return Ok(CreateSuccessResponse(participants, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener participantes de la conversación: {ConversationId}", conversationId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Crea una nueva conversación
    /// </summary>
    /// <param name="crearConversacionDto">Datos de la conversación</param>
    /// <returns>ResponseBase con la conversación creada</returns>
    [HttpPost("conversations")]
    public async Task<IActionResult> CreateConversation([FromBody] CrearConversacionDto crearConversacionDto)
    {
        try
        {
            if (crearConversacionDto == null)
            {
                return BadRequest(CreateErrorResponse("Los datos de la conversación son requeridos", GetClientName(), GetUserName()));
            }

            if (string.IsNullOrWhiteSpace(crearConversacionDto.Nombre))
            {
                return BadRequest(CreateErrorResponse("El nombre de la conversación es requerido", GetClientName(), GetUserName()));
            }

            var userId = GetCurrentUserId();

            // Completar appCode desde headers si no viene en el body
            var appCode = Request.Headers["x-app-code"].FirstOrDefault() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(crearConversacionDto.cAppCodigo))
            {
                crearConversacionDto.cAppCodigo = appCode;
            }

            var conversation = await _chatService.CreateConversationAsync(userId, crearConversacionDto);

            // Emitir evento de creación vía SignalR al grupo de la aplicación
            if (!string.IsNullOrWhiteSpace(appCode))
            {
                await _hubContext.Clients.Group($"app_{appCode}").SendAsync("ConversationCreated", conversation);
            }

            // Emitir evento de creación a cada participante (incluye creador si está en la lista)
            var participants = await _chatService.GetConversationParticipantsAsync(conversation.nConversacionesChatId);
            foreach (var p in participants)
            {
                var pid = p.cUsuariosChatId;
                if (!string.IsNullOrWhiteSpace(pid))
                {
                    await _hubContext.Clients.Group($"user_{pid}").SendAsync("ConversationCreated", conversation);
                }
            }

            return Ok(CreateSuccessResponse(conversation, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear conversación");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Agrega un participante a una conversación
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <param name="addParticipantDto">Datos del participante</param>
    /// <returns>ResponseBase con el resultado de la operación</returns>
    [HttpPost("conversations/{conversationId}/participants")]
    public async Task<IActionResult> AddParticipant(
        long conversationId, 
        [FromBody] AddParticipantDto addParticipantDto)
    {
        try
        {
            if (addParticipantDto == null || addParticipantDto.UserId <= 0)
            {
                return BadRequest(CreateErrorResponse("Los datos del participante son requeridos", GetClientName(), GetUserName()));
            }

            var currentUserId = GetCurrentUserId();
            
            // Aquí iría la lógica para agregar participante
            // await _chatService.AddParticipantAsync(conversationId, addParticipantDto.UserId, currentUserId);
            
            return Ok(CreateSuccessResponse(true, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al agregar participante a la conversación: {ConversationId}", conversationId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Elimina un participante de una conversación
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <param name="userId">ID del usuario a eliminar</param>
    /// <returns>ResponseBase con el resultado de la operación</returns>
    [HttpDelete("conversations/{conversationId}/participants/{userId}")]
    public async Task<IActionResult> RemoveParticipant(long conversationId, string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return BadRequest(CreateErrorResponse("El ID del usuario es requerido", GetClientName(), GetUserName()));
            }

            var currentUserId = GetCurrentUserId();
            
            // Aquí iría la lógica para eliminar participante
            // await _chatService.RemoveParticipantAsync(conversationId, userId, currentUserId);
            
            return Ok(CreateSuccessResponse(true, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar participante de la conversación: {ConversationId}", conversationId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene el historial de mensajes con filtros
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <param name="fromDate">Fecha desde</param>
    /// <param name="toDate">Fecha hasta</param>
    /// <param name="messageType">Tipo de mensaje</param>
    /// <param name="page">Número de página</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <returns>ResponseBase con historial de mensajes</returns>
    [HttpGet("conversations/{conversationId}/history")]
    public async Task<IActionResult> GetMessageHistory(
        long conversationId,
        [FromQuery] DateTime? fromDate = null,
        [FromQuery] DateTime? toDate = null,
        [FromQuery] string? messageType = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            // Aquí iría la lógica para obtener historial con filtros
            var messages = await _chatService.GetConversationMessagesAsync(conversationId, page, pageSize);
            
            // Aplicar filtros si es necesario
            if (fromDate.HasValue)
            {
                messages = messages.Where(m => m.dMensajesChatFechaHora >= fromDate.Value).ToList();
            }
            
            if (toDate.HasValue)
            {
                messages = messages.Where(m => m.dMensajesChatFechaHora <= toDate.Value).ToList();
            }
            
            if (!string.IsNullOrWhiteSpace(messageType))
            {
                messages = messages.Where(m => m.cMensajesChatTipo == messageType).ToList();
            }

            var pagination = new Utils.Pagination(page, pageSize, messages.Count);
            
            return Ok(CreateSuccessResponseWithPagination(messages, pagination, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener historial de mensajes: {ConversationId}", conversationId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Endpoint de prueba para verificar la conexión a las nuevas tablas
    /// </summary>
    /// <returns>ResponseBase con información de conexión</returns>
    [HttpGet("test-connection")]
    [AllowAnonymous]
    public IActionResult TestConnection()
    {
        try
        {
            var result = new {
                message = "Conexión simulada a las nuevas tablas",
                tablesConnected = new[] { "ChatConversacion", "ChatMensaje", "ChatUsuario" },
                timestamp = DateTime.UtcNow
            };

            return Ok(CreateSuccessResponse(result, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al probar conexión a la base de datos");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }
}
