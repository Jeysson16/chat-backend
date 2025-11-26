using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Pagination = ChatModularMicroservice.Shared.Utils.Pagination;
using System.Text.Json;

namespace ChatModularMicroservice.Api.Services;

/// <summary>
/// Adaptador de servicio para la gestión de configuraciones
/// </summary>
public class ConfiguracionServiceAdapter : IConfiguracionService
{
    private readonly IConfiguracionRepository _configuracionRepository;

    public ConfiguracionServiceAdapter(IConfiguracionRepository configuracionRepository)
    {
        _configuracionRepository = configuracionRepository;
    }

    public async Task<ConfiguracionDto?> ObtenerConfiguracionAsync(string codigoAplicacion)
    {
        var filter = new ConfiguracionFilter { ConfiguracionClave = codigoAplicacion };
        var configuracion = await _configuracionRepository.GetItem(filter, ConfiguracionFilterItemType.ConfiguracionClave);
        return configuracion == null ? null : ToDto(configuracion);
    }

    public async Task<ConfiguracionDto> GuardarConfiguracionAsync(ConfiguracionDto configuracion)
    {
        var configuracionAplicacion = ToEntity(configuracion);
        var newId = await _configuracionRepository.Insert(configuracionAplicacion);
        configuracionAplicacion.nConfiguracionAplicacionId = newId;
        return ToDto(configuracionAplicacion);
    }

    public async Task<bool> EliminarConfiguracionAsync(string codigoAplicacion)
    {
        var filter = new ConfiguracionFilter { ConfiguracionClave = codigoAplicacion };
        var configuracion = await _configuracionRepository.GetItem(filter, ConfiguracionFilterItemType.ConfiguracionClave);
        if (configuracion != null)
        {
            return await _configuracionRepository.DeleteEntero(configuracion.nConfiguracionAplicacionId);
        }
        return false;
    }

    public async Task<IEnumerable<ConfiguracionDto>> ListarConfiguracionesAsync()
    {
        var filter = new ConfiguracionFilter();
        var configuraciones = await _configuracionRepository.GetLstItem(filter, ConfiguracionFilterListType.TodasConfiguraciones, new Pagination { currentPage = 1, pageSize = 1000 });
        return configuraciones.Select(ToDto);
    }

    private ConfiguracionDto ToDto(Entities.Models.ConfiguracionAplicacion configuracion)
    {
        return new ConfiguracionDto
        {
            nConfiguracionId = configuracion.nConfiguracionAplicacionId,
            nAplicacionesId = configuracion.nAplicacionesId,
            nConfiguracionMaxTamanoArchivo = configuracion.nMaxTamanoArchivo,
            cConfiguracionTiposArchivosPermitidos = configuracion.cTiposArchivosPermitidos,
            bConfiguracionPermitirAdjuntos = configuracion.bPermitirAdjuntos,
            nConfiguracionMaxCantidadAdjuntos = configuracion.nMaxCantidadAdjuntos,
            bConfiguracionPermitirVisualizacionAdjuntos = configuracion.bPermitirVisualizacionAdjuntos,
            nConfiguracionMaxLongitudMensaje = configuracion.nMaxLongitudMensaje,
            bConfiguracionPermitirEmojis = configuracion.bPermitirEmojis,
            bConfiguracionPermitirNotificaciones = configuracion.bPermitirNotificaciones,
            bConfiguracionRequiereAutenticacion = configuracion.bRequiereAutenticacion,
            bConfiguracionPermitirMensajesAnonimos = configuracion.bPermitirMensajesAnonimos,
            nConfiguracionTiempoExpiracionSesion = configuracion.nTiempoExpiracionSesion,
            cContactosModoGestion = configuracion.cModoGestionContactos,
            cContactosUrlApiPersonas = configuracion.cUrlApiPersonas,
            bContactosSincronizar = configuracion.bSincronizarContactos,
            nContactosTiempoCacheSegundos = configuracion.nTiempoCacheContactos,
            dConfiguracionFechaCreacion = configuracion.dFechaCreacion,
            dConfiguracionFechaActualizacion = configuracion.dFechaActualizacion,
            bConfiguracionEsActiva = configuracion.bEsActiva
        };
    }

    private ChatModularMicroservice.Entities.Models.ConfiguracionAplicacion ToEntity(ConfiguracionDto dto)
    {
        return new ChatModularMicroservice.Entities.Models.ConfiguracionAplicacion
        {
            nConfiguracionAplicacionId = dto.nConfiguracionId,
            nAplicacionesId = 0, // No hay campo equivalente en ConfiguracionDto
            nMaxTamanoArchivo = 10485760, // Valor por defecto 10MB
            cTiposArchivosPermitidos = "jpg,jpeg,png,gif,pdf,doc,docx,txt,mp3,wav,mp4,avi",
            bPermitirAdjuntos = true,
            nMaxCantidadAdjuntos = 5,
            bPermitirVisualizacionAdjuntos = true,
            nMaxLongitudMensaje = 1000,
            bPermitirEmojis = true,
            bPermitirNotificaciones = true,
            bRequiereAutenticacion = true,
            bPermitirMensajesAnonimos = false,
            nTiempoExpiracionSesion = 3600,
            cModoGestionContactos = "LOCAL",
            cUrlApiPersonas = null,
            bSincronizarContactos = true,
            nTiempoCacheContactos = 300,
            dFechaCreacion = dto.dConfiguracionFechaCreacion,
            dFechaActualizacion = dto.dConfiguracionFechaActualizacion,
            bEsActiva = dto.bConfiguracionEsActiva
        };
    }
}