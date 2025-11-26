using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Repositorio para operaciones de chat y conversaciones
/// </summary>
public interface IChatRepository : IDeleteIntRepository, IInsertIntRepository<ChatConversacion>, IUpdateRepository<ChatConversacion>
{
    /// <summary>
    /// Obtiene una conversación específica basada en el filtro y tipo de filtro
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <returns>Conversación encontrada</returns>
    Task<ChatConversacion> GetItem(ChatFilter filter, ChatFilterItemType filterType);

    /// <summary>
    /// Obtiene una lista de conversaciones basada en el filtro, tipo de filtro y paginación
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <param name="pagination">Configuración de paginación</param>
    /// <returns>Lista de conversaciones</returns>
    Task<IEnumerable<ChatConversacion>> GetLstItem(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination);

    // Métodos específicos del dominio de chat
    Task<List<ChatConversacion>> GetUserConversationsAsync(string appCode, string userId);
    Task<List<ChatMensaje>> GetConversationMessagesAsync(long conversationId, int page = 1, int pageSize = 50);
    Task<IEnumerable<ChatMensaje>> GetConversationMessages(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination);
    Task<ChatMensaje> CreateMessageAsync(long conversationId, Guid userId, string messageText, string messageType = "text");
    Task<ChatMensaje> SendMessageAsync(ChatMensaje message);
    Task<ChatConversacion> CreateConversationAsync(string appCode, string? conversationName, string conversationType, List<Guid> participantIds);
    Task<bool> AddUserToConversationAsync(long conversationId, Guid userId);
    Task<bool> RemoveUserFromConversationAsync(long conversationId, Guid userId);
    Task<List<ChatUsuario>> GetConversationParticipantsAsync(long conversationId);
    Task<bool> MarkMessagesAsReadAsync(long conversationId, Guid userId);
    Task<ChatConversacion?> GetConversationByIdAsync(long conversationId);
    Task<bool> IsUserInConversationAsync(long conversationId, Guid userId);
    
    /// <summary>
    /// Actualiza una conversación existente
    /// </summary>
    /// <param name="conversacion">Datos de la conversación a actualizar</param>
    /// <returns>True si se actualizó correctamente, false en caso contrario</returns>
    Task<bool> UpdateConversationAsync(ChatConversacion conversacion);
    
    /// <summary>
    /// Elimina una conversación
    /// </summary>
    /// <param name="conversationId">ID de la conversación a eliminar</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario</returns>
    Task<bool> DeleteConversationAsync(long conversationId);
    
    /// <summary>
    /// Verifica si una conversación existe
    /// </summary>
    /// <param name="conversationId">ID de la conversación</param>
    /// <returns>True si la conversación existe, false en caso contrario</returns>
    Task<bool> ConversationExistsAsync(long conversationId);
}