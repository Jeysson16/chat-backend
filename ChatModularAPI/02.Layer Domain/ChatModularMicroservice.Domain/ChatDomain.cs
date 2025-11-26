using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ChatModularMicroservice.Domain
{
    public class ChatDomain
    {
        #region Interfaces
        private IChatRepository _ChatRepository { get; set; }
        #endregion

        #region Constructor 
        public ChatDomain(IChatRepository chatRepository)
        {
            _ChatRepository = chatRepository ?? throw new ArgumentNullException(nameof(chatRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateConversation(ChatConversacion conversation)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var createdConversation = await _ChatRepository.CreateConversationAsync(
                conversation.cConversacionesChatAppCodigo,
                conversation.cConversacionesChatNombre,
                conversation.cConversacionesChatTipo,
                new List<Guid>() // Lista vacía por defecto, se pueden agregar participantes después
            );
            if (createdConversation == null)
            {
                throw new Exception("Error creating conversation");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> SendMessage(ChatMensaje message)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            // Convertir de Domain a Entities.Models
            var entityMessage = new Entities.Models.ChatMensaje
            {
                Id = message.Id,
                nMensajesChatId = message.nMensajesChatId,
                nMensajesChatConversacionId = message.nMensajesChatConversacionId,
                cMensajesChatRemitenteId = message.cMensajesChatRemitenteId,
                cMensajesChatTexto = message.cMensajesChatTexto,
                cMensajesChatTipo = message.cMensajesChatTipo,
                dMensajesChatFechaHora = message.dMensajesChatFechaHora,
                bMensajesChatEstaLeido = message.bMensajesChatEstaLeido,
                CreatedAt = message.CreatedAt
            };
            
            var sentMessage = await _ChatRepository.SendMessageAsync(entityMessage);
            if (sentMessage == null)
            {
                throw new Exception("Error sending message");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetConversationById(ChatFilter filter, ChatFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _ChatRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetUserConversations(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = (await _ChatRepository.GetLstItem(filter, filterType, pagination)).Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDTLst> GetConversationMessages(ChatFilter filter, ChatFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = (await _ChatRepository.GetConversationMessages(filter, filterType, pagination)).Cast<object>().ToList();
            return lst;
        }

        public async Task<Utils.ItemResponseDT> UpdateConversation(ChatConversacion conversation)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            // Convertir de Domain a Entities.Models
            var entityConversation = new Entities.Models.ChatConversacion
            {
                Id = conversation.Id,
                nConversacionesChatId = conversation.nConversacionesChatId,
                cConversacionesChatAppCodigo = conversation.cConversacionesChatAppCodigo,
                cConversacionesChatNombre = conversation.cConversacionesChatNombre,
                cConversacionesChatDescripcion = conversation.cConversacionesChatDescripcion,
                cConversacionesChatTipo = conversation.cConversacionesChatTipo,
                cConversacionesChatUsuarioCreadorId = conversation.cConversacionesChatUsuarioCreadorId,
                dConversacionesChatFechaCreacion = conversation.dConversacionesChatFechaCreacion,
                dConversacionesChatUltimaActividad = conversation.dConversacionesChatUltimaActividad,
                bConversacionesChatEstaActiva = conversation.bConversacionesChatEstaActiva,
                CreatedAt = conversation.CreatedAt,
                UpdatedAt = conversation.UpdatedAt
            };

            if (!await _ChatRepository.UpdateConversationAsync(entityConversation))
            {
                throw new Exception("Error updating conversation");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteConversation(string conversationId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (long.TryParse(conversationId, out long id))
            {
                item.Item = await _ChatRepository.DeleteConversationAsync(id);
            }
            return item;
        }

        public async Task<Utils.ItemResponseDT> ConversationExists(string conversationId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            if (long.TryParse(conversationId, out long id))
            {
                item.Item = await _ChatRepository.ConversationExistsAsync(id);
            }
            return item;
        }
        #endregion
    }
}
