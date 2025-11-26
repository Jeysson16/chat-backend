using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Implementación completa de IChatService que utiliza stored procedures a través del repositorio.
    /// </summary>
    public class ChatService : IChatService
    {
        private readonly IChatRepository _chatRepository;
        private readonly ILogger<ChatService> _logger;

        public ChatService(IChatRepository chatRepository, ILogger<ChatService> logger)
        {
            _chatRepository = chatRepository;
            _logger = logger;
        }

        public async Task<List<ChatConversacionDto>> GetUserConversationsAsync(string appCode, string perJurCodigo, string userId)
        {
            _logger.LogInformation("GetUserConversationsAsync for user {UserId} in app {AppCode} perJur {PerJur}", userId, appCode, perJurCodigo);
            
            try
            {
                var conversations = await _chatRepository.GetUserConversationsAsync(appCode, userId);
                
                // Convertir de ChatConversacion (modelo) a ChatConversacionDto
                var conversationDtos = conversations.Select(conv => new ChatConversacionDto
                {
                    nConversacionesChatId = conv.nConversacionesChatId,
                    cConversacionesChatAppCodigo = conv.cConversacionesChatAppCodigo,
                    cConversacionesChatNombre = conv.cConversacionesChatNombre ?? string.Empty,
                    cConversacionesChatTipo = conv.cConversacionesChatTipo,
                    cConversacionesChatUsuarioCreadorId = conv.cConversacionesChatUsuarioCreadorId ?? string.Empty,
                    dConversacionesChatFechaCreacion = conv.dConversacionesChatFechaCreacion,
                    dConversacionesChatUltimaActividad = conv.dConversacionesChatUltimaActividad,
                    bConversacionesChatEstaActiva = conv.bConversacionesChatEstaActiva
                }).ToList();
                
                return conversationDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting conversations for user {UserId}", userId);
                throw;
            }
        }

        public async Task<List<ChatMensajeDto>> GetConversationMessagesAsync(long conversationId, int page = 1, int pageSize = 50)
        {
            _logger.LogInformation("GetConversationMessagesAsync for conversation {ConversationId}", conversationId);
            
            try
            {
                var messages = await _chatRepository.GetConversationMessagesAsync(conversationId, page, pageSize);
                
                // Convertir de ChatMensaje (modelo) a ChatMensajeDto
                var messageDtos = messages.Select(msg => new ChatMensajeDto
                {
                    nMensajesChatId = msg.nMensajesChatId,
                    nMensajesChatConversacionId = msg.nMensajesChatConversacionId,
                    cMensajesChatRemitenteId = msg.cMensajesChatRemitenteId,
                    cMensajesChatTexto = msg.cMensajesChatTexto,
                    cMensajesChatTipo = msg.cMensajesChatTipo,
                    dMensajesChatFechaHora = msg.dMensajesChatFechaHora,
                    bMensajesChatEstaLeido = msg.bMensajesChatEstaLeido,
                    cMensajesChatRemitenteNombre = "" // Se puede pobular más adelante si es necesario
                }).ToList();
                
                return messageDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting messages for conversation {ConversationId}", conversationId);
                throw;
            }
        }

        public async Task<ChatMensajeDto> SendMessageAsync(string userId, EnviarMensajeDto messageDto)
        {
            _logger.LogInformation("SendMessageAsync by user {UserId} to conversation {ConversationId}", userId, messageDto.nConversacionesChatId);
            
            try
            {
                // Convertir el userId string a Guid
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    throw new ArgumentException($"Invalid user ID format: {userId}", nameof(userId));
                }
                
                var message = await _chatRepository.CreateMessageAsync(
                    messageDto.nConversacionesChatId, 
                    userGuid, 
                    messageDto.cMensajesChatTexto, 
                    messageDto.cMensajesChatTipo ?? "text");
                
                // Convertir de ChatMensaje (modelo) a ChatMensajeDto
                return new ChatMensajeDto
                {
                    nMensajesChatId = message.nMensajesChatId,
                    nMensajesChatConversacionId = message.nMensajesChatConversacionId,
                    cMensajesChatRemitenteId = message.cMensajesChatRemitenteId,
                    cMensajesChatTexto = message.cMensajesChatTexto,
                    cMensajesChatTipo = message.cMensajesChatTipo,
                    dMensajesChatFechaHora = message.dMensajesChatFechaHora,
                    bMensajesChatEstaLeido = message.bMensajesChatEstaLeido,
                    cMensajesChatRemitenteNombre = "" // Se puede pobular más adelante si es necesario
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message for user {UserId} in conversation {ConversationId}", userId, messageDto.nConversacionesChatId);
                throw;
            }
        }

        public async Task<ChatConversacionDto> CreateConversationAsync(string userId, CrearConversacionDto conversationDto)
        {
            _logger.LogInformation("CreateConversationAsync by user {UserId}", userId);
            
            try
            {
                // Convertir el userId string a Guid
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    throw new ArgumentException($"Invalid user ID format: {userId}", nameof(userId));
                }
                
                // Crear lista con el creador como único participante inicial
                var participantGuids = new List<Guid> { userGuid };
                
                var conversation = await _chatRepository.CreateConversationAsync(
                    conversationDto.cAppCodigo,
                    conversationDto.cConversacionesChatNombre,
                    conversationDto.cConversacionesChatTipo ?? "direct",
                    participantGuids);
                
                // Convertir de ChatConversacion (modelo) a ChatConversacionDto
                return new ChatConversacionDto
                {
                    nConversacionesChatId = conversation.nConversacionesChatId,
                    cConversacionesChatAppCodigo = conversation.cConversacionesChatAppCodigo,
                    cConversacionesChatNombre = conversation.cConversacionesChatNombre ?? string.Empty,
                    cConversacionesChatTipo = conversation.cConversacionesChatTipo,
                    cConversacionesChatUsuarioCreadorId = userId,
                    dConversacionesChatFechaCreacion = conversation.dConversacionesChatFechaCreacion,
                    bConversacionesChatEstaActiva = conversation.bConversacionesChatEstaActiva
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating conversation for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> JoinConversationAsync(string userId, long conversationId)
        {
            _logger.LogInformation("JoinConversationAsync user {UserId} conversation {ConversationId}", userId, conversationId);
            
            try
            {
                // Convertir el userId string a Guid
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    throw new ArgumentException($"Invalid user ID format: {userId}", nameof(userId));
                }
                
                var result = await _chatRepository.AddUserToConversationAsync(conversationId, userGuid);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error joining conversation {ConversationId} for user {UserId}", conversationId, userId);
                return false;
            }
        }

        public async Task<bool> LeaveConversationAsync(string userId, long conversationId)
        {
            _logger.LogInformation("LeaveConversationAsync user {UserId} conversation {ConversationId}", userId, conversationId);
            
            try
            {
                // Convertir el userId string a Guid
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    throw new ArgumentException($"Invalid user ID format: {userId}", nameof(userId));
                }
                
                var result = await _chatRepository.RemoveUserFromConversationAsync(conversationId, userGuid);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error leaving conversation {ConversationId} for user {UserId}", conversationId, userId);
                return false;
            }
        }

        public async Task<List<ChatUsuarioDto>> GetConversationParticipantsAsync(long conversationId)
        {
            _logger.LogInformation("GetConversationParticipantsAsync conversation {ConversationId}", conversationId);
            
            try
            {
                var participants = await _chatRepository.GetConversationParticipantsAsync(conversationId);
                
                // Convertir de ChatUsuario (modelo) a ChatUsuarioDto
                var participantDtos = participants.Select(participant => new ChatUsuarioDto
                {
                    cUsuariosChatId = participant.cUsuariosChatId,
                    cUsuariosChatNombre = participant.cUsuariosChatNombre,
                    cUsuariosChatEmail = participant.cUsuariosChatEmail,
                    cUsuariosChatAvatar = participant.cUsuariosChatAvatar ?? string.Empty,
                    bUsuariosChatEstaActivo = participant.bUsuariosChatEstaActivo,
                    bUsuariosChatEstaEnLinea = participant.bUsuariosChatEstaEnLinea,
                    dUsuariosChatFechaCreacion = participant.dUsuariosChatFechaCreacion,
                    dUsuariosChatUltimaVez = participant.dUsuariosChatUltimaConexion,
                    cUsuariosChatTelefono = string.Empty,
                    cUsuariosChatEstado = participant.bUsuariosChatEstaActivo ? "active" : "inactive",
                    nEmpresaId = 0,
                    nAplicacionId = 0,
                    bHasExistingConversation = false,
                    nExistingConversationId = null
                }).ToList();
                
                return participantDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting participants for conversation {ConversationId}", conversationId);
                throw;
            }
        }

        public async Task<bool> MarkMessagesAsReadAsync(string userId, long conversationId)
        {
            _logger.LogInformation("MarkMessagesAsReadAsync user {UserId} conversation {ConversationId}", userId, conversationId);
            
            try
            {
                // Convertir el userId string a Guid
                if (!Guid.TryParse(userId, out var userGuid))
                {
                    throw new ArgumentException($"Invalid user ID format: {userId}", nameof(userId));
                }
                
                var result = await _chatRepository.MarkMessagesAsReadAsync(conversationId, userGuid);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking messages as read for user {UserId} in conversation {ConversationId}", userId, conversationId);
                return false;
            }
        }
    }
}
