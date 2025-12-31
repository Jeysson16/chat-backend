using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using Supabase;
using Supabase.Postgrest;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;
using System.Text.Json;

namespace ChatModularMicroservice.Infrastructure;

public class ChatRepository : BaseRepository, ChatModularMicroservice.Repository.IChatRepository
{
    #region Constructor
    private readonly Supabase.Client? _supabaseClient;

    public ChatRepository(IConnectionFactory cn, Supabase.Client? supabaseClient = null) : base(cn)
    {
        _supabaseClient = supabaseClient;
    }

    // Implementaciones explícitas de interfaz para resolver problemas de firma
    Task<List<ChatConversacion>> IChatRepository.GetUserConversationsAsync(string appCode, string userId, string? perJurCodigo, int page, int pageSize)
    {
        return GetUserConversationsAsync(appCode, userId, perJurCodigo, page, pageSize);
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
        var conversations = await GetUserConversationsAsync(appCode, userId, "DEFAULT");
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
        if (_supabaseClient == null)
            return new List<ChatMensaje>();

        var response = await _supabaseClient
            .From<ChatModularMicroservice.Infrastructure.SupabaseModels.MensajeSupabaseFull>()
            .Filter("nMensajesConversacionId", Supabase.Postgrest.Constants.Operator.Equals, (int)conversationId)
            .Order(x => x.dMensajesFechaCreacion, Supabase.Postgrest.Constants.Ordering.Descending)
            .Range((page - 1) * pageSize, (page * pageSize) - 1)
            .Get();

        var models = response.Models ?? new List<ChatModularMicroservice.Infrastructure.SupabaseModels.MensajeSupabaseFull>();

        return models.Select(m => new ChatMensaje
        {
            nMensajesChatId = (long)(m.nMensajesId ?? 0),
            nMensajesChatConversacionId = m.nMensajesConversacionId,
            cMensajesChatRemitenteId = m.cMensajesRemitenteId ?? string.Empty,
            cMensajesChatTexto = m.cMensajesTexto ?? string.Empty,
            cMensajesChatTipo = m.cMensajesTipo ?? "text",
            dMensajesChatFechaHora = m.dMensajesFechaCreacion ?? DateTime.UtcNow,
            bMensajesChatEstaLeido = m.bMensajesEsLeido ?? false
        }).ToList();
    }

    public async Task<ChatMensaje> CreateMessageAsync(long conversationId, string senderId, string messageText, string messageType = "text")
    {
        if (_supabaseClient != null)
        {
            var lowerParams = new Dictionary<string, object?>
            {
                { "nmensajesconversacionid", (int)conversationId },
                { "cmensajesremitenteid", senderId },
                { "cmensajestexto", messageText },
                { "cmensajestipo", string.IsNullOrWhiteSpace(messageType) ? "text" : messageType }
            };

            var upperParams = new Dictionary<string, object?>
            {
                { "nMensajesConversacionId", (int)conversationId },
                { "cMensajesRemitenteId", senderId },
                { "cMensajesTexto", messageText },
                { "cMensajesTipo", string.IsNullOrWhiteSpace(messageType) ? "text" : messageType }
            };

            var rpc = await _supabaseClient.Rpc("USP_Chat_CreateMessage", lowerParams);
            if (rpc == null || string.IsNullOrWhiteSpace(rpc.Content))
            {
                rpc = await _supabaseClient.Rpc("USP_Chat_CreateMessage", upperParams);
            }

            var content = rpc?.Content ?? "0";
            int insertedId = 0;
            try
            {
                using var doc = JsonDocument.Parse(content);
                var root = doc.RootElement;
                if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
                {
                    var el = root[0];
                    if (el.TryGetProperty("nMensajesChatId", out var pId) && pId.ValueKind == JsonValueKind.Number)
                        insertedId = pId.GetInt32();
                    else if (el.TryGetProperty("nmensajeschatid", out var pId2) && pId2.ValueKind == JsonValueKind.Number)
                        insertedId = pId2.GetInt32();
                    else if (el.TryGetProperty("nMensajesId", out var pId3) && pId3.ValueKind == JsonValueKind.Number)
                        insertedId = pId3.GetInt32();
                }
                else if (root.ValueKind == JsonValueKind.Number)
                {
                    insertedId = root.GetInt32();
                }
                else if (root.ValueKind == JsonValueKind.String)
                {
                    int.TryParse(root.GetString()?.Trim('"'), out insertedId);
                }
            }
            catch
            {
                int.TryParse(content.Trim().Trim('"'), out insertedId);
            }
            if (insertedId <= 0)
            {
                throw new InvalidOperationException("Supabase RPC no devolvió un ID válido para el mensaje");
            }

            var getResp = await _supabaseClient
                .From<ChatModularMicroservice.Infrastructure.SupabaseModels.MensajeSupabaseFull>()
                .Filter("nMensajesId", Supabase.Postgrest.Constants.Operator.Equals, insertedId)
                .Get();

            var model = (getResp?.Models != null && getResp.Models.Count > 0)
                ? getResp.Models[0]
                : new ChatModularMicroservice.Infrastructure.SupabaseModels.MensajeSupabaseFull
                {
                    nMensajesId = insertedId,
                    nMensajesConversacionId = (int)conversationId,
                    cMensajesRemitenteId = senderId,
                    cMensajesTexto = messageText,
                    cMensajesTipo = string.IsNullOrWhiteSpace(messageType) ? "text" : messageType,
                    dMensajesFechaCreacion = DateTime.UtcNow,
                    bMensajesEsLeido = false
                };

            try
            {
                var convUpdate = new ChatModularMicroservice.Infrastructure.SupabaseModels.ConversacionSupabase
                {
                    nConversacionesId = (int)conversationId,
                    dConversacionesFechaActualizacion = DateTime.UtcNow
                };
                await _supabaseClient
                    .From<ChatModularMicroservice.Infrastructure.SupabaseModels.ConversacionSupabase>()
                    .Update(convUpdate, new Supabase.Postgrest.QueryOptions
                    {
                        Returning = Supabase.Postgrest.QueryOptions.ReturnType.Minimal
                    });
            }
            catch { }

            return new ChatMensaje
            {
                nMensajesChatId = (long)(model.nMensajesId ?? insertedId),
                nMensajesChatConversacionId = model.nMensajesConversacionId,
                cMensajesChatRemitenteId = model.cMensajesRemitenteId ?? senderId,
                cMensajesChatTexto = model.cMensajesTexto ?? messageText,
                cMensajesChatTipo = model.cMensajesTipo ?? (string.IsNullOrWhiteSpace(messageType) ? "text" : messageType),
                dMensajesChatFechaHora = model.dMensajesFechaCreacion ?? DateTime.UtcNow,
                bMensajesChatEstaLeido = model.bMensajesEsLeido ?? false
            };
        }
        throw new InvalidOperationException("Supabase client no disponible para crear mensaje");
    }

    public async Task<ChatConversacion> CreateConversationAsync(string appCode, string? conversationName, string conversationType, List<Guid> participantIds)
    {
        try
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
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Message.Contains("could not find stored procedure") || ex.Message.Contains("No se encontró el procedimiento almacenado"))
        {
            return await CreateConversationSupabaseAsync(appCode, conversationName, conversationType, participantIds.Select(p => p.ToString()).ToList());
        }
    }

    public async Task<ChatConversacion> CreateConversationAsync(string appCode, string? conversationName, string conversationType, List<string> participantIds)
    {
        try
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
        catch (Microsoft.Data.SqlClient.SqlException ex) when (ex.Message.Contains("could not find stored procedure") || ex.Message.Contains("No se encontró el procedimiento almacenado"))
        {
            return await CreateConversationSupabaseAsync(appCode, conversationName, conversationType, participantIds);
        }
    }

    private async Task<ChatConversacion> CreateConversationSupabaseAsync(string appCode, string? conversationName, string conversationType, List<string> participantIds)
    {
        if (_supabaseClient == null)
            throw new InvalidOperationException("Supabase client no disponible para creación de conversación");

        var parameters = new Dictionary<string, object?>
        {
            { "cConversacionesChatAppCodigo", appCode },
            { "cConversacionesChatNombre", conversationName ?? string.Empty },
            { "cConversacionesChatTipo", string.IsNullOrWhiteSpace(conversationType) ? "individual" : conversationType },
            { "ParticipantIds", string.Join(",", participantIds ?? new List<string>()) }
        };

        var rpc = await _supabaseClient.Rpc("usp_chat_createconversation", parameters);
        var content = rpc?.Content ?? "0";
        int conversationId = 0;
        int.TryParse(content.Trim().Trim('"'), out conversationId);
        if (conversationId <= 0)
        {
            throw new InvalidOperationException("Supabase RPC no devolvió un ID válido para la conversación");
        }

        return new ChatConversacion
        {
            nConversacionesChatId = conversationId,
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

    public async Task<bool> AddUserToConversationAsync(long conversationId, string userId)
    {
        if (_supabaseClient != null)
        {
            var parameters = new Dictionary<string, object?>
            {
                { "conversationid", (int)conversationId },
                { "usuarioid", userId }
            };
            var rpc = await _supabaseClient.Rpc("USP_Chat_AddUserToConversation", parameters);
            var content = (rpc?.Content ?? "true").Trim('"').ToLowerInvariant();
            return content == "true";
        }
        return false;
    }

    public async Task<bool> RemoveUserFromConversationAsync(long conversationId, Guid userId)
    {
        string query = "USP_Chat_RemoveUserFromConversation";
        var param = new DynamicParameters();
        param.Add("@nConversacionId", conversationId);
        param.Add("@cUsuarioId", userId.ToString());

        return await this.UpdateOrDelete(query, param);
    }

    public async Task<bool> RemoveUserFromConversationAsync(long conversationId, string userId)
    {
        if (_supabaseClient != null)
        {
            var parameters = new Dictionary<string, object?>
            {
                { "conversationid", (int)conversationId },
                { "usuarioid", userId }
            };
            var rpc = await _supabaseClient.Rpc("USP_Chat_RemoveUserFromConversation", parameters);
            var content = (rpc?.Content ?? "true").Trim('"').ToLowerInvariant();
            return content == "true";
        }
        return false;
    }

    public async Task<List<ChatConversacion>> GetUserConversationsAsync(string appCode, string userId, string? perJurCodigo, int page = 1, int pageSize = 50)
    {
        if (_supabaseClient != null)
        {
            var parametersLower = new Dictionary<string, object?>
            {
                { "cappcodigo", appCode },
                { "cusuarioid", userId },
                { "npage", page },
                { "npagesize", pageSize },
                { "perjurcodigo", string.IsNullOrWhiteSpace(perJurCodigo) ? "DEFAULT" : perJurCodigo }
            };

            var parametersUpper = new Dictionary<string, object?>
            {
                { "cAppCodigo", appCode },
                { "cUsuarioId", userId },
                { "nPage", page },
                { "nPageSize", pageSize },
                { "perJurCodigo", string.IsNullOrWhiteSpace(perJurCodigo) ? "DEFAULT" : perJurCodigo }
            };

            var rpc = await _supabaseClient.Rpc("USP_Chat_GetUserConversations", parametersLower);
            if (rpc == null || string.IsNullOrWhiteSpace(rpc.Content))
            {
                rpc = await _supabaseClient.Rpc("USP_Chat_GetUserConversations", parametersUpper);
            }
            var content = rpc?.Content ?? "[]";
            var list = new List<ChatConversacion>();

            using var doc = JsonDocument.Parse(content);
            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                long id = 0;
                if (elem.TryGetProperty("nconversacioneschatid", out var pId) && pId.ValueKind == JsonValueKind.Number)
                    id = pId.GetInt64();
                else if (elem.TryGetProperty("nConversacionesChatId", out var pId2) && pId2.ValueKind == JsonValueKind.Number)
                    id = pId2.GetInt64();

                string app = appCode;
                if (elem.TryGetProperty("cconversacioneschatappcodigo", out var pApp) && pApp.ValueKind == JsonValueKind.String)
                    app = pApp.GetString() ?? appCode;
                else if (elem.TryGetProperty("cConversacionesChatAppCodigo", out var pApp2) && pApp2.ValueKind == JsonValueKind.String)
                    app = pApp2.GetString() ?? appCode;

                string? name = null;
                if (elem.TryGetProperty("cconversacioneschatname", out var pName) && pName.ValueKind == JsonValueKind.String)
                    name = pName.GetString();
                else if (elem.TryGetProperty("cConversacionesChatNombre", out var pName2) && pName2.ValueKind == JsonValueKind.String)
                    name = pName2.GetString();

                string tipo = "individual";
                if (elem.TryGetProperty("cconversacioneschattype", out var pTipo) && pTipo.ValueKind == JsonValueKind.String)
                    tipo = pTipo.GetString() ?? "individual";
                else if (elem.TryGetProperty("cConversacionesChatTipo", out var pTipo2) && pTipo2.ValueKind == JsonValueKind.String)
                    tipo = pTipo2.GetString() ?? "individual";

                DateTime created = DateTime.UtcNow;
                if (elem.TryGetProperty("dtconversacioneschatcreatedat", out var pCreated) && pCreated.ValueKind == JsonValueKind.String)
                {
                    var s = pCreated.GetString();
                    if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out var dt)) created = dt;
                }
                else if (elem.TryGetProperty("dConversacionesChatFechaCreacion", out var pCreated2) && pCreated2.ValueKind == JsonValueKind.String)
                {
                    var s = pCreated2.GetString();
                    if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out var dt)) created = dt;
                }

                DateTime? last = null;
                if (elem.TryGetProperty("dtlastmessagetimestamp", out var pLast) && pLast.ValueKind == JsonValueKind.String)
                {
                    var s = pLast.GetString();
                    if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out var dt)) last = dt;
                }
                else if (elem.TryGetProperty("dConversacionesChatUltimaActividad", out var pLast2) && pLast2.ValueKind == JsonValueKind.String)
                {
                    var s = pLast2.GetString();
                    if (!string.IsNullOrWhiteSpace(s) && DateTime.TryParse(s, out var dt)) last = dt;
                }

                bool activa = false;
                if (elem.TryGetProperty("bconversacioneschatisactive", out var pAct))
                    activa = pAct.ValueKind == JsonValueKind.True;
                else if (elem.TryGetProperty("bConversacionesChatEstaActiva", out var pAct2))
                    activa = pAct2.ValueKind == JsonValueKind.True;

                list.Add(new ChatConversacion
                {
                    nConversacionesChatId = id,
                    cConversacionesChatAppCodigo = app,
                    cConversacionesChatNombre = name,
                    cConversacionesChatTipo = tipo,
                    dConversacionesChatFechaCreacion = created,
                    dConversacionesChatUltimaActividad = last,
                    bConversacionesChatEstaActiva = activa
                });
            }

            return list;
        }

        string query = "USP_Chat_GetUserConversations";
        var param = new DynamicParameters();
        param.Add("@cAppCodigo", appCode);
        param.Add("@cUsuarioId", userId);
        param.Add("@nPage", page);
        param.Add("@nPageSize", pageSize);
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
        if (_supabaseClient != null)
        {
            var parameters = new Dictionary<string, object?>
            {
                { "conversationid", (int)conversationId }
            };
            var rpc = await _supabaseClient.Rpc("USP_ChatParticipants_SelectByConversation", parameters);
            var content = rpc?.Content ?? "[]";
            var list = new List<ChatUsuario>();
            using var doc = JsonDocument.Parse(content);
            foreach (var elem in doc.RootElement.EnumerateArray())
            {
                var id = elem.TryGetProperty("cUsuariosChatId", out var pid) ? (pid.GetString() ?? string.Empty) : string.Empty;
                var name = elem.TryGetProperty("cUsuariosChatNombre", out var pName) ? (pName.GetString() ?? string.Empty) : string.Empty;
                var email = elem.TryGetProperty("cUsuariosChatEmail", out var pEmail) ? (pEmail.GetString() ?? string.Empty) : string.Empty;
                var avatar = elem.TryGetProperty("cUsuariosChatAvatar", out var pAvatar) ? (pAvatar.GetString() ?? string.Empty) : string.Empty;
                var online = elem.TryGetProperty("bUsuariosChatEstaEnLinea", out var pOnline) && pOnline.ValueKind == JsonValueKind.True;
                list.Add(new ChatUsuario
                {
                    cUsuariosChatId = id,
                    cUsuariosChatNombre = name,
                    cUsuariosChatEmail = email,
                    cUsuariosChatAvatar = avatar,
                    bUsuariosChatEstaEnLinea = online,
                    bUsuariosChatEstaActivo = true
                });
            }
            return list;
        }

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
