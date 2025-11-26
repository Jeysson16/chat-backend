using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de webhooks
/// </summary>
public interface IWebhookRepository : IDeleteIntRepository, IInsertIntRepository<Webhook>, IUpdateRepository<Webhook>
{
    Task<Webhook?> CreateWebhookAsync(Webhook webhook);
    Task<bool> DeleteWebhookAsync(string webhookId);
    Task<object?> GetItem(WebhookFilter filter, WebhookFilterItemType filterType);
    Task<List<object>?> GetLstItem(WebhookFilter filter, WebhookFilterListType filterType, Utils.Pagination pagination);
    Task<string?> GetWebhookConfigurationAsync(string webhookId);
    Task<bool> UpdateWebhookConfigurationAsync(string webhookId, string configuration);
    
    /// <summary>
    /// Registra un evento de webhook
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <param name="eventType">Tipo de evento</param>
    /// <param name="payload">Datos del evento</param>
    /// <returns>True si se registró correctamente, false en caso contrario</returns>
    Task<bool> RegisterWebhookEventAsync(string webhookId, string eventType, string payload);
    
    /// <summary>
    /// Obtiene el historial de eventos de un webhook específico
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <returns>Lista de eventos del webhook</returns>
    Task<List<object>?> GetWebhookEventHistoryAsync(string webhookId);
    
    /// <summary>
    /// Envía un webhook a una URL específica
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <param name="url">URL de destino</param>
    /// <param name="payload">Datos a enviar</param>
    /// <returns>True si se envió correctamente, false en caso contrario</returns>
    Task<bool> SendWebhookAsync(string webhookId, string url, string payload);
    
    /// <summary>
    /// Valida la configuración de un webhook
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <returns>True si la configuración es válida, false en caso contrario</returns>
    Task<bool> ValidateWebhookAsync(string webhookId);
    
    /// <summary>
    /// Reintenta el envío de un webhook fallido
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <returns>True si se reintentó correctamente, false en caso contrario</returns>
    Task<bool> RetryFailedWebhookAsync(string webhookId);
    
    /// <summary>
    /// Obtiene un webhook por su URL
    /// </summary>
    /// <param name="url">URL del webhook</param>
    /// <returns>El webhook si existe, null en caso contrario</returns>
    Task<Webhook?> GetWebhookByUrlAsync(string url);
    
    /// <summary>
    /// Actualiza un webhook existente
    /// </summary>
    /// <param name="webhook">Webhook a actualizar</param>
    /// <returns>True si se actualizó correctamente, false en caso contrario</returns>
    Task<bool> UpdateWebhookAsync(Webhook webhook);
    
    /// <summary>
    /// Habilita un webhook
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <returns>True si se habilitó correctamente, false en caso contrario</returns>
    Task<bool> EnableWebhookAsync(string webhookId);
    
    /// <summary>
    /// Deshabilita un webhook
    /// </summary>
    /// <param name="webhookId">ID del webhook</param>
    /// <returns>True si se deshabilitó correctamente, false en caso contrario</returns>
    Task<bool> DisableWebhookAsync(string webhookId);
}