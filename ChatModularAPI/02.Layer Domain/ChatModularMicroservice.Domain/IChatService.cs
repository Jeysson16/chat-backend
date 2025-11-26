using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IChatService
    {
        Task<List<ChatConversacionDto>> GetUserConversationsAsync(string appCode, string perJurCodigo, string userId);
        Task<List<ChatMensajeDto>> GetConversationMessagesAsync(long conversationId, int page = 1, int pageSize = 50);
        Task<ChatMensajeDto> SendMessageAsync(string userId, EnviarMensajeDto messageDto);
        Task<ChatConversacionDto> CreateConversationAsync(string userId, CrearConversacionDto conversationDto);
        Task<bool> JoinConversationAsync(string userId, long conversationId);
        Task<bool> LeaveConversationAsync(string userId, long conversationId);
        Task<List<ChatUsuarioDto>> GetConversationParticipantsAsync(long conversationId);
        Task<bool> MarkMessagesAsReadAsync(string userId, long conversationId);
    }
}
