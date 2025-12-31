using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Shared;
using Pagination = ChatModularMicroservice.Shared.Utils.Pagination;
using System.Text.Json;

namespace ChatModularMicroservice.Api.Services;

/// <summary>
/// Adaptador de servicio para la gestión de configuraciones de empresa
/// </summary>
public class ConfiguracionEmpresaServiceAdapter : IConfiguracionEmpresaService
{
    private readonly IConfiguracionEmpresaRepository _configuracionEmpresaRepository;

    public ConfiguracionEmpresaServiceAdapter(IConfiguracionEmpresaRepository configuracionEmpresaRepository)
    {
        _configuracionEmpresaRepository = configuracionEmpresaRepository;
    }

    public async Task<List<ConfiguracionEmpresaDto>> GetAllAsync()
    {
        return await _configuracionEmpresaRepository.GetAllAsync();
    }

    public async Task<ConfiguracionEmpresaDto?> GetByIdAsync(int id)
    {
        return await _configuracionEmpresaRepository.GetByIdAsync(id);
    }

    public async Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAsync(int empresaId)
    {
        return await _configuracionEmpresaRepository.GetByEmpresaAsync(empresaId);
    }

    public async Task<List<ConfiguracionEmpresaDto>> GetByAplicacionAsync(int aplicacionId)
    {
        return await _configuracionEmpresaRepository.GetByAplicacionAsync(aplicacionId);
    }

    public async Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAndAplicacionAsync(int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.GetByEmpresaAndAplicacionAsync(empresaId, aplicacionId);
    }

    public async Task<ConfiguracionEmpresaDto?> GetByClaveAsync(string clave, int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.GetByClaveAsync(clave, empresaId, aplicacionId);
    }

    public async Task<List<ConfiguracionEmpresaDto>> GetActivasAsync()
    {
        return await _configuracionEmpresaRepository.GetActivasAsync();
    }

    public async Task<List<ConfiguracionEmpresaDto>> SearchAsync(string terminoBusqueda)
    {
        return await _configuracionEmpresaRepository.SearchAsync(terminoBusqueda);
    }

    public async Task<List<ConfiguracionEmpresaAgrupadaDto>> GetAgrupadasAsync(int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.GetAgrupadasAsync();
    }

    public async Task<ConfiguracionEmpresaDto> CreateAsync(CreateConfiguracionEmpresaDto createDto)
    {
        return await _configuracionEmpresaRepository.CreateAsync(createDto);
    }

    public async Task<ConfiguracionEmpresaDto?> UpdateAsync(int id, UpdateConfiguracionEmpresaDto updateDto)
    {
        return await _configuracionEmpresaRepository.UpdateAsync(id, updateDto);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _configuracionEmpresaRepository.DeleteAsync(id);
    }

    public async Task<bool> ExistsByClaveAsync(string clave, int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.ExistsByClaveAsync(clave, empresaId, aplicacionId);
    }

    public async Task<List<ConfiguracionEmpresaDto>> CopiarConfiguracionesDeAplicacionAsync(int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.CopiarConfiguracionesDeAplicacionAsync(empresaId, aplicacionId);
    }

    public async Task<bool> RestaurarConfiguracionesPorDefectoAsync(int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.RestaurarConfiguracionesPorDefectoAsync(empresaId, aplicacionId);
    }

    public async Task<List<ConfiguracionHeredadaDto>> GetConfiguracionesHeredadasAsync(int empresaId, int aplicacionId)
    {
        return await _configuracionEmpresaRepository.GetConfiguracionesHeredadasAsync(empresaId, aplicacionId);
    }

    private ConfiguracionEmpresaDto ToDto(ChatModularMicroservice.Entities.Models.ConfiguracionEmpresa configuracion)
    {
        return new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = configuracion.nConfigEmpresaId,
            nEmpresasId = configuracion.nConfigEmpresaEmpresaId,
            nAplicacionesId = configuracion.nConfigEmpresaAplicacionId,
            cConfiguracionEmpresaClave = "CONFIG_EMPRESA_" + configuracion.nConfigEmpresaId,
            cConfiguracionEmpresaValor = System.Text.Json.JsonSerializer.Serialize(new {
                Nombre = configuracion.cConfigEmpresaNombre,
                Descripcion = configuracion.cConfigEmpresaDescripcion,
                Dominio = configuracion.cConfigEmpresaDominio,
                ColorPrimario = configuracion.cConfigEmpresaColorPrimario,
                ColorSecundario = configuracion.cConfigEmpresaColorSecundario,
                UrlLogo = configuracion.cConfigEmpresaUrlLogo,
                FuentePersonalizada = configuracion.cConfigEmpresaFuentePersonalizada,
                MaxUsuarios = configuracion.nConfigEmpresaMaxUsuarios,
                MaxCanales = configuracion.nConfigEmpresaMaxCanales,
                CuotaAlmacenamientoGB = configuracion.nConfigEmpresaCuotaAlmacenamientoGB,
                TiempoSesionMinutos = configuracion.nConfigEmpresaTiempoSesionMinutos,
                HabilitarCompartirArchivos = configuracion.bConfigEmpresaHabilitarCompartirArchivos,
                HabilitarNotificaciones = configuracion.bConfigEmpresaHabilitarNotificaciones,
                HabilitarIntegraciones = configuracion.bConfigEmpresaHabilitarIntegraciones,
                HabilitarAnaliticas = configuracion.bConfigEmpresaHabilitarAnaliticas
            }),
            cConfiguracionEmpresaTipo = "JSON",
            cConfiguracionEmpresaDescripcion = configuracion.cConfigEmpresaDescripcion ?? "Configuración de empresa",
            dConfiguracionEmpresaFechaCreacion = configuracion.dConfigEmpresaFechaCreacion,
            dConfiguracionEmpresaFechaActualizacion = configuracion.dConfigEmpresaFechaActualizacion,
            bConfiguracionEmpresaEsActiva = configuracion.bConfigEmpresaEsActiva
        };
    }

    private ChatModularMicroservice.Entities.Models.ConfiguracionEmpresa ToEntity(CreateConfiguracionEmpresaDto dto)
    {
        var configuracion = new ChatModularMicroservice.Entities.Models.ConfiguracionEmpresa
        {
            nConfigEmpresaEmpresaId = dto.NEmpresasId,
            nConfigEmpresaAplicacionId = dto.NAplicacionesId,
            dConfigEmpresaFechaCreacion = DateTime.UtcNow,
            dConfigEmpresaFechaActualizacion = DateTime.UtcNow,
            bConfigEmpresaEsActiva = dto.BConfiguracionEmpresaEsActiva
        };

        // Parsear el valor JSON para obtener las propiedades
        if (!string.IsNullOrEmpty(dto.CConfiguracionEmpresaValor))
        {
            try
            {
                var valorJson = System.Text.Json.JsonDocument.Parse(dto.CConfiguracionEmpresaValor);
                var root = valorJson.RootElement;
                
                if (root.TryGetProperty("Nombre", out var nombre))
                    configuracion.cConfigEmpresaNombre = nombre.GetString() ?? string.Empty;
                if (root.TryGetProperty("Descripcion", out var descripcion))
                    configuracion.cConfigEmpresaDescripcion = descripcion.GetString();
                if (root.TryGetProperty("Dominio", out var dominio))
                    configuracion.cConfigEmpresaDominio = dominio.GetString() ?? string.Empty;
                if (root.TryGetProperty("ColorPrimario", out var colorPrimario))
                    configuracion.cConfigEmpresaColorPrimario = colorPrimario.GetString() ?? "#233559";
                if (root.TryGetProperty("ColorSecundario", out var colorSecundario))
                    configuracion.cConfigEmpresaColorSecundario = colorSecundario.GetString() ?? "#C90000";
                if (root.TryGetProperty("UrlLogo", out var urlLogo))
                    configuracion.cConfigEmpresaUrlLogo = urlLogo.GetString();
                if (root.TryGetProperty("FuentePersonalizada", out var fuente))
                    configuracion.cConfigEmpresaFuentePersonalizada = fuente.GetString() ?? "Roboto, Arial, sans-serif";
                if (root.TryGetProperty("MaxUsuarios", out var maxUsuarios) && maxUsuarios.TryGetInt32(out var maxUsers))
                    configuracion.nConfigEmpresaMaxUsuarios = maxUsers;
                if (root.TryGetProperty("MaxCanales", out var maxCanales) && maxCanales.TryGetInt32(out var maxChannels))
                    configuracion.nConfigEmpresaMaxCanales = maxChannels;
                if (root.TryGetProperty("CuotaAlmacenamientoGB", out var cuota) && cuota.TryGetInt32(out var storage))
                    configuracion.nConfigEmpresaCuotaAlmacenamientoGB = storage;
                if (root.TryGetProperty("TiempoSesionMinutos", out var tiempo) && tiempo.TryGetInt32(out var session))
                    configuracion.nConfigEmpresaTiempoSesionMinutos = session;
                if (root.TryGetProperty("HabilitarCompartirArchivos", out var archivos))
                    configuracion.bConfigEmpresaHabilitarCompartirArchivos = archivos.GetBoolean();
                if (root.TryGetProperty("HabilitarNotificaciones", out var notificaciones))
                    configuracion.bConfigEmpresaHabilitarNotificaciones = notificaciones.GetBoolean();
                if (root.TryGetProperty("HabilitarIntegraciones", out var integraciones))
                    configuracion.bConfigEmpresaHabilitarIntegraciones = integraciones.GetBoolean();
                if (root.TryGetProperty("HabilitarAnaliticas", out var analiticas))
                    configuracion.bConfigEmpresaHabilitarAnaliticas = analiticas.GetBoolean();
            }
            catch
            {
                // Si hay error en el JSON, usar valores por defecto
                configuracion.cConfigEmpresaNombre = "Configuración Empresa";
                configuracion.cConfigEmpresaDominio = "empresa.com";
                configuracion.cConfigEmpresaColorPrimario = "#233559";
                configuracion.cConfigEmpresaColorSecundario = "#C90000";
                configuracion.cConfigEmpresaFuentePersonalizada = "Roboto, Arial, sans-serif";
                configuracion.nConfigEmpresaMaxUsuarios = 100;
                configuracion.nConfigEmpresaMaxCanales = 50;
                configuracion.nConfigEmpresaCuotaAlmacenamientoGB = 10;
                configuracion.nConfigEmpresaTiempoSesionMinutos = 30;
                configuracion.bConfigEmpresaHabilitarCompartirArchivos = true;
                configuracion.bConfigEmpresaHabilitarNotificaciones = true;
                configuracion.bConfigEmpresaHabilitarIntegraciones = false;
                configuracion.bConfigEmpresaHabilitarAnaliticas = true;
            }
        }
        else
        {
            // Si no hay valor JSON, usar valores por defecto
            configuracion.cConfigEmpresaNombre = "Configuración Empresa";
            configuracion.cConfigEmpresaDominio = "empresa.com";
            configuracion.cConfigEmpresaColorPrimario = "#233559";
            configuracion.cConfigEmpresaColorSecundario = "#C90000";
            configuracion.cConfigEmpresaFuentePersonalizada = "Roboto, Arial, sans-serif";
            configuracion.nConfigEmpresaMaxUsuarios = 100;
            configuracion.nConfigEmpresaMaxCanales = 50;
            configuracion.nConfigEmpresaCuotaAlmacenamientoGB = 10;
            configuracion.nConfigEmpresaTiempoSesionMinutos = 30;
            configuracion.bConfigEmpresaHabilitarCompartirArchivos = true;
            configuracion.bConfigEmpresaHabilitarNotificaciones = true;
            configuracion.bConfigEmpresaHabilitarIntegraciones = false;
            configuracion.bConfigEmpresaHabilitarAnaliticas = true;
        }

        return configuracion;
    }

    private ChatModularMicroservice.Entities.Models.ConfiguracionEmpresa ToEntity(UpdateConfiguracionEmpresaDto dto)
    {
        var configuracion = new ChatModularMicroservice.Entities.Models.ConfiguracionEmpresa
        {
            nConfigEmpresaId = dto.NConfiguracionEmpresaId,
            dConfigEmpresaFechaActualizacion = DateTime.UtcNow
        };

        // Si hay valor, parsear el JSON para obtener las propiedades
        if (!string.IsNullOrEmpty(dto.CConfiguracionEmpresaValor))
        {
            try
            {
                var valorJson = System.Text.Json.JsonDocument.Parse(dto.CConfiguracionEmpresaValor);
                var root = valorJson.RootElement;
                
                if (root.TryGetProperty("Nombre", out var nombre))
                    configuracion.cConfigEmpresaNombre = nombre.GetString() ?? string.Empty;
                if (root.TryGetProperty("Descripcion", out var descripcion))
                    configuracion.cConfigEmpresaDescripcion = descripcion.GetString();
                if (root.TryGetProperty("Dominio", out var dominio))
                    configuracion.cConfigEmpresaDominio = dominio.GetString() ?? string.Empty;
                if (root.TryGetProperty("ColorPrimario", out var colorPrimario))
                    configuracion.cConfigEmpresaColorPrimario = colorPrimario.GetString() ?? "#233559";
                if (root.TryGetProperty("ColorSecundario", out var colorSecundario))
                    configuracion.cConfigEmpresaColorSecundario = colorSecundario.GetString() ?? "#C90000";
                if (root.TryGetProperty("UrlLogo", out var urlLogo))
                    configuracion.cConfigEmpresaUrlLogo = urlLogo.GetString();
                if (root.TryGetProperty("FuentePersonalizada", out var fuente))
                    configuracion.cConfigEmpresaFuentePersonalizada = fuente.GetString() ?? "Roboto, Arial, sans-serif";
                if (root.TryGetProperty("MaxUsuarios", out var maxUsuarios) && maxUsuarios.TryGetInt32(out var maxUsers))
                    configuracion.nConfigEmpresaMaxUsuarios = maxUsers;
                if (root.TryGetProperty("MaxCanales", out var maxCanales) && maxCanales.TryGetInt32(out var maxChannels))
                    configuracion.nConfigEmpresaMaxCanales = maxChannels;
                if (root.TryGetProperty("CuotaAlmacenamientoGB", out var cuota) && cuota.TryGetInt32(out var storage))
                    configuracion.nConfigEmpresaCuotaAlmacenamientoGB = storage;
                if (root.TryGetProperty("TiempoSesionMinutos", out var tiempo) && tiempo.TryGetInt32(out var session))
                    configuracion.nConfigEmpresaTiempoSesionMinutos = session;
                if (root.TryGetProperty("HabilitarCompartirArchivos", out var archivos))
                    configuracion.bConfigEmpresaHabilitarCompartirArchivos = archivos.GetBoolean();
                if (root.TryGetProperty("HabilitarNotificaciones", out var notificaciones))
                    configuracion.bConfigEmpresaHabilitarNotificaciones = notificaciones.GetBoolean();
                if (root.TryGetProperty("HabilitarIntegraciones", out var integraciones))
                    configuracion.bConfigEmpresaHabilitarIntegraciones = integraciones.GetBoolean();
                if (root.TryGetProperty("HabilitarAnaliticas", out var analiticas))
                    configuracion.bConfigEmpresaHabilitarAnaliticas = analiticas.GetBoolean();
            }
            catch
            {
                // Si hay error en el JSON, no actualizar esas propiedades
            }
        }

        if (!string.IsNullOrEmpty(dto.CConfiguracionEmpresaDescripcion))
            configuracion.cConfigEmpresaDescripcion = dto.CConfiguracionEmpresaDescripcion;
            
        if (dto.BConfiguracionEmpresaEsActiva.HasValue)
            configuracion.bConfigEmpresaEsActiva = dto.BConfiguracionEmpresaEsActiva.Value;

        return configuracion;
    }
}
