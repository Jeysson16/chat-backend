using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;

namespace ChatModularMicroservice.Infrastructure;

public class ChatRepository : BaseRepository, ChatModularMicroservice.Repository.IChatRepository
{
    #region Constructor
    public ChatRepository(IConnectionFactory cn) : base(cn)
    {
    }

    // Implementaciones explícitas de interfaz para resolver problemas de firma
    Task<List<ChatConversacion>> IChatRepository.GetUserConversationsAsync(string appCode, string userId)
    {
        return GetUserConversationsAsync(appCode, userId);
    }

    Task<List<ChatUsuario>> IChatRepository.GetConversationParticipantsAsync(long conversationId)
    {
        return GetConversationParticipantsAsync(conversationId);
    }

    Task<bool> IChatRepository.MarkMessagesAsReadAsync(long conversationId, Guid userId)
    {
        return MarkMessagesAsReadAsync(conversationId, userId);
    }

    #endregion

    #region Public Methods

    public async Task<int> Insert(ChatConversacion item)
    {
        int affectedRows = 0;
        var query = "USP_Chat_Insert";
        var parameters = new DynamicParameters();

        parameters.Add("@nConversacionesChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        parameters.Add("@cConversacionesChatAppCodigo", item.cConversacionesChatAppCodigo, DbType.String);
        parameters.Add("@cConversacionesChatNombre", item.cConversacionesChatNombre, DbType.String);
        parameters.Add("@cConversacionesChatTipo", item.cConversacionesChatTipo, DbType.String);
        parameters.Add("@bConversacionesChatEstaActiva", item.bConversacionesChatEstaActiva, DbType.Boolean);
        parameters.Add("@cConversacionesChatUsuarioCreador", item.cConversacionesChatUsuarioCreadorId, DbType.String);

        affectedRows = await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, parameters, commandType: CommandType.StoredProcedure);
        int generatedId = parameters.Get<int>("@nConversacionesChatId");

        if (affectedRows <= 0 || generatedId <= 0)
        {
            throw new InvalidOperationException("No se pudo insertar la conversación o no se obtuvo un ID válido");
        }

        return generatedId;
    }

    public async Task<bool> Update(ChatConversacion item) =>
        await this.UpdateOrDelete("USP_Chat_Update", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nConversacionesChatId", item.nConversacionesChatId},
            {"@cConversacionesChatAppCodigo", item.cConversacionesChatAppCodigo},
            {"@cConversacionesChatNombre", item.cConversacionesChatNombre},
            {"@cConversacionesChatTipo", item.cConversacionesChatTipo},
            {"@bConversacionesChatEstaActiva", item.bConversacionesChatEstaActiva},
            {"@cConversacionesChatUsuarioCreador", item.cConversacionesChatUsuarioCreadorId}
        }));

    public async Task<bool> DeleteEntero(Int32 nConversacionesChatId) =>
        await this.UpdateOrDelete("USP_Chat_Delete", new DynamicParameters(new Dictionary<string, object>
        {
            {"@nConversacionesChatId", nConversacionesChatId}
        }));

    public async Task<ChatConversacion> GetItem(ChatFilter filter, ChatFilterItemType filterType)
    {
        ChatConversacion itemfound = null;
        switch (filterType)
        {
            case ChatFilterItemType.ById:
                itemfound = await this.GetById(filter);
                break;
            case ChatFilterItemType.ByAppCodigoYUsuario:
                itemfound = await this.GetByAppCodigoYUsuario(filter);
                break;
            case ChatFilterItemType.ByNombre:
                itemfound = await this.GetByNombre(filter);
                break;
            case ChatFilterItemType.ByTipo:
                itemfound = await this.GetByTipo(filter);
                break;
        }
        return itemfound;
    }

    private async Task<ChatConversacion> GetById(ChatFilter filter)
    {
        if (filter?.nConversacionesChatId is int id)
        {
            return await GetConversationByIdAsync(id) ?? null;
        }
        return null;
    }

    private async Task<ChatConversacion> GetByAppCodigoYUsuario(ChatFilter filter)
    {
        var appCode = filter.cConversacionesChatAppCodigo;
        var userId = filter.cConversacionesChatUsuarioCreador;
        if (string.IsNullOrWhiteSpace(appCode) || string.IsNullOrWhiteSpace(userId))
            return null;
        var conversations = await GetUserConversationsAsync(appCode, userId);
        return conversations.FirstOrDefault();
    }

    private async Task<ChatConversacion> GetByNombre(ChatFilter filter)
    {
        string query = "USP_Chat_GetByNombre";
        var param = new DynamicParameters();
        param.Add("@cNombre", filter.cConversacionesChatNombre);
        return (await this.LoadData<ChatConversacion>(query, param)).FirstOrDefault();
    }

    private async Task<ChatConversacion> GetByTipo(ChatFilter filter)
    {
        string query = "USP_Chat_GetByTipo";
        var param = new DynamicParameters();
        param.Add("@cTipo", filter.cConversacionesChatTipo);
        return (await this.LoadData<ChatConversacion>(query, param)).FirstOrDefault();
    }

    public async Task<IEnumerable<ChatConversacion>> GetLstItem(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination)
    {
        IEnumerable<ChatConversacion> lstItemFound = new List<ChatConversacion>();
        switch (filterType)
        {
            case ChatFilterListType.ByPagination:
                lstItemFound = await this.GetByPagination(filter, pagination);
                break;
            case ChatFilterListType.ByAppCodigo:
                lstItemFound = await this.GetByAppCodigo(filter);
                break;
            case ChatFilterListType.ByUsuarioCreador:
                lstItemFound = await this.GetByUsuarioCreador(filter);
                break;
            case ChatFilterListType.ByActivas:
                lstItemFound = await this.GetByActivas(filter);
                break;
            case ChatFilterListType.ByTipo:
                lstItemFound = await this.GetByTipoList(filter);
                break;
            case ChatFilterListType.ByTerminoBusqueda:
                lstItemFound = await this.GetByTerminoBusqueda(filter);
                break;
            case ChatFilterListType.All:
                lstItemFound = await this.GetAll(filter);
                break;
        }
        return lstItemFound;
    }

    // Métodos específicos del dominio (mantenidos para compatibilidad)
    public async Task<List<ChatMensaje>> GetConversationMessagesAsync(long conversationId, int page = 1, int pageSize = 50)
    {
        string query = "USP_Chat_GetMessages";
        var param = new DynamicParameters();
        param.Add("@nConversacionId", conversationId);
        param.Add("@nPage", page);
        param.Add("@nPageSize", pageSize);
        
        var result = await this.LoadData<ChatMensaje>(query, param);
        return result.ToList();
    }

    public async Task<ChatMensaje> CreateMessageAsync(long conversationId, string senderId, string messageText, string messageType = "text")
    {
        string query = "USP_Chat_CreateMessage";
        var param = new DynamicParameters();
        param.Add("@nMensajesChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@nMensajesChatConversacionId", conversationId);
        param.Add("@cMensajesChatRemitenteId", senderId);
        param.Add("@cMensajesChatTexto", messageText);
        param.Add("@cMensajesChatTipo", messageType);

        await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
        int generatedId = param.Get<int>("@nMensajesChatId");

        return new ChatMensaje
        {
            nMensajesChatId = generatedId,
            nMensajesChatConversacionId = (int)conversationId,
            cMensajesChatRemitenteId = senderId,
            cMensajesChatTexto = messageText,
            cMensajesChatTipo = messageType,
            dMensajesChatFechaHora = DateTime.UtcNow
        };
    }

    public async Task<ChatConversacion> CreateConversationAsync(string appCode, string? conversationName, string conversationType, List<Guid> participantIds)
    {
        string query = "USP_Chat_CreateConversation";
        var param = new DynamicParameters();
        param.Add("@nConversacionesChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@cConversacionesChatAppCodigo", appCode);
        param.Add("@cConversacionesChatNombre", conversationName);
        param.Add("@cConversacionesChatTipo", conversationType);
        param.Add("@ParticipantIds", string.Join(",", participantIds));

        await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
        int generatedId = param.Get<int>("@nConversacionesChatId");

        return new ChatConversacion
        {
            nConversacionesChatId = generatedId,
            cConversacionesChatAppCodigo = appCode,
            cConversacionesChatNombre = conversationName,
            cConversacionesChatTipo = conversationType,
            dConversacionesChatFechaCreacion = DateTime.UtcNow,
            bConversacionesChatEstaActiva = true,
            dConversacionesChatUltimaActividad = DateTime.UtcNow
        };
    }

    public async Task<bool> AddUserToConversationAsync(long conversationId, Guid userId)
    {
        string query = "USP_Chat_AddUserToConversation";
        var param = new DynamicParameters();
        param.Add("@nConversacionId", conversationId);
        param.Add("@cUsuarioId", userId.ToString());

        return await this.UpdateOrDelete(query, param);
    }

    public async Task<bool> RemoveUserFromConversationAsync(long conversationId, Guid userId)
    {
        string query = "USP_Chat_RemoveUserFromConversation";
        var param = new DynamicParameters();
        param.Add("@nConversacionId", conversationId);
        param.Add("@cUsuarioId", userId.ToString());

        return await this.UpdateOrDelete(query, param);
    }

    public async Task<List<ChatConversacion>> GetUserConversationsAsync(string appCode, string userId)
    {
        string query = "USP_Chat_GetUserConversations";
        var param = new DynamicParameters();
        param.Add("@cAppCodigo", appCode);
        param.Add("@cUsuarioId", userId);
        // Paginación por defecto; el controlador puede paginar en memoria
        param.Add("@nPage", 1);
        param.Add("@nPageSize", 20);
        var result = await this.LoadData<ChatConversacion>(query, param);
        return result.ToList();
    }

    public async Task<ChatConversacion?> GetConversationByIdAsync(long id)
    {
        string query = "USP_Chat_GetConversationById";
        var param = new DynamicParameters();
        param.Add("@nConversacionesChatId", id);
        return (await this.LoadData<ChatConversacion>(query, param)).FirstOrDefault();
    }

    public async Task<bool> UpdateConversationAsync(ChatConversacion conversation)
    {
        return await Update(conversation);
    }

    public async Task<bool> DeleteConversationAsync(long id)
    {
        return await DeleteEntero((int)id);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByPagination(ChatFilter filter, Utils.Pagination pagination)
    {
        string query = "USP_Chat_GetByPagination";
        var param = new DynamicParameters();
        param.Add("@PageNumber", pagination.PageNumber);
        param.Add("@PageSize", pagination.PageSize);
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByAppCodigo(ChatFilter filter)
    {
        string query = "USP_Chat_GetByAppCodigo";
        var param = new DynamicParameters();
        param.Add("@cAppCodigo", filter.cConversacionesChatAppCodigo);
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByUsuarioCreador(ChatFilter filter)
    {
        string query = "USP_Chat_GetByUsuarioCreador";
        var param = new DynamicParameters();
        param.Add("@cUsuarioCreador", filter.cConversacionesChatUsuarioCreador);
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByActivas(ChatFilter filter)
    {
        string query = "USP_Chat_GetActivas";
        var param = new DynamicParameters();
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByTipoList(ChatFilter filter)
    {
        string query = "USP_Chat_GetByTipo";
        var param = new DynamicParameters();
        param.Add("@cTipo", filter.cConversacionesChatTipo);
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetByTerminoBusqueda(ChatFilter filter)
    {
        string query = "USP_Chat_Search";
        var param = new DynamicParameters();
        param.Add("@Termino", filter.TerminoBusqueda);
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatConversacion>> GetAll(ChatFilter filter)
    {
        string query = "USP_Chat_GetAll";
        var param = new DynamicParameters();
        return await this.LoadData<ChatConversacion>(query, param);
    }

    public async Task<IEnumerable<ChatMensaje>> GetConversationMessages(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination)
    {
        // Usamos el ID de conversación si está presente en el filtro
        if (filter?.nConversacionesChatId is int convId)
        {
            var messages = await GetConversationMessagesAsync(convId, pagination.PageNumber, pagination.PageSize);
            return messages;
        }
        // Si no hay ID, retornamos colección vacía
        return Enumerable.Empty<ChatMensaje>();
    }

    public async Task<ChatMensaje> CreateMessageAsync(long conversationId, Guid userId, string messageText, string messageType = "text")
    {
        // Reutilizamos la versión existente que acepta senderId como string
        return await CreateMessageAsync(conversationId, userId.ToString(), messageText, messageType);
    }

    public async Task<ChatMensaje> SendMessageAsync(ChatMensaje message)
    {
        string query = "USP_Chat_CreateMessage";
        var param = new DynamicParameters();
        param.Add("@nMensajesChatId", dbType: DbType.Int32, direction: ParameterDirection.Output);
        param.Add("@nMensajesChatConversacionId", message.nMensajesChatConversacionId);
        param.Add("@cMensajesChatRemitenteId", message.cMensajesChatRemitenteId);
        param.Add("@cMensajesChatTexto", message.cMensajesChatTexto);
        param.Add("@cMensajesChatTipo", message.cMensajesChatTipo ?? "text");

        await SqlMapper.ExecuteAsync(_connectionFactory.GetConnection(), query, param, commandType: CommandType.StoredProcedure);
        int generatedId = param.Get<int>("@nMensajesChatId");

        message.nMensajesChatId = generatedId;
        message.dMensajesChatFechaHora = DateTime.UtcNow;
        return message;
    }

    public async Task<bool> IsUserInConversationAsync(long conversationId, Guid userId)
    {
        string query = "USP_ChatParticipants_SelectByConversation";
        var param = new DynamicParameters();
        param.Add("@ConversationId", conversationId);
        var participants = await this.LoadData<ChatUsuario>(query, param);
        return participants.Any(p => string.Equals(p.cUsuariosChatId, userId.ToString(), StringComparison.OrdinalIgnoreCase));
    }

    public async Task<bool> ConversationExistsAsync(long conversationId)
    {
        var conv = await GetConversationByIdAsync(conversationId);
        return conv != null;
    }

    public async Task<List<ChatUsuario>> GetConversationParticipantsAsync(long conversationId)
    {
        string query = "USP_ChatParticipants_SelectByConversation";
        var param = new DynamicParameters();
        param.Add("@ConversationId", conversationId);
        var participants = await this.LoadData<ChatUsuario>(query, param);
        return participants.ToList();
    }

    public async Task<bool> MarkMessagesAsReadAsync(long conversationId, Guid userId)
    {
        string query = "USP_ChatMessages_MarkAsRead";
        var param = new DynamicParameters();
        param.Add("@ConversationId", conversationId);
        param.Add("@UserId", userId.ToString());
        return await this.UpdateOrDelete(query, param);
    }

    #endregion Public Methods
}