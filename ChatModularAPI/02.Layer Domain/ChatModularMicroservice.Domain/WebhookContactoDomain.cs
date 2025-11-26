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
    public class WebhookContactoDomain
    {
        #region Interfaces
        private IWebhookContactoRepository _WebhookContactoRepository { get; set; }
        #endregion

        #region Constructor 
        public WebhookContactoDomain(IWebhookContactoRepository webhookContactoRepository)
        {
            _WebhookContactoRepository = webhookContactoRepository ?? throw new ArgumentNullException(nameof(webhookContactoRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateWebhookContacto(WebhookContacto webhookContacto)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var createdWebhookContacto = await _WebhookContactoRepository.CreateWebhookContactoAsync(webhookContacto);
            if (createdWebhookContacto == null)
            {
                throw new Exception("Error creating webhook contacto");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditWebhookContacto(WebhookContacto webhookContacto)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!await _WebhookContactoRepository.UpdateWebhookContactoAsync(webhookContacto))
            {
                throw new Exception("Error updating webhook contacto");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteWebhookContacto(string webhookContactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookContactoRepository.DeleteWebhookContactoAsync(webhookContactoId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookContactoById(WebhookContactoFilter filter, WebhookContactoFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _WebhookContactoRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetWebhookContactoList(WebhookContactoFilter filter, WebhookContactoFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = await _WebhookContactoRepository.GetLstItem(filter, filterType, pagination);
            return lst;
        }

        public async Task<Utils.ItemResponseDT> ProcessContactRequest(string contactoId, string eventoTipo)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookContactoRepository.ProcessContactRequestAsync(contactoId, eventoTipo);
            if (!result)
            {
                throw new Exception("Error processing contact request");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> SendWebhookNotification(string webhookUrl, string payload)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookContactoRepository.SendWebhookNotificationAsync(webhookUrl, payload);
            if (!result)
            {
                throw new Exception("Error sending webhook notification");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> ValidateWebhookContacto(string webhookContactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookContactoRepository.ValidateWebhookContactoAsync(webhookContactoId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookContactoByEvent(string eventoTipo)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookContactoRepository.GetWebhookContactoByEventAsync(eventoTipo);
            return item;
        }

        public async Task<Utils.ItemResponseDT> RegisterWebhookEvent(string contactoId, string eventoTipo, string payload)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookContactoRepository.RegisterWebhookEventAsync(contactoId, eventoTipo, payload);
            if (!result)
            {
                throw new Exception("Error registering webhook event");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookEventHistory(string contactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookContactoRepository.GetWebhookEventHistoryAsync(contactoId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> RetryFailedWebhook(string webhookContactoId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookContactoRepository.RetryFailedWebhookAsync(webhookContactoId);
            if (!result)
            {
                throw new Exception("Error retrying failed webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }
        #endregion
    }
}
