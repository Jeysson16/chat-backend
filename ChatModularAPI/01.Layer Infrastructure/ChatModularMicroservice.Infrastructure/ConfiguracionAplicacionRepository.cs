using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using ChatModularMicroservice.Infrastructure.Repositories;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace ChatModularMicroservice.Infrastructure.Repositories;

/// <summary>
/// Repositorio para la gestión de configuraciones de aplicaciones usando stored procedures
/// </summary>
public class ConfiguracionAplicacionRepository : SupabaseRepository, IConfiguracionAplicacionUnificadaRepository
{
    private new readonly ILogger<ConfiguracionAplicacionRepository> _logger;

    public ConfiguracionAplicacionRepository(Supabase.Client supabaseClient, ILogger<ConfiguracionAplicacionRepository> logger, SupabaseConfig config) 
        : base(supabaseClient, logger, config)
    {
        _logger = logger;
    }

    /// <summary>
    /// Obtiene la configuración unificada de una aplicación por código usando Supabase client
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto?> ObtenerUnificadaPorCodigoAplicacionAsync(string codigoAplicacion)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración unificada para aplicación: {CodigoAplicacion}", codigoAplicacion);

            // First, get the application ID from the Aplicaciones table
            var appQuery = _supabaseClient
                .From<ChatModularMicroservice.Domain.Aplicacion>()
                .Select("nAplicacionesId")
                .Where(x => x.cAplicacionesCodigo == codigoAplicacion)
                .Where(x => x.bAplicacionesEsActiva == true);

            var appResult = await appQuery.Single();
            
            if (appResult == null)
            {
                _logger.LogWarning("No se encontró aplicación con código: {CodigoAplicacion}", codigoAplicacion);
                return null;
            }

            var aplicacionId = appResult.nAplicacionesId;
            _logger.LogInformation("Aplicación encontrada con ID: {AplicacionId}", aplicacionId);

            // Now get the unified configuration for this application
            var configQuery = _supabaseClient
                .From<Entities.Models.ConfiguracionAplicacionUnificada>()
                .Where(x => x.NAplicacionesId == aplicacionId)
                .Where(x => x.BEsActiva == true);

            var configResult = await configQuery.Single();
            
            if (configResult == null)
            {
                _logger.LogWarning("No se encontró configuración unificada para la aplicación con código: {CodigoAplicacion}", codigoAplicacion);
                return null;
            }

            _logger.LogInformation("Configuración unificada encontrada para aplicación: {CodigoAplicacion}", codigoAplicacion);

            // Map the entity to Unificada DTO
            return MapEntityToUnificadaDto(configResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración unificada para aplicación: {CodigoAplicacion}", codigoAplicacion);
            throw;
        }
    }

    /// <summary>
    /// Obtiene la configuración completa de una aplicación por código usando Supabase client
    /// </summary>
    public async Task<ConfiguracionAplicacionDto?> ObtenerPorCodigoAplicacionAsync(string codigoAplicacion)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);

            // First, get the application ID from the Aplicaciones table
            _logger.LogInformation("Intentando obtener aplicación con código exacto: {CodigoAplicacion}", codigoAplicacion);
            
            // Hacer un query más simple para verificar conexión y datos
            var appQuery = _supabaseClient
                .From<ChatModularMicroservice.Domain.Aplicacion>()
                .Select("*");

            var allAppsResult = await appQuery.Get();
            _logger.LogInformation("Total de aplicaciones encontradas: {Total}", allAppsResult?.Models?.Count ?? 0);
            
            if (allAppsResult?.Models != null)
            {
                foreach (var app in allAppsResult.Models)
                {
                    _logger.LogInformation("Aplicación encontrada - ID: {Id}, Código: {Codigo}, Activa: {Activa}", 
                        app.nAplicacionesId, app.cAplicacionesCodigo, app.bAplicacionesEsActiva);
                }
            }

            // Ahora hacer el query específico
            var specificQuery = _supabaseClient
                .From<ChatModularMicroservice.Domain.Aplicacion>()
                .Select("nAplicacionesId")
                .Where(x => x.cAplicacionesCodigo == codigoAplicacion)
                .Where(x => x.bAplicacionesEsActiva == true);

            var appResult = await specificQuery.Get();
            
            _logger.LogInformation("Resultado de aplicación: {Resultado}", appResult != null ? $"Encontrado {appResult.Models?.Count ?? 0} registros" : "null");
            
            if (appResult?.Models?.FirstOrDefault() == null)
            {
                _logger.LogWarning("No se encontró aplicación con código: {CodigoAplicacion}", codigoAplicacion);
                return null;
            }

            var aplicacionId = appResult.Models.First().nAplicacionesId;
            _logger.LogInformation("Aplicación encontrada con ID: {AplicacionId}", aplicacionId);

            // Now get the configuration for this application
            var configQuery = _supabaseClient
                .From<Entities.Models.ConfiguracionAplicacion>()
                .Where(x => x.nAplicacionesId == aplicacionId)
                .Where(x => x.bEsActiva == true);

            _logger.LogInformation("Ejecutando query para obtener configuración con aplicacionId: {AplicacionId}", aplicacionId);
            var configResult = await configQuery.Get();
            
            _logger.LogInformation("Resultado de configuración: {Resultado}", configResult != null ? $"Encontrado {configResult.Models?.Count ?? 0} registros" : "null");
            
            if (configResult?.Models?.FirstOrDefault() == null)
            {
                _logger.LogWarning("No se encontró configuración para la aplicación con código: {CodigoAplicacion}", codigoAplicacion);
                return null;
            }

            var config = configResult.Models.First();
            _logger.LogInformation("Configuración encontrada para aplicación: {CodigoAplicacion}", codigoAplicacion);

            // Map the entity to DTO
            return MapEntityToDto(config, codigoAplicacion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);
            throw;
        }
    }

    /// <summary>
    /// Obtiene la configuración completa de una aplicación (método legacy)
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificada?> ObtenerPorAplicacionIdAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación ID: {AplicacionId}", aplicacionId);

            // Primero obtener el código de aplicación
            var appResult = await ExecuteStoredProcedureListAsync<dynamic>(
                "SELECT c_aplicaciones_codigo FROM Aplicaciones WHERE n_aplicaciones_id = @aplicacionId", 
                new { aplicacionId }, 
                "ConfiguracionAplicacionRepository"
            );

            if (appResult?.lstItem?.Any() != true)
            {
                _logger.LogWarning("No se encontró aplicación con ID: {AplicacionId}", aplicacionId);
                return null;
            }

            var firstApp = appResult.lstItem.FirstOrDefault();
                string? codigoApp = null;
                if (firstApp != null)
                {
                    dynamic d = firstApp;
                    codigoApp = d.c_aplicaciones_codigo?.ToString();
                }
            if (string.IsNullOrEmpty(codigoApp))
            {
                _logger.LogWarning("No se pudo obtener el código de la aplicación con ID: {AplicacionId}", aplicacionId);
                return null;
            }

            // Ahora obtener la configuración usando el código
            var config = await ObtenerPorCodigoAplicacionAsync(codigoApp);
            return config != null ? MapDtoToEntity(config) : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación ID: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Crea o actualiza una configuración (upsert)
    /// </summary>
    public async Task<ConfiguracionAplicacionDto> UpsertAsync(string codigoAplicacion, ConfiguracionAplicacionDto configuracion)
    {
        try
        {
            _logger.LogInformation("Creando/actualizando configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);

            // Primero obtener el ID de la aplicación desde Aplicaciones
            var appQuery = @"
                SELECT n_aplicaciones_id 
                FROM Aplicaciones 
                WHERE c_aplicaciones_codigo = @codigoAplicacion 
                LIMIT 1";

            var appParams = new { codigoAplicacion };
            var appResult = await ExecuteStoredProcedureListAsync<dynamic>(
                appQuery, 
                appParams, 
                "ConfiguracionAplicacionRepository"
            );

            if (appResult?.lstItem?.Any() != true)
            {
                throw new InvalidOperationException($"No se encontró aplicación con código: {codigoAplicacion}");
            }

            var aplicacionId = (int)((dynamic)appResult.lstItem.First()).n_aplicaciones_id;

            // Verificar si existe configuración
            var existeQuery = @"
                SELECT nConfiguracionAplicacionId 
                FROM ConfiguracionAplicacion 
                WHERE nAplicacionesId = @aplicacionId 
                LIMIT 1";

            var existeParams = new { aplicacionId };
            var existeResult = await ExecuteStoredProcedureListAsync<dynamic>(
                existeQuery, 
                existeParams, 
                "ConfiguracionAplicacionRepository"
            );

            bool existeConfiguracion = existeResult?.lstItem?.Any() == true;

            if (existeConfiguracion)
            {
                // Actualizar configuración existente
                var updateQuery = @"
                    UPDATE ConfiguracionAplicacion 
                    SET nMaxTamanoArchivo = @nMaxTamanoArchivo,
                        cTiposArchivosPermitidos = @cTiposArchivosPermitidos,
                        bPermitirAdjuntos = @bPermitirAdjuntos,
                        nMaxCantidadAdjuntos = @nMaxCantidadAdjuntos,
                        nMaxLongitudMensaje = @nMaxLongitudMensaje,
                        bPermitirEmojis = @bPermitirEmojis,
                        bPermitirNotificaciones = @bPermitirNotificaciones,
                        bRequiereAutenticacion = @bRequiereAutenticacion,
                        nTiempoExpiracionSesion = @nTiempoExpiracionSesion,
                        dFechaActualizacion = NOW()
                    WHERE nAplicacionesId = @nAplicacionesId
                    RETURNING nConfiguracionAplicacionId";

                var updateParams = new
                {
                    nAplicacionesId = aplicacionId,
                    configuracion.NMaxTamanoArchivo,
                    configuracion.CTiposArchivosPermitidos,
                    configuracion.BPermitirAdjuntos,
                    configuracion.NMaxCantidadAdjuntos,
                    configuracion.NMaxLongitudMensaje,
                    configuracion.BPermitirEmojis,
                    configuracion.BPermitirNotificaciones,
                    configuracion.BRequiereAutenticacion,
                    configuracion.NTiempoExpiracionSesion
                };

                var updateResult = await ExecuteStoredProcedureListAsync<dynamic>(
                    updateQuery, 
                    updateParams, 
                    "ConfiguracionAplicacionRepository"
                );

                if (updateResult?.lstItem?.Any() != true)
                {
                    throw new InvalidOperationException("Error al actualizar la configuración");
                }
            }
            else
            {
                // Insertar nueva configuración
                var insertQuery = @"
                    INSERT INTO ConfiguracionAplicacion (
                        nAplicacionesId, nMaxTamanoArchivo, cTiposArchivosPermitidos, 
                        bPermitirAdjuntos, nMaxCantidadAdjuntos, nMaxLongitudMensaje, 
                        bPermitirEmojis, bPermitirNotificaciones, bRequiereAutenticacion, 
                        nTiempoExpiracionSesion, bEsActiva, dFechaCreacion, dFechaActualizacion
                    ) VALUES (
                        @nAplicacionesId, @nMaxTamanoArchivo, @cTiposArchivosPermitidos,
                        @bPermitirAdjuntos, @nMaxCantidadAdjuntos, @nMaxLongitudMensaje,
                        @bPermitirEmojis, @bPermitirNotificaciones, @bRequiereAutenticacion,
                        @nTiempoExpiracionSesion, true, NOW(), NOW()
                    )
                    RETURNING nConfiguracionAplicacionId";

                var insertParams = new
                {
                    nAplicacionesId = aplicacionId,
                    configuracion.NMaxTamanoArchivo,
                    configuracion.CTiposArchivosPermitidos,
                    configuracion.BPermitirAdjuntos,
                    configuracion.NMaxCantidadAdjuntos,
                    configuracion.NMaxLongitudMensaje,
                    configuracion.BPermitirEmojis,
                    configuracion.BPermitirNotificaciones,
                    configuracion.BRequiereAutenticacion,
                    configuracion.NTiempoExpiracionSesion
                };

                var insertResult = await ExecuteStoredProcedureListAsync<dynamic>(
                    insertQuery, 
                    insertParams, 
                    "ConfiguracionAplicacionRepository"
                );

                if (insertResult?.lstItem?.Any() != true)
                {
                    throw new InvalidOperationException("Error al insertar la configuración");
                }
            }

            // Retornar la configuración actualizada
            return await ObtenerPorCodigoAplicacionAsync(codigoAplicacion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear/actualizar configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuración específica de adjuntos
    /// </summary>
    public async Task<ConfiguracionAplicacionDto?> ObtenerConfiguracionAdjuntosAsync(string codigoAplicacion)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración de adjuntos para aplicación: {CodigoAplicacion}", codigoAplicacion);

            var parameters = new Dictionary<string, object>
            {
                { "p_app_codigo", codigoAplicacion }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionAplicacionDto>(
                "sp_configuracion_adjuntos_obtener", 
                parameters, 
                "ConfiguracionAplicacionRepository"
            );

            if (result?.lstItem?.Any() == true)
            {
                return result.lstItem.OfType<ConfiguracionAplicacionDto>().First();
            }

            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de adjuntos para aplicación: {CodigoAplicacion}", codigoAplicacion);
            throw;
        }
    }

    /// <summary>
    /// Lista todas las configuraciones activas
    /// </summary>
    public async Task<List<ConfiguracionAplicacionDto>> ListarConfiguracionesActivasAsync()
    {
        try
        {
            _logger.LogInformation("Listando todas las configuraciones activas");

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionAplicacionDto>(
                "sp_configuracion_aplicacion_listar", 
                new Dictionary<string, object>(), 
                "ConfiguracionAplicacionRepository"
            );

            if (result?.lstItem?.Any() == true)
            {
                return result.lstItem.OfType<ConfiguracionAplicacionDto>().ToList();
            }

            return new List<ConfiguracionAplicacionDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar configuraciones activas");
            throw;
        }
    }

    /// <summary>
    /// Verifica si existe una configuración para una aplicación
    /// </summary>
    public async Task<bool> ExistePorCodigoAplicacionAsync(string codigoAplicacion)
    {
        try
        {
            var configuracion = await ObtenerPorCodigoAplicacionAsync(codigoAplicacion);
            return configuracion != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);
            return false;
        }
    }

    /// <summary>
    /// Verifica si existe una configuración para una aplicación (método legacy)
    /// </summary>
    public async Task<bool> ExistePorAplicacionIdAsync(int aplicacionId)
    {
        try
        {
            var configuracion = await ObtenerPorAplicacionIdAsync(aplicacionId);
            return configuracion != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración para aplicación: {AplicacionId}", aplicacionId);
            return false;
        }
    }

    #region Implementación genérica requerida por interfaces

    public Task<ConfiguracionAplicacionUnificada> GetItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterItemType filterType)
    {
        // Esta implementación no usa los filtros legacy; se mantiene por compatibilidad.
        throw new NotSupportedException("Use ObtenerPorCodigoAplicacionAsync u ObtenerPorAplicacionIdAsync.");
    }

    public Task<IEnumerable<ConfiguracionAplicacionUnificada>> GetLstItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterListType filterType, Utils.Pagination pagination)
    {
        // No se soporta listado legacy en esta implementación basada en stored procedures.
        return Task.FromResult<IEnumerable<ConfiguracionAplicacionUnificada>>(Enumerable.Empty<ConfiguracionAplicacionUnificada>());
    }

    public Task<int> Insert(ConfiguracionAplicacionUnificada item)
    {
        // Operaciones CRUD legacy no están soportadas en repositorio unificado.
        throw new NotSupportedException("Use UpsertAsync con DTO para crear/actualizar configuración.");
    }

    public Task<bool> Update(ConfiguracionAplicacionUnificada item)
    {
        // Operaciones CRUD legacy no están soportadas en repositorio unificado.
        throw new NotSupportedException("Use UpsertAsync con DTO para actualizar configuración.");
    }

    public Task<bool> DeleteEntero(int id)
    {
        // Operaciones CRUD legacy no están soportadas en repositorio unificado.
        throw new NotSupportedException("Eliminación no soportada. Considere desactivar la configuración.");
    }

    // Implementaciones explícitas para asegurar coincidencia exacta con las interfaces
    Task<ConfiguracionAplicacionUnificada> IConfiguracionAplicacionUnificadaRepository.GetItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterItemType filterType)
    {
        throw new NotSupportedException("Use ObtenerPorCodigoAplicacionAsync u ObtenerPorAplicacionIdAsync.");
    }

    Task<IEnumerable<ConfiguracionAplicacionUnificada>> IConfiguracionAplicacionUnificadaRepository.GetLstItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterListType filterType, Utils.Pagination pagination)
    {
        return Task.FromResult<IEnumerable<ConfiguracionAplicacionUnificada>>(Enumerable.Empty<ConfiguracionAplicacionUnificada>());
    }

    Task<int> IInsertIntRepository<ConfiguracionAplicacionUnificada>.Insert(ConfiguracionAplicacionUnificada item)
    {
        throw new NotSupportedException("Use UpsertAsync con DTO para crear/actualizar configuración.");
    }

    Task<bool> IUpdateRepository<ConfiguracionAplicacionUnificada>.Update(ConfiguracionAplicacionUnificada item)
    {
        throw new NotSupportedException("Use UpsertAsync con DTO para actualizar configuración.");
    }

    Task<bool> IDeleteIntRepository.DeleteEntero(int id)
    {
        throw new NotSupportedException("Eliminación no soportada. Considere desactivar la configuración.");
    }

    // Implementación explícita para resolver discrepancias de tipos en compilación
    Task<ChatModularMicroservice.Entities.Models.ConfiguracionAplicacionUnificada?> ChatModularMicroservice.Repository.IConfiguracionAplicacionUnificadaRepository.ObtenerPorAplicacionIdAsync(int aplicacionId)
    {
        return ObtenerPorAplicacionIdAsync(aplicacionId);
    }

    #endregion

    #region Métodos de Mapeo

    /// <summary>
    /// Mapea de entidad ConfiguracionAplicacion a DTO
    /// </summary>
    private ConfiguracionAplicacionDto MapEntityToDto(Entities.Models.ConfiguracionAplicacion entity, string codigoAplicacion)
    {
        return new ConfiguracionAplicacionDto
        {
            Id = entity.nConfiguracionAplicacionId,
            CAppCodigo = codigoAplicacion,
            NMaxTamanoArchivo = entity.nMaxTamanoArchivo,
            CTiposArchivosPermitidos = entity.cTiposArchivosPermitidos,
            BPermitirAdjuntos = entity.bPermitirAdjuntos,
            NMaxCantidadAdjuntos = entity.nMaxCantidadAdjuntos,
            BPermitirVisualizacionAdjuntos = entity.bPermitirVisualizacionAdjuntos,
            NMaxLongitudMensaje = entity.nMaxLongitudMensaje,
            BPermitirEmojis = entity.bPermitirEmojis,
            BPermitirMensajesVoz = entity.bPermitirMensajesVoz,
            BPermitirNotificaciones = entity.bPermitirNotificaciones,
            BRequiereAutenticacion = entity.bRequiereAutenticacion,
            BPermitirMensajesAnonimos = entity.bPermitirMensajesAnonimos,
            NTiempoExpiracionSesion = entity.nTiempoExpiracionSesion,
            CreatedAt = entity.dFechaCreacion,
            UpdatedAt = entity.dFechaActualizacion,
            BEsActiva = entity.bEsActiva
        };
    }

    /// <summary>
    /// Mapea de DTO a entidad (para compatibilidad con métodos legacy)
    /// </summary>
    private ConfiguracionAplicacionUnificada MapDtoToEntity(ConfiguracionAplicacionDto dto)
    {
        return new ConfiguracionAplicacionUnificada
        {
            // Mapeo básico para compatibilidad
            NMaxTamanoArchivo = dto.NMaxTamanoArchivo,
            CTiposArchivosPermitidos = dto.CTiposArchivosPermitidos,
            BPermitirAdjuntos = dto.BPermitirAdjuntos,
            NMaxLongitudMensaje = dto.NMaxLongitudMensaje,
            BPermitirEmojis = dto.BPermitirEmojis,
            NTiempoExpiracionSesion = dto.NTiempoExpiracionSesion,
            BRequiereAutenticacion = dto.BRequiereAutenticacion,
            DFechaCreacion = dto.CreatedAt,
            DFechaActualizacion = dto.UpdatedAt,
            BEsActiva = dto.BEsActiva
        };
    }

    /// <summary>
    /// Mapea de entidad ConfiguracionAplicacionUnificada a DTO Unificada
    /// </summary>
    private ConfiguracionAplicacionUnificadaDto MapEntityToUnificadaDto(Entities.Models.ConfiguracionAplicacionUnificada entity)
    {
        return new ConfiguracionAplicacionUnificadaDto
        {
            // Configuraciones de Adjuntos
            nAdjuntosMaxTamanoArchivo = entity.NMaxTamanoArchivo ?? 10485760,
            cAdjuntosTiposArchivosPermitidos = entity.CTiposArchivosPermitidos ?? "jpg,jpeg,png,gif,pdf,doc,docx,txt",
            bAdjuntosPermitirAdjuntos = entity.BPermitirAdjuntos ?? true,
            nAdjuntosMaxArchivosSimultaneos = entity.NMaxArchivosSimultaneos ?? 5,
            bAdjuntosPermitirImagenes = entity.BPermitirImagenes ?? true,
            bAdjuntosPermitirDocumentos = entity.BPermitirDocumentos ?? true,
            bAdjuntosPermitirVideos = entity.BPermitirVideos ?? false,
            bAdjuntosPermitirAudio = entity.BPermitirAudio ?? false,
            bAdjuntosRequiereAprobacion = entity.BRequiereAprobacionAdjuntos ?? false,

            // Configuraciones de Chat
            nChatMaxLongitudMensaje = entity.NMaxLongitudMensaje ?? 1000,
            bChatPermitirEmojis = entity.BPermitirEmojis ?? true,
            bChatPermitirMenciones = entity.BPermitirMenciones ?? true,
            bChatPermitirReacciones = entity.BPermitirReacciones ?? true,
            bChatPermitirEdicionMensajes = entity.BPermitirEdicionMensajes ?? true,
            bChatPermitirEliminacionMensajes = entity.BPermitirEliminacionMensajes ?? true,
            nChatTiempoLimiteEdicion = entity.NTiempoLimiteEdicion ?? 300,
            bChatPermitirMensajesPrivados = entity.BPermitirMensajesPrivados ?? true,

            // Configuraciones de Conversaciones
            nConversacionesMaxSimultaneas = entity.NMaxConversacionesSimultaneas ?? 10,
            bConversacionesPermitirChatsGrupales = entity.BPermitirChatsGrupales ?? true,
            nConversacionesMaxParticipantesGrupo = entity.NMaxParticipantesGrupo ?? 50,
            bConversacionesPermitirCrearGrupos = entity.BPermitirCrearGrupos ?? true,
            bConversacionesRequiereAprobacionGrupos = entity.BRequiereAprobacionGrupos ?? false,

            // Configuraciones de Contactos
            bContactosPermitirAgregar = entity.BPermitirAgregarContactos ?? true,
            bContactosRequiereAprobacion = entity.BRequiereAprobacionContactos ?? false,
            bContactosPermitirBusquedaGlobal = entity.BPermitirBusquedaGlobal ?? true,
            bContactosPermitirInvitaciones = entity.BPermitirInvitaciones ?? true,
            nContactosMaxContactos = entity.NMaxContactos ?? 500,

            // Configuraciones de Gestión de Contactos
            cContactosModoGestion = entity.CContactosModoGestion ?? "LOCAL",
            cContactosUrlApiPersonas = string.Empty,
            cContactosTokenApiPersonas = string.Empty,
            bContactosSincronizar = false,
            nContactosTiempoCacheSegundos = 300,
            bContactosHabilitarCache = true,
            nContactosIntervaloSincronizacionMinutos = 60,

            // Configuraciones de Notificaciones
            bNotificacionesEmail = entity.BNotificacionesEmail ?? true,
            bNotificacionesPush = entity.BNotificacionesPush ?? true,
            bNotificacionesEnTiempoReal = entity.BNotificacionesEscritorio ?? true,
            bNotificacionesSonido = entity.BNotificacionesSonido ?? true,
            bNotificacionesVibracion = false,
            cNotificacionesHorarioInicio = "09:00",
            cNotificacionesHorarioFin = "18:00",
            bNotificacionesNoMolestar = false,

            // Configuraciones de Seguridad
            bSeguridadRequiereAutenticacion = entity.BRequiereAutenticacion ?? true,
            bSeguridadEncriptarMensajes = entity.BEncriptarMensajes ?? true,
            nSeguridadTiempoSesionMinutos = entity.NTiempoExpiracionSesion ?? 1440,
            bSeguridadRequiere2FA = entity.BRequiere2FA ?? false,
            bSeguridadPermitirSesionesMultiples = entity.BPermitirSesionesMultiples ?? true,
            bSeguridadLogearActividad = entity.BRegistrarActividad ?? true,
            nSeguridadMaxIntentosLogin = 5,
            nSeguridadTiempoBloqueoMinutos = 15,

            // Configuraciones de Interfaz
            cInterfazTema = entity.BPermitirTemaOscuro == true ? "claro" : "oscuro",
            cInterfazIdioma = entity.CIdiomaDefecto ?? "es",
            bInterfazModoOscuro = entity.BPermitirTemaOscuro ?? false,
            cInterfazColorPrimario = "#007bff",
            cInterfazColorSecundario = "#6c757d",
            cInterfazFuenteTamano = "mediano",
            bInterfazAnimaciones = true,
            bInterfazSonidos = true,

            // Configuraciones de Almacenamiento
            cAlmacenamientoTipo = "local",
            cAlmacenamientoRuta = "/uploads",
            nAlmacenamientoMaxEspacioMB = entity.NCuotaAlmacenamientoMB ?? 1024,
            bAlmacenamientoCompresion = true,
            nAlmacenamientoDiasRetencion = entity.NDiasRetencionMensajes ?? 365,
            bAlmacenamientoLimpiezaAutomatica = entity.BPermitirBackupAutomatico ?? true,

            // Configuraciones de Rendimiento
            nRendimientoMaxConexionesSimultaneas = 100,
            nRendimientoTimeoutConexionSegundos = 30,
            bRendimientoHabilitarCache = true,
            nRendimientoTiempoCacheSegundos = 300,
            bRendimientoCompresionMensajes = true,
            nRendimientoMaxMensajesPorMinuto = 60,

            // Configuraciones de Integración
            bIntegracionHabilitarWebhooks = entity.BPermitirWebhooks ?? false,
            cIntegracionUrlWebhook = string.Empty,
            cIntegracionTokenWebhook = string.Empty,
            bIntegracionHabilitarAPI = entity.BPermitirAPI ?? false,
            cIntegracionVersionAPI = "v1",
            bIntegracionRequiereTokenAPI = true,

            // Configuraciones de Moderación
            bModeracionHabilitarFiltros = false,
            cModeracionPalabrasProhibidas = string.Empty,
            bModeracionAutoModerar = false,
            bModeracionRequiereAprobacion = false,
            nModeracionMaxReportesPorUsuario = 3,
            bModeracionLogearAcciones = true,

            // Configuraciones de Backup
            bBackupHabilitarAutomatico = entity.BPermitirBackupAutomatico ?? true,
            nBackupIntervaloHoras = 24,
            cBackupRutaDestino = "/backups",
            bBackupCompresion = true,
            nBackupDiasRetencion = 30,
            bBackupIncluirAdjuntos = true,

            // Configuraciones de Auditoría
            bAuditoriaHabilitar = entity.BRegistrarActividad ?? true,
            bAuditoriaLogearMensajes = false,
            bAuditoriaLogearConexiones = true,
            bAuditoriaLogearCambiosConfiguracion = true,
            nAuditoriaDiasRetencionLogs = 90,
            cAuditoriaRutaLogs = "/logs",

            // Fechas de control
            dConfiguracionFechaCreacion = entity.DFechaCreacion,
            dConfiguracionFechaModificacion = entity.DFechaActualizacion,
            cConfiguracionCreadoPor = entity.CCreadoPor ?? string.Empty,
            cConfiguracionModificadoPor = entity.CActualizadoPor ?? string.Empty,
            bConfiguracionEstaActiva = entity.BEsActiva
        };
    }

    /// <summary>
    /// Mapea de entidad a DTO
    /// </summary>
    private ConfiguracionAplicacionDto MapEntityToDto(ConfiguracionAplicacionUnificada entity)
    {
        return new ConfiguracionAplicacionDto
        {
            NMaxTamanoArchivo = entity.NMaxTamanoArchivo ?? 0,
            CTiposArchivosPermitidos = entity.CTiposArchivosPermitidos ?? string.Empty,
            BPermitirAdjuntos = entity.BPermitirAdjuntos ?? false,
            NMaxLongitudMensaje = entity.NMaxLongitudMensaje ?? 0,
            BPermitirEmojis = entity.BPermitirEmojis ?? false,
            NTiempoExpiracionSesion = entity.NTiempoExpiracionSesion ?? 0,
            BRequiereAutenticacion = entity.BRequiereAutenticacion ?? false,
            CreatedAt = entity.DFechaCreacion,
            UpdatedAt = entity.DFechaActualizacion,
            BEsActiva = entity.BEsActiva
        };
    }

    #endregion
}
