using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Api.Controllers;

[ApiController]
[Route("api/chat-legacy")]
public class ChatController : ControllerBase
{
    private readonly IChatService _chatService;
    private readonly ILogger<ChatController> _logger;

    public ChatController(IChatService chatService, ILogger<ChatController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpGet("conversations")]
    public async Task<ActionResult<List<ChatConversacionDto>>> GetUserConversations()
    {
        try
        {
            var appCode = HttpContext.Items["AppCode"]?.ToString();
            var userId = HttpContext.Items["UserId"]?.ToString();
            var perJurCodigo = HttpContext.Items["PerJurCodigo"]?.ToString() ?? string.Empty;

            if (string.IsNullOrEmpty(appCode) || string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user or app context");
            }

            var conversations = await _chatService.GetUserConversationsAsync(appCode!, perJurCodigo, userId!);
            return Ok(conversations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user conversations");
            return StatusCode(500, "Error retrieving conversations");
        }
    }

    [HttpGet("conversations/{conversationId}/messages")]
    public async Task<ActionResult<List<ChatMensajeDto>>> GetConversationMessages(
        long conversationId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            if (pageSize > 100) pageSize = 100; // Limit page size

            var messages = await _chatService.GetConversationMessagesAsync(conversationId, page, pageSize);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conversation messages for conversation: {ConversationId}", conversationId);
            return StatusCode(500, "Error retrieving messages");
        }
    }

    [HttpPost("messages")]
    public async Task<ActionResult<ChatMensajeDto>> SendMessage([FromBody] EnviarMensajeDto messageDto)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var message = await _chatService.SendMessageAsync(userId, messageDto);
            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message");
            return StatusCode(500, "Error sending message");
        }
    }

    [HttpPost("conversations")]
    public async Task<ActionResult<ChatConversacionDto>> CreateConversation([FromBody] CrearConversacionDto conversationDto)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();
            var appCode = HttpContext.Items["AppCode"]?.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(appCode))
            {
                return BadRequest("Invalid user or app context");
            }

            // Ensure app code matches
            conversationDto.cAppCodigo = appCode;

            var conversation = await _chatService.CreateConversationAsync(userId, conversationDto);
            return Ok(conversation);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating conversation");
            return StatusCode(500, "Error creating conversation");
        }
    }

    [HttpPost("conversations/{conversationId}/join")]
    public async Task<ActionResult> JoinConversation(long conversationId)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var success = await _chatService.JoinConversationAsync(userId, conversationId);
            
            if (success)
            {
                return Ok(new { Message = "Successfully joined conversation" });
            }

            return BadRequest("Failed to join conversation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error joining conversation: {ConversationId}", conversationId);
            return StatusCode(500, "Error joining conversation");
        }
    }

    [HttpPost("conversations/{conversationId}/leave")]
    public async Task<ActionResult> LeaveConversation(long conversationId)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var success = await _chatService.LeaveConversationAsync(userId, conversationId);
            
            if (success)
            {
                return Ok(new { Message = "Successfully left conversation" });
            }

            return BadRequest("Failed to leave conversation");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error leaving conversation: {ConversationId}", conversationId);
            return StatusCode(500, "Error leaving conversation");
        }
    }

    [HttpGet("conversations/{conversationId}/participants")]
    public async Task<ActionResult<List<ChatUsuarioDto>>> GetConversationParticipants(long conversationId)
    {
        try
        {
            var participants = await _chatService.GetConversationParticipantsAsync(conversationId);
            return Ok(participants);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conversation participants: {ConversationId}", conversationId);
            return StatusCode(500, "Error retrieving participants");
        }
    }

    [HttpPost("conversations/{conversationId}/mark-read")]
    public async Task<ActionResult> MarkMessagesAsRead(long conversationId)
    {
        try
        {
            var userId = HttpContext.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("Invalid user context");
            }

            var success = await _chatService.MarkMessagesAsReadAsync(userId, conversationId);
            
            if (success)
            {
                return Ok(new { Message = "Messages marked as read" });
            }

            return BadRequest("Failed to mark messages as read");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking messages as read for conversation: {ConversationId}", conversationId);
            return StatusCode(500, "Error marking messages as read");
        }
    }
}
