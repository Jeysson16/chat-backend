using Microsoft.AspNetCore.SignalR;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using System.Security.Claims;

namespace ChatModularMicroservice.Api.Hubs;

public class ChatHub : Hub
{
    private readonly IChatService _chatService;
    private readonly ITokenService _tokenService;
    private readonly ILogger<ChatHub> _logger;

    public ChatHub(IChatService chatService, ITokenService tokenService, ILogger<ChatHub> logger)
    {
        _chatService = chatService;
        _tokenService = tokenService;
        _logger = logger;
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            // Accept authentication via both headers (for HTTP) and query parameters (for WebSocket)
            var httpContext = Context.GetHttpContext();
            
            // Try headers first (for HTTP requests)
            var appCode = httpContext?.Request.Headers["x-app-code"].FirstOrDefault() ?? 
                         httpContext?.Request.Headers["X-App-Code"].FirstOrDefault() ??
                         httpContext?.Request.Headers["X-APP-CODE"].FirstOrDefault();
            
            // Try query parameters for WebSocket connections (browsers can't send custom headers via WebSocket)
            if (string.IsNullOrEmpty(appCode))
            {
                appCode = httpContext?.Request.Query["appCode"].FirstOrDefault() ??
                         httpContext?.Request.Query["app_code"].FirstOrDefault();
            }

            // Try Authorization header first, then query parameter for WebSocket
            var authHeader = httpContext?.Request.Headers["Authorization"].FirstOrDefault();
            if (string.IsNullOrEmpty(authHeader))
            {
                var tokenFromQuery = httpContext?.Request.Query["access_token"].FirstOrDefault() ??
                                   httpContext?.Request.Query["token"].FirstOrDefault();
                if (!string.IsNullOrEmpty(tokenFromQuery))
                {
                    authHeader = $"Bearer {tokenFromQuery}";
                }
            }

            _logger.LogInformation("SignalR connection attempt - AppCode: {AppCode}, HasAuthHeader: {HasAuth}", 
                appCode ?? "null", !string.IsNullOrEmpty(authHeader));

            if (string.IsNullOrEmpty(appCode) || string.IsNullOrEmpty(authHeader))
            {
                _logger.LogWarning("Missing required headers for SignalR connection. AppCode: {AppCode}, AuthHeader: {AuthHeader}", 
                    appCode ?? "null", authHeader ?? "null");
                Context.Abort();
                return;
            }

            var token = authHeader.Replace("Bearer ", "");
            var principal = await _tokenService.ValidateJwtTokenAsync(token);

            if (principal == null)
            {
                _logger.LogWarning("Invalid token for SignalR connection");
                Context.Abort();
                return;
            }

            // Log all claims for debugging
            var claims = principal.Claims.Select(c => $"{c.Type}:{c.Value}").ToList();
            _logger.LogInformation("Token validated with claims: {Claims}", string.Join(", ", claims));

            var tokenAppCode = principal.FindFirst("app_code")?.Value;
            if (tokenAppCode != appCode)
            {
                _logger.LogWarning("Token app code mismatch. Token: {TokenAppCode}, Header: {HeaderAppCode}", 
                    tokenAppCode ?? "null", appCode);
                Context.Abort();
                return;
            }

            var userId = principal.FindFirst("user_id")?.Value ?? 
                        principal.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                        principal.FindFirst("nameid")?.Value;
            var userName = principal.FindFirst(ClaimTypes.Name)?.Value ??
                           principal.FindFirst("unique_name")?.Value;

            _logger.LogInformation("Extracted user info - UserId: {UserId}, UserName: {UserName}", 
                userId ?? "null", userName ?? "null");

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("Could not extract user ID from token claims");
                Context.Abort();
                return;
            }

            // Extraer información adicional de headers (case-insensitive) o query parameters
            var headers = httpContext?.Request.Headers;
            var query = httpContext?.Request.Query;
            
            var accessToken = headers?["X-Access-Token"].FirstOrDefault() ?? headers?["x-access-token"].FirstOrDefault() ??
                             query?["accessToken"].FirstOrDefault() ?? query?["access_token"].FirstOrDefault();
            var secretToken = headers?["X-Secret-Token"].FirstOrDefault() ?? headers?["x-secret-token"].FirstOrDefault() ??
                             query?["secretToken"].FirstOrDefault() ?? query?["secret_token"].FirstOrDefault();
            var userCode = headers?["X-User-Code"].FirstOrDefault() ?? headers?["x-user-code"].FirstOrDefault() ??
                          query?["userCode"].FirstOrDefault() ?? query?["user_code"].FirstOrDefault() ??
                          query?["nPerId"].FirstOrDefault() ?? query?["cPerCodigo"].FirstOrDefault();
            var personCode = headers?["X-Person-Code"].FirstOrDefault() ?? headers?["x-person-code"].FirstOrDefault() ??
                            query?["personCode"].FirstOrDefault() ?? query?["person_code"].FirstOrDefault();

            // Add user to app-specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"app_{appCode}");
            
            // Add user to user-specific group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user_{userId}");

            // Store user info in connection
            Context.Items["AppCode"] = appCode;
            Context.Items["UserId"] = userId;
            Context.Items["UserName"] = userName;
            Context.Items["AccessToken"] = accessToken;
            Context.Items["SecretToken"] = secretToken;
            Context.Items["UserCode"] = userCode;
            Context.Items["PersonCode"] = personCode;

            _logger.LogInformation("User {UserName} connected to app {AppCode} with connection {ConnectionId}", 
                userName, appCode, Context.ConnectionId);

            // Notify app group about user connection
            await Clients.Group($"app_{appCode}").SendAsync("UserConnected", new
            {
                UserId = userId,
                UserName = userName,
                ConnectionId = Context.ConnectionId,
                ConnectedAt = DateTime.UtcNow
            });

            await base.OnConnectedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OnConnectedAsync");
            Context.Abort();
        }
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            var appCode = Context.Items["AppCode"]?.ToString();
            var userId = Context.Items["UserId"]?.ToString();
            var userName = Context.Items["UserName"]?.ToString();

            if (!string.IsNullOrEmpty(appCode) && !string.IsNullOrEmpty(userId))
            {
                // Remove from groups
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"app_{appCode}");
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user_{userId}");

                _logger.LogInformation("User {UserName} disconnected from app {AppCode} with connection {ConnectionId}", 
                    userName, appCode, Context.ConnectionId);

                // Notify app group about user disconnection
                await Clients.Group($"app_{appCode}").SendAsync("UserDisconnected", new
                {
                    UserId = userId,
                    UserName = userName,
                    ConnectionId = Context.ConnectionId,
                    DisconnectedAt = DateTime.UtcNow
                });
            }

            await base.OnDisconnectedAsync(exception);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in OnDisconnectedAsync");
        }
    }

    public async Task JoinConversation(long conversationId)
    {
        try
        {
            var userId = Context.Items["UserId"]?.ToString();
            var appCode = Context.Items["AppCode"]?.ToString();

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(appCode))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user or app context");
                return;
            }

            // Join conversation in database
            var success = await _chatService.JoinConversationAsync(userId, conversationId);
            
            if (success)
            {
                // Add to conversation group
                await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
                
                // Notify conversation participants
                await Clients.Group($"conversation_{conversationId}").SendAsync("UserJoinedConversation", new
                {
                    ConversationId = conversationId,
                    UserId = userId,
                    UserName = Context.Items["UserName"]?.ToString(),
                    JoinedAt = DateTime.UtcNow
                });

                await Clients.Caller.SendAsync("JoinedConversation", conversationId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Failed to join conversation");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error joining conversation {ConversationId}", conversationId);
            await Clients.Caller.SendAsync("Error", "Error joining conversation");
        }
    }

    public async Task LeaveConversation(long conversationId)
    {
        try
        {
            var userId = Context.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user context");
                return;
            }

            // Leave conversation in database
            var success = await _chatService.LeaveConversationAsync(userId, conversationId);
            
            if (success)
            {
                // Remove from conversation group
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation_{conversationId}");
                
                // Notify conversation participants
                await Clients.Group($"conversation_{conversationId}").SendAsync("UserLeftConversation", new
                {
                    ConversationId = conversationId,
                    UserId = userId,
                    UserName = Context.Items["UserName"]?.ToString(),
                    LeftAt = DateTime.UtcNow
                });

                await Clients.Caller.SendAsync("LeftConversation", conversationId);
            }
            else
            {
                await Clients.Caller.SendAsync("Error", "Failed to leave conversation");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error leaving conversation {ConversationId}", conversationId);
            await Clients.Caller.SendAsync("Error", "Error leaving conversation");
        }
    }

    public async Task SendMessage(EnviarMensajeDto messageDto)
    {
        try
        {
            var userId = Context.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user context");
                return;
            }

            // Send message through service
            var message = await _chatService.SendMessageAsync(userId, messageDto);

            // Broadcast message to conversation participants
            await Clients.Group($"conversation_{messageDto.nConversacionesChatId}").SendAsync("ReceiveMessage", message);

            _logger.LogInformation("Message sent by user {UserId} to conversation {ConversationId}", 
                userId, messageDto.nConversacionesChatId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to conversation {ConversationId}", messageDto.nConversacionesChatId);
            await Clients.Caller.SendAsync("Error", "Error sending message");
        }
    }

    public async Task MarkMessagesAsRead(long conversationId)
    {
        try
        {
            var userId = Context.Items["UserId"]?.ToString();

            if (string.IsNullOrEmpty(userId))
            {
                await Clients.Caller.SendAsync("Error", "Invalid user context");
                return;
            }

            var success = await _chatService.MarkMessagesAsReadAsync(userId, conversationId);
            
            if (success)
            {
                // Notify conversation participants about read status
                await Clients.Group($"conversation_{conversationId}").SendAsync("MessagesMarkedAsRead", new
                {
                    ConversationId = conversationId,
                    UserId = userId,
                    ReadAt = DateTime.UtcNow
                });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking messages as read for conversation {ConversationId}", conversationId);
            await Clients.Caller.SendAsync("Error", "Error marking messages as read");
        }
    }

    public async Task GetOnlineUsers()
    {
        try
        {
            var appCode = Context.Items["AppCode"]?.ToString();

            if (string.IsNullOrEmpty(appCode))
            {
                await Clients.Caller.SendAsync("Error", "Invalid app context");
                return;
            }

            // This is a simplified implementation
            // In a production environment, you might want to track online users in a cache or database
            await Clients.Caller.SendAsync("OnlineUsers", new List<object>());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting online users");
            await Clients.Caller.SendAsync("Error", "Error getting online users");
        }
    }
}