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
    public class WebhookDomain
    {
        #region Interfaces
        private IWebhookRepository _WebhookRepository { get; set; }
        #endregion

        #region Constructor 
        public WebhookDomain(IWebhookRepository webhookRepository)
        {
            _WebhookRepository = webhookRepository ?? throw new ArgumentNullException(nameof(webhookRepository));
        }
        #endregion

        #region Method Publics 
        public async Task<Utils.ItemResponseDT> CreateWebhook(Webhook webhook)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var createdWebhook = await _WebhookRepository.CreateWebhookAsync(webhook);
            if (createdWebhook == null)
            {
                throw new Exception("Error creating webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EditWebhook(Webhook webhook)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };

            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            if (!await _WebhookRepository.UpdateWebhookAsync(webhook))
            {
                throw new Exception("Error updating webhook");
            }
            tx.Complete();
            item.Item = true;
            return item;
        }

        public async Task<Utils.ItemResponseDT> DeleteWebhook(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookRepository.DeleteWebhookAsync(webhookId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookById(WebhookFilter filter, WebhookFilterItemType filterType)
        {
            Utils.ItemResponseDT response = new Utils.ItemResponseDT();
            response.Item = await _WebhookRepository.GetItem(filter, filterType);
            return response;
        }

        public async Task<Utils.ItemResponseDTLst> GetWebhookList(WebhookFilter filter, WebhookFilterListType filterType, Utils.Pagination pagination)
        {
            Utils.ItemResponseDTLst lst = new Utils.ItemResponseDTLst();
            lst.LstItem = await _WebhookRepository.GetLstItem(filter, filterType, pagination);
            return lst;
        }

        public async Task<Utils.ItemResponseDT> SendWebhook(string webhookUrl, string payload, string eventType)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookRepository.SendWebhookAsync(webhookUrl, payload, eventType);
            if (!result)
            {
                throw new Exception("Error sending webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> ValidateWebhook(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookRepository.ValidateWebhookAsync(webhookId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookByUrl(string webhookUrl)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookRepository.GetWebhookByUrlAsync(webhookUrl);
            return item;
        }

        public async Task<Utils.ItemResponseDT> RegisterWebhookEvent(string webhookId, string eventType, string payload)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookRepository.RegisterWebhookEventAsync(webhookId, eventType, payload);
            if (!result)
            {
                throw new Exception("Error registering webhook event");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookEventHistory(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookRepository.GetWebhookEventHistoryAsync(webhookId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> RetryFailedWebhook(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            var result = await _WebhookRepository.RetryFailedWebhookAsync(webhookId);
            if (!result)
            {
                throw new Exception("Error retrying failed webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> EnableWebhook(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _WebhookRepository.EnableWebhookAsync(webhookId))
            {
                throw new Exception("Error enabling webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> DisableWebhook(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _WebhookRepository.DisableWebhookAsync(webhookId))
            {
                throw new Exception("Error disabling webhook");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }

        public async Task<Utils.ItemResponseDT> GetWebhookConfiguration(string webhookId)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            item.Item = await _WebhookRepository.GetWebhookConfigurationAsync(webhookId);
            return item;
        }

        public async Task<Utils.ItemResponseDT> UpdateWebhookConfiguration(string webhookId, string configuration)
        {
            Utils.ItemResponseDT item = new Utils.ItemResponseDT() { Item = false };
            
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            
            if (!await _WebhookRepository.UpdateWebhookConfigurationAsync(webhookId, configuration))
            {
                throw new Exception("Error updating webhook configuration");
            }
            
            item.Item = true;
            tx.Complete();
            return item;
        }
        #endregion
    }
}
