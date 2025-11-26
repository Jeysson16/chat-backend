using System.Threading.Tasks;

namespace ChatModularMicroservice.Api.Webhooks
{
    /// <summary>
    /// Contrato para el servicio de webhooks de la API
    /// </summary>
    public interface IWebhookService
    {
        /// <summary>
        /// Envía un webhook a la URL especificada con el payload indicado
        /// </summary>
        Task<bool> EnviarWebhookAsync(string url, object payload, string tipoEvento);

        /// <summary>
        /// Valida que la configuración de una aplicación sea correcta para el envío de webhooks
        /// </summary>
        Task<bool> ValidarConfiguracionAsync(string codigoAplicacion);

        /// <summary>
        /// Genera una signatura HMAC para el payload usando el secreto proporcionado
        /// </summary>
        Task<string> GenerarSignaturaAsync(string payload, string secreto);

        /// <summary>
        /// Valida que la signatura recibida coincida con la esperada
        /// </summary>
        Task<bool> ValidarSignaturaAsync(string payload, string signatura, string secreto);
    }
}