using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Shared.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de configuración de chat
    /// </summary>
    public interface IConfiguracionChatService
    {
        // Métodos usados por ConfiguracionChatController
        Task<ItemResponseDT> ObtenerConfiguracionChatAsync(Guid empresaId, Guid aplicacionId);
        Task<ItemResponseDT> ActualizarConfiguracionChatAsync(Guid empresaId, Guid aplicacionId, ConfiguracionChatDto configuracion);
        Task<ItemResponseDT> InicializarConfiguracionPorDefectoAsync(Guid empresaId, Guid aplicacionId);
        Task<ItemResponseDT> ObtenerConfiguracionPorClaveAsync(Guid empresaId, Guid aplicacionId, string clave);
        Task<ItemResponseDT> EstablecerConfiguracionPorClaveAsync(Guid empresaId, Guid aplicacionId, string clave, string valor, string? descripcion = null);
        Task<ItemResponseDT> ValidarFuncionalidadAsync(Guid empresaId, Guid aplicacionId, string funcionalidad);
        Task<ItemResponseDT> ObtenerTodasConfiguracionesAsync(Guid empresaId, Guid aplicacionId);
        Task<ItemResponseDT> CopiarConfiguracionesAsync(Guid empresaOrigenId, Guid empresaDestinoId, Guid aplicacionId);
        Task<ItemResponseDT> ResetearConfiguracionesAsync(Guid empresaId, Guid aplicacionId);

        // Métodos CRUD genéricos por código de aplicación
        Task<ConfiguracionChatDto?> ObtenerConfiguracionChatAsync(string codigoAplicacion);
        Task<ConfiguracionChatDto> GuardarConfiguracionChatAsync(ConfiguracionChatDto configuracion);
        Task<bool> EliminarConfiguracionChatAsync(string codigoAplicacion);
        Task<IEnumerable<ConfiguracionChatDto>> ListarConfiguracionesChatAsync();
        Task<bool> ValidarConfiguracionChatAsync(ConfiguracionChatDto configuracion);
    }
}