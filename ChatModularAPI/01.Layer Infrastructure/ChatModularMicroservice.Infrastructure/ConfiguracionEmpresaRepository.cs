using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Entities.Models;
using CEFilter = ChatModularMicroservice.Entities.ConfiguracionEmpresaFilter;
using CEItemType = ChatModularMicroservice.Entities.ConfiguracionEmpresaFilterItemType;
using CEListType = ChatModularMicroservice.Entities.ConfiguracionEmpresaFilterListType;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using ChatModularMicroservice.Infrastructure.Repositories;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de configuraciones de empresa usando stored procedures
/// </summary>
public class ConfiguracionEmpresaRepository : SupabaseRepository, IConfiguracionEmpresaRepository
{
    private new readonly ILogger<ConfiguracionEmpresaRepository> _logger;

    public ConfiguracionEmpresaRepository(Supabase.Client supabaseClient, ILogger<ConfiguracionEmpresaRepository> logger, SupabaseConfig config) 
        : base(supabaseClient, logger, config)
    {
        _logger = logger;
    }

    #region Implementación requerida por interfaces legacy

    public Task<ConfiguracionEmpresa> GetItem(CEFilter filter, CEItemType filterType)
    {
        // Esta implementación no usa los filtros legacy; se mantiene por compatibilidad.
        throw new NotSupportedException("Operación legacy no soportada en repositorio basado en stored procedures.");
    }

    public Task<IEnumerable<ConfiguracionEmpresa>> GetLstItem(CEFilter filter, CEListType filterType, Utils.Pagination pagination)
    {
        // No se soporta listado legacy en esta implementación basada en stored procedures.
        return Task.FromResult<IEnumerable<ConfiguracionEmpresa>>(Enumerable.Empty<ConfiguracionEmpresa>());
    }

    public Task<int> Insert(ConfiguracionEmpresa item)
    {
        // Operaciones CRUD legacy no están soportadas directamente.
        throw new NotSupportedException("Use los endpoints CreateAsync/UpdateAsync basados en DTO.");
    }

    public Task<bool> Update(ConfiguracionEmpresa item)
    {
        // Operaciones CRUD legacy no están soportadas directamente.
        throw new NotSupportedException("Use los endpoints UpdateAsync basados en DTO.");
    }

    public Task<bool> DeleteEntero(int id)
    {
        // Operaciones CRUD legacy no están soportadas directamente.
        throw new NotSupportedException("Eliminación legacy no soportada. Use DeleteAsync(id).");
    }

    // Implementaciones públicas ya cumplen la interfaz para GetItem y GetLstItem

    Task<int> IInsertIntRepository<ConfiguracionEmpresa>.Insert(ConfiguracionEmpresa item)
    {
        throw new NotSupportedException("Operación legacy Insert no soportada. Use CreateAsync con DTO.");
    }

    Task<bool> IUpdateRepository<ConfiguracionEmpresa>.Update(ConfiguracionEmpresa item)
    {
        throw new NotSupportedException("Operación legacy Update no soportada. Use UpdateAsync con DTO.");
    }

    Task<bool> IDeleteIntRepository.DeleteEntero(int id)
    {
        // Redirigimos a la implementación moderna si corresponde
        return DeleteAsync(id);
    }

    Task<ConfiguracionEmpresa> IConfiguracionEmpresaRepository.CreateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa)
    {
        // No hay un mapeo 1:1 entre el modelo complejo y los stored procedures actuales basados en clave/valor.
        throw new NotSupportedException("CreateConfiguracionEmpresaAsync (modelo complejo) no soportado por stored procedures actuales.");
    }

    Task<bool> IConfiguracionEmpresaRepository.UpdateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa)
    {
        // No hay un mapeo 1:1 entre el modelo complejo y los stored procedures actuales basados en clave/valor.
        throw new NotSupportedException("UpdateConfiguracionEmpresaAsync (modelo complejo) no soportado por stored procedures actuales.");
    }

    Task<bool> IConfiguracionEmpresaRepository.DeleteConfiguracionEmpresaAsync(string configuracionId)
    {
        if (int.TryParse(configuracionId, out var idInt))
        {
            return DeleteAsync(idInt);
        }

        throw new ArgumentException("configuracionId debe ser convertible a entero.", nameof(configuracionId));
    }

    Task<bool> IConfiguracionEmpresaRepository.ConfiguracionEmpresaExistsAsync(string configuracionId)
    {
        if (int.TryParse(configuracionId, out var idInt))
        {
            return ExistsAsync(idInt);
        }

        throw new ArgumentException("configuracionId debe ser convertible a entero.", nameof(configuracionId));
    }

    #endregion

    /// <summary>
    /// Obtiene todas las configuraciones de empresa
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetAllAsync()
    {
        var res = await _supabaseClient.From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>().Get();
        var models = res.Models ?? new List<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>();
        return models.Select(m => new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = m.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = m.nConfiguracionEmpresaEmpresaId,
            nAplicacionesId = 0,
            cConfiguracionEmpresaClave = m.cConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = m.cConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = "text",
            cConfiguracionEmpresaDescripcion = string.Empty,
            dConfiguracionEmpresaFechaCreacion = m.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = m.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = true
        }).ToList();
    }

    /// <summary>
    /// Obtiene una configuración por ID
    /// </summary>
    public async Task<ConfiguracionEmpresaDto?> GetByIdAsync(int id)
    {
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, id)
            .Get();
        var m = res.Models?.FirstOrDefault();
        return m == null ? null : new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = m.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = m.nConfiguracionEmpresaEmpresaId,
            nAplicacionesId = 0,
            cConfiguracionEmpresaClave = m.cConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = m.cConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = "text",
            cConfiguracionEmpresaDescripcion = string.Empty,
            dConfiguracionEmpresaFechaCreacion = m.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = m.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = true
        };
    }

    /// <summary>
    /// Obtiene configuraciones por empresa
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAsync(int empresaId)
    {
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, empresaId)
            .Get();
        var models = res.Models ?? new List<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>();
        return models.Select(m => new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = m.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = m.nConfiguracionEmpresaEmpresaId,
            nAplicacionesId = 0,
            cConfiguracionEmpresaClave = m.cConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = m.cConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = "text",
            cConfiguracionEmpresaDescripcion = string.Empty,
            dConfiguracionEmpresaFechaCreacion = m.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = m.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = true
        }).ToList();
    }

    /// <summary>
    /// Obtiene configuraciones por aplicación
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetByAplicacionAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por aplicación: {AplicacionId}", aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetByAplicacion", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones para la aplicación: {AplicacionId}", result.lstItem.Count, aplicacionId);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones para la aplicación: {AplicacionId}", aplicacionId);
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por aplicación: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuraciones por empresa y aplicación
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAndAplicacionAsync(int empresaId, int aplicacionId)
    {
        // Unificar: devolver configuración efectiva (aplicación + overrides de empresa)
        _logger.LogInformation("Obteniendo configuración efectiva para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

        // Leer configuración base de la aplicación
        var appRes = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionAplicacionSupabase>()
            .Filter("nAplicacionesId", Supabase.Postgrest.Constants.Operator.Equals, aplicacionId)
            .Limit(1)
            .Get();
        var appCfg = appRes.Models?.FirstOrDefault();

        // Leer overrides de empresa
        var empRes = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, empresaId)
            .Get();
        var empCfg = empRes.Models ?? new List<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>();
        var empDict = empCfg.ToDictionary(k => k.cConfiguracionEmpresaClave, v => v);

        var resultado = new List<ConfiguracionEmpresaDto>();
        void Add(string clave, object? valorApp)
        {
            var tieneOverride = empDict.TryGetValue(clave, out var ov);
            var val = tieneOverride ? ov!.cConfiguracionEmpresaValor : valorApp?.ToString() ?? string.Empty;
            resultado.Add(new ConfiguracionEmpresaDto
            {
                nConfiguracionEmpresaId = tieneOverride ? (ov!.nConfiguracionEmpresaId ?? 0) : 0,
                nEmpresasId = empresaId,
                nAplicacionesId = aplicacionId,
                cConfiguracionEmpresaClave = clave,
                cConfiguracionEmpresaValor = val,
                cConfiguracionEmpresaTipo = "text",
                cConfiguracionEmpresaDescripcion = string.Empty,
                dConfiguracionEmpresaFechaCreacion = tieneOverride ? (ov!.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow) : System.DateTime.UtcNow,
                dConfiguracionEmpresaFechaActualizacion = tieneOverride ? (ov!.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow) : System.DateTime.UtcNow,
                bConfiguracionEmpresaEsActiva = true
            });
        }

        if (appCfg != null)
        {
            Add("nMaxTamanoArchivo", appCfg.nMaxTamanoArchivo);
            Add("cTiposArchivosPermitidos", appCfg.cTiposArchivosPermitidos);
            Add("bPermitirAdjuntos", appCfg.bPermitirAdjuntos);
            Add("nMaxCantidadAdjuntos", appCfg.nMaxCantidadAdjuntos);
            Add("bPermitirVisualizacionAdjuntos", appCfg.bPermitirVisualizacionAdjuntos);
            Add("nMaxLongitudMensaje", appCfg.nMaxLongitudMensaje);
            Add("bPermitirEmojis", appCfg.bPermitirEmojis);
            Add("bPermitirMensajesVoz", appCfg.bPermitirMensajesVoz);
            Add("bPermitirNotificaciones", appCfg.bPermitirNotificaciones);
            Add("bRequiereAutenticacion", appCfg.bRequiereAutenticacion);
            Add("bPermitirMensajesAnonimos", appCfg.bPermitirMensajesAnonimos);
            Add("nTiempoExpiracionSesion", appCfg.nTiempoExpiracionSesion);
        }

        return resultado;
    }

    /// <summary>
    /// Obtiene una configuración específica por clave, empresa y aplicación
    /// </summary>
    public async Task<ConfiguracionEmpresaDto?> GetByClaveAsync(string clave, int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pClave", clave },
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetByClave", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                _logger.LogInformation("Configuración encontrada por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
                return (ConfiguracionEmpresaDto)result.lstItem.First();
            }
            
            _logger.LogWarning("Configuración no encontrada por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuraciones activas
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetActivasAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones activas");

            var parameters = new Dictionary<string, object>();
            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetActivas", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones activas", result.lstItem.Count);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones activas");
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones activas");
            throw;
        }
    }

    /// <summary>
    /// Busca configuraciones por término
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> SearchAsync(string searchTerm)
    {
        try
        {
            _logger.LogInformation("Buscando configuraciones con término: {SearchTerm}", searchTerm);

            var parameters = new Dictionary<string, object>
            {
                { "pTerminoBusqueda", searchTerm ?? string.Empty }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_Search", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones con el término: {SearchTerm}", result.lstItem.Count, searchTerm);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones con el término: {SearchTerm}", searchTerm);
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar configuraciones con término: {SearchTerm}", searchTerm);
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuraciones agrupadas por empresa y aplicación
    /// </summary>
    public async Task<List<ConfiguracionEmpresaAgrupadaDto>> GetAgrupadasAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones agrupadas");

            var parameters = new Dictionary<string, object>();
            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaAgrupadaDto>("USP_ConfiguracionEmpresa_GetAgrupadas", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} grupos de configuraciones", result.lstItem.Count);
                return result.lstItem.Cast<ConfiguracionEmpresaAgrupadaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones agrupadas");
            return new List<ConfiguracionEmpresaAgrupadaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones agrupadas");
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuraciones heredadas de aplicación para una empresa
    /// </summary>
    public async Task<List<ConfiguracionHeredadaDto>> GetConfiguracionesHeredadasAsync(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones heredadas para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionHeredadaDto>("USP_ConfiguracionEmpresa_GetHeredadas", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones heredadas", result.lstItem.Count);
                return result.lstItem.Cast<ConfiguracionHeredadaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones heredadas");
            return new List<ConfiguracionHeredadaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones heredadas");
            throw;
        }
    }

    /// <summary>
    /// Crea una nueva configuración de empresa
    /// </summary>
    public async Task<ConfiguracionEmpresaDto> CreateAsync(CreateConfiguracionEmpresaDto createDto)
    {
        var model = new ChatModularMicroservice.Domain.ConfiguracionEmpresaInsertSupabase
        {
            nConfiguracionEmpresaEmpresaId = createDto.NEmpresasId,
            cConfiguracionEmpresaClave = createDto.CConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = createDto.CConfiguracionEmpresaValor,
            dConfiguracionEmpresaFechaCreacion = System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = System.DateTime.UtcNow
        };
        await _supabaseClient.From<ChatModularMicroservice.Domain.ConfiguracionEmpresaInsertSupabase>().Insert(model);
        var fetch = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, createDto.NEmpresasId)
            .Filter("cConfiguracionEmpresaClave", Supabase.Postgrest.Constants.Operator.Equals, createDto.CConfiguracionEmpresaClave)
            .Limit(1)
            .Get();
        var m = fetch.Models?.FirstOrDefault();
        return new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = m?.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = m?.nConfiguracionEmpresaEmpresaId ?? createDto.NEmpresasId,
            nAplicacionesId = createDto.NAplicacionesId,
            cConfiguracionEmpresaClave = m?.cConfiguracionEmpresaClave ?? createDto.CConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = m?.cConfiguracionEmpresaValor ?? createDto.CConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = createDto.CConfiguracionEmpresaTipo,
            cConfiguracionEmpresaDescripcion = createDto.CConfiguracionEmpresaDescripcion ?? string.Empty,
            dConfiguracionEmpresaFechaCreacion = m?.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = m?.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = createDto.BConfiguracionEmpresaEsActiva
        };
    }

    /// <summary>
    /// Actualiza una configuración existente
    /// </summary>
    public async Task<ConfiguracionEmpresaDto> UpdateAsync(int id, UpdateConfiguracionEmpresaDto updateDto)
    {
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, id)
            .Get();
        var m = res.Models?.FirstOrDefault();
        if (m == null) throw new InvalidOperationException("Configuración no encontrada");
        m.cConfiguracionEmpresaValor = updateDto.CConfiguracionEmpresaValor ?? m.cConfiguracionEmpresaValor;
        m.dConfiguracionEmpresaFechaActualizacion = System.DateTime.UtcNow;
        var upd = await _supabaseClient.From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>().Update(m);
        var u = upd.Models?.FirstOrDefault() ?? m;
        return new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = u.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = u.nConfiguracionEmpresaEmpresaId,
            nAplicacionesId = 0,
            cConfiguracionEmpresaClave = u.cConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = u.cConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = updateDto.CConfiguracionEmpresaTipo ?? "text",
            cConfiguracionEmpresaDescripcion = updateDto.CConfiguracionEmpresaDescripcion ?? string.Empty,
            dConfiguracionEmpresaFechaCreacion = u.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = u.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = updateDto.BConfiguracionEmpresaEsActiva ?? true
        };
    }

    /// <summary>
    /// Elimina una configuración
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, id)
            .Delete();
        return true;
    }

    /// <summary>
    /// Verifica si existe una configuración con la clave especificada para una empresa y aplicación
    /// </summary>
    public async Task<bool> ExistsByClaveAsync(string clave, int empresaId, int aplicacionId)
    {
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, empresaId)
            .Filter("cConfiguracionEmpresaClave", Supabase.Postgrest.Constants.Operator.Equals, clave)
            .Get();
        return (res.Models?.Any() ?? false);
    }

    /// <summary>
    /// Verifica si existe una configuración con el ID especificado
    /// </summary>
    public async Task<bool> ExistsAsync(int id)
    {
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, id)
            .Get();
        return (res.Models?.Any() ?? false);
    }

    /// <summary>
    /// Copia configuraciones de aplicación a empresa
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> CopiarConfiguracionesDeAplicacionAsync(int empresaId, int aplicacionId)
    {
        _logger.LogInformation("Copiando configuraciones de aplicación {AplicacionId} a empresa {EmpresaId}", aplicacionId, empresaId);

        // Leer configuración de aplicación
        var res = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionAplicacionSupabase>()
            .Filter("nAplicacionesId", Supabase.Postgrest.Constants.Operator.Equals, aplicacionId)
            .Limit(1)
            .Get();
        var appCfg = res.Models?.FirstOrDefault();
        if (appCfg == null)
        {
            _logger.LogWarning("No hay configuración de aplicación para {AplicacionId}", aplicacionId);
            return new List<ConfiguracionEmpresaDto>();
        }

        // Mapear columnas conocidas a clave/valor para empresa (modelo de inserción sin PK)
        var kv = new List<ChatModularMicroservice.Domain.ConfiguracionEmpresaInsertSupabase>();
        void AddKV(string clave, object? valor)
        {
            if (valor == null) return;
            kv.Add(new ChatModularMicroservice.Domain.ConfiguracionEmpresaInsertSupabase
            {
                nConfiguracionEmpresaEmpresaId = empresaId,
                cConfiguracionEmpresaClave = clave,
                cConfiguracionEmpresaValor = valor.ToString(),
                dConfiguracionEmpresaFechaCreacion = System.DateTime.UtcNow,
                dConfiguracionEmpresaFechaActualizacion = System.DateTime.UtcNow
            });
        }

        AddKV("nMaxTamanoArchivo", appCfg.nMaxTamanoArchivo);
        AddKV("cTiposArchivosPermitidos", appCfg.cTiposArchivosPermitidos);
        AddKV("bPermitirAdjuntos", appCfg.bPermitirAdjuntos);
        AddKV("nMaxCantidadAdjuntos", appCfg.nMaxCantidadAdjuntos);
        AddKV("bPermitirVisualizacionAdjuntos", appCfg.bPermitirVisualizacionAdjuntos);
        AddKV("nMaxLongitudMensaje", appCfg.nMaxLongitudMensaje);
        AddKV("bPermitirEmojis", appCfg.bPermitirEmojis);
        AddKV("bPermitirMensajesVoz", appCfg.bPermitirMensajesVoz);
        AddKV("bPermitirNotificaciones", appCfg.bPermitirNotificaciones);
        AddKV("bRequiereAutenticacion", appCfg.bRequiereAutenticacion);
        AddKV("bPermitirMensajesAnonimos", appCfg.bPermitirMensajesAnonimos);
        AddKV("nTiempoExpiracionSesion", appCfg.nTiempoExpiracionSesion);

        if (kv.Count == 0)
        {
            _logger.LogWarning("No hay valores para copiar desde ConfiguracionAplicacion {AplicacionId}", aplicacionId);
            return new List<ConfiguracionEmpresaDto>();
        }

        await _supabaseClient.From<ChatModularMicroservice.Domain.ConfiguracionEmpresaInsertSupabase>().Insert(kv);
        var fetchInserted = await _supabaseClient
            .From<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>()
            .Filter("nConfiguracionEmpresaEmpresaId", Supabase.Postgrest.Constants.Operator.Equals, empresaId)
            .Get();
        var insertedModels = fetchInserted.Models ?? new List<ChatModularMicroservice.Domain.ConfiguracionEmpresaSupabase>();

        return insertedModels.Select(m => new ConfiguracionEmpresaDto
        {
            nConfiguracionEmpresaId = m.nConfiguracionEmpresaId ?? 0,
            nEmpresasId = m.nConfiguracionEmpresaEmpresaId,
            nAplicacionesId = aplicacionId,
            cConfiguracionEmpresaClave = m.cConfiguracionEmpresaClave,
            cConfiguracionEmpresaValor = m.cConfiguracionEmpresaValor ?? string.Empty,
            cConfiguracionEmpresaTipo = "text",
            cConfiguracionEmpresaDescripcion = string.Empty,
            dConfiguracionEmpresaFechaCreacion = m.dConfiguracionEmpresaFechaCreacion ?? System.DateTime.UtcNow,
            dConfiguracionEmpresaFechaActualizacion = m.dConfiguracionEmpresaFechaActualizacion ?? System.DateTime.UtcNow,
            bConfiguracionEmpresaEsActiva = true
        }).ToList();
    }

    /// <summary>
    /// Restaura configuraciones de empresa a los valores por defecto de la aplicación
    /// </summary>
    public async Task<bool> RestaurarConfiguracionesPorDefectoAsync(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Restaurando configuraciones por defecto para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<dynamic>("USP_ConfiguracionEmpresa_RestaurarPorDefecto", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                dynamic resultData = result.lstItem.First();
                bool success = Convert.ToBoolean(resultData.success);
                _logger.LogInformation("Configuraciones restauradas por defecto: {Success}", success);
                return success;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restaurar configuraciones por defecto para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            throw;
        }
    }
}
