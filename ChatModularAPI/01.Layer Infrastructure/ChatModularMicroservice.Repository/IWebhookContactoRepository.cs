using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de webhooks de contactos
/// </summary>
public interface IWebhookContactoRepository : IDeleteIntRepository, IInsertIntRepository<WebhookContacto>, IUpdateRepository<WebhookContacto>
{
    Task<WebhookContacto?> CreateWebhookContactoAsync(WebhookContacto webhookContacto);
    Task<bool> DeleteWebhookContactoAsync(string webhookContactoId);
    Task<object?> GetItem(WebhookContactoFilter filter, WebhookContactoFilterItemType filterType);
    Task<List<object>?> GetLstItem(WebhookContactoFilter filter, WebhookContactoFilterListType filterType, Utils.Pagination pagination);
    
    /// <summary>
    /// Obtiene el historial de eventos de webhook para un contacto específico
    /// </summary>
    /// <param name="contactoId">ID del contacto</param>
    /// <returns>Lista de eventos de webhook</returns>
    Task<List<WebhookContacto>?> GetWebhookEventHistoryAsync(string contactoId);
    
    /// <summary>
    /// Reintenta un webhook fallido
    /// </summary>
    /// <param name="webhookContactoId">ID del webhook de contacto</param>
    /// <returns>True si se pudo reintentar, false en caso contrario</returns>
    Task<bool> RetryFailedWebhookAsync(string webhookContactoId);
    
    /// <summary>
    /// Registra un evento de webhook para un contacto
    /// </summary>
    /// <param name="contactoId">ID del contacto</param>
    /// <param name="eventoTipo">Tipo de evento</param>
    /// <param name="payload">Datos del evento</param>
    /// <returns>True si se registró correctamente, false en caso contrario</returns>
    Task<bool> RegisterWebhookEventAsync(string contactoId, string eventoTipo, string payload);
    
    /// <summary>
    /// Obtiene un webhook de contacto por tipo de evento
    /// </summary>
    /// <param name="eventoTipo">Tipo de evento</param>
    /// <returns>Webhook de contacto encontrado</returns>
    Task<WebhookContacto?> GetWebhookContactoByEventAsync(string eventoTipo);
    
    /// <summary>
    /// Procesa una solicitud de contacto
    /// </summary>
    /// <param name="contactoId">ID del contacto</param>
    /// <param name="eventoTipo">Tipo de evento</param>
    /// <returns>True si se procesó correctamente, false en caso contrario</returns>
    Task<bool> ProcessContactRequestAsync(string contactoId, string eventoTipo);
    
    /// <summary>
    /// Valida un webhook de contacto
    /// </summary>
    /// <param name="webhookContactoId">ID del webhook de contacto</param>
    /// <returns>True si es válido, false en caso contrario</returns>
    Task<bool> ValidateWebhookContactoAsync(string webhookContactoId);
    
    /// <summary>
    /// Envía una notificación webhook
    /// </summary>
    /// <param name="webhookUrl">URL del webhook</param>
    /// <param name="payload">Datos a enviar</param>
    /// <returns>True si se envió correctamente, false en caso contrario</returns>
    Task<bool> SendWebhookNotificationAsync(string webhookUrl, string payload);
    
    /// <summary>
    /// Actualiza un webhook de contacto existente
    /// </summary>
    /// <param name="webhookContacto">Webhook de contacto a actualizar</param>
    /// <returns>True si se actualizó correctamente, false en caso contrario</returns>
    Task<bool> UpdateWebhookContactoAsync(WebhookContacto webhookContacto);
}