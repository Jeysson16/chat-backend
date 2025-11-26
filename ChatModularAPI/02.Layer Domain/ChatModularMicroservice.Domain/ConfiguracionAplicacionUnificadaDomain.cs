using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using Microsoft.Extensions.Logging;
using ChatModularMicroservice.Repository;

namespace ChatModularMicroservice.Domain
{

/// <summary>
/// Servicio para la gestión de configuraciones unificadas de aplicaciones
/// </summary>
public class ConfiguracionAplicacionUnificadaService : IConfiguracionAplicacionUnificadaService
{
    private readonly IConfiguracionAplicacionUnificadaRepository _repository;
    private readonly IApplicationRepository _applicationRepository;
    private readonly ILogger<ConfiguracionAplicacionUnificadaService> _logger;

    public ConfiguracionAplicacionUnificadaService(
        IConfiguracionAplicacionUnificadaRepository repository,
        IApplicationRepository applicationRepository,
        ILogger<ConfiguracionAplicacionUnificadaService> logger)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _applicationRepository = applicationRepository ?? throw new ArgumentNullException(nameof(applicationRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Obtiene la configuración completa de una aplicación
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto?> ObtenerConfiguracionAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación: {AplicacionId}", aplicacionId);

            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
            {
                _logger.LogWarning("No se encontró aplicación con ID: {AplicacionId}", aplicacionId);
                return null;
            }

            var configuracion = await _repository.ObtenerPorCodigoAplicacionAsync(aplicacion.Code);
            if (configuracion == null)
            {
                _logger.LogWarning("No se encontró configuración para aplicación: {AplicacionId}", aplicacionId);
                return null;
            }

            return MapDtoToUnificadaDto(configuracion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene la configuración completa de una aplicación por código
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto?> ObtenerConfiguracionPorCodigoAsync(string codigoAplicacion)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);

            var configuracion = await _repository.ObtenerPorCodigoAplicacionAsync(codigoAplicacion);
            if (configuracion == null)
            {
                _logger.LogWarning("No se encontró configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);
                return null;
            }

            return MapDtoToUnificadaDto(configuracion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación: {CodigoAplicacion}", codigoAplicacion);
            throw;
        }
    }

    /// <summary>
    /// Crea una nueva configuración para una aplicación
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto> CrearConfiguracionAsync(CreateConfiguracionAplicacionUnificadaDto createDto)
    {
        try
        {
            _logger.LogInformation("Creando configuración para aplicación: {AplicacionId}", createDto.NAplicacionesId);

            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(createDto.NAplicacionesId);
            if (aplicacion == null)
            {
                throw new InvalidOperationException($"No se encontró aplicación con ID: {createDto.NAplicacionesId}");
            }

            // Verificar si ya existe configuración para esta aplicación
            var existeConfiguracion = await _repository.ExistePorCodigoAplicacionAsync(aplicacion.Code);
            if (existeConfiguracion)
            {
                throw new InvalidOperationException($"Ya existe una configuración para la aplicación {createDto.NAplicacionesId}");
            }

            // Crear configuración usando el nuevo método Upsert
            var configuracionDto = MapCreateDtoToConfiguracionDto(createDto, aplicacion.Code);
            var configuracionCreada = await _repository.UpsertAsync(aplicacion.Code, configuracionDto);

            _logger.LogInformation("Configuración creada exitosamente para aplicación: {AplicacionId}", createDto.NAplicacionesId);
            return MapDtoToUnificadaDto(configuracionCreada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear configuración para aplicación: {AplicacionId}", createDto.NAplicacionesId);
            throw;
        }
    }

    /// <summary>
    /// Actualiza la configuración de una aplicación
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto> ActualizarConfiguracionAsync(int aplicacionId, UpdateConfiguracionAplicacionUnificadaDto updateDto)
    {
        try
        {
            _logger.LogInformation("Actualizando configuración para aplicación: {AplicacionId}", aplicacionId);

            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
            {
                throw new InvalidOperationException($"No se encontró aplicación con ID: {aplicacionId}");
            }

            // Obtener configuración existente
            var configuracionExistente = await _repository.ObtenerPorCodigoAplicacionAsync(aplicacion.Code);
            if (configuracionExistente == null)
            {
                throw new InvalidOperationException($"No se encontró configuración para la aplicación {aplicacionId}");
            }

            // Actualizar configuración usando Upsert
            var configuracionActualizada = MapUpdateDtoToConfiguracionDto(updateDto, configuracionExistente);
            var resultado = await _repository.UpsertAsync(aplicacion.Code, configuracionActualizada);

            _logger.LogInformation("Configuración actualizada exitosamente para aplicación: {AplicacionId}", aplicacionId);
            return MapDtoToUnificadaDto(resultado);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración para aplicación: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Obtiene solo la configuración de adjuntos de una aplicación
    /// </summary>
    public async Task<ConfiguracionAdjuntosDto?> ObtenerConfiguracionAdjuntosAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);

            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
            {
                _logger.LogWarning("No se encontró aplicación con ID: {AplicacionId}", aplicacionId);
                return null;
            }

            var configuracion = await _repository.ObtenerConfiguracionAdjuntosAsync(aplicacion.Code);
            if (configuracion == null)
            {
                return null;
            }

            return new ConfiguracionAdjuntosDto
            {
                NMaxTamanoArchivo = configuracion.NMaxTamanoArchivo,
                CTiposArchivosPermitidos = configuracion.CTiposArchivosPermitidos ?? "jpg,jpeg,png,gif,pdf,doc,docx,txt",
                BPermitirAdjuntos = configuracion.BPermitirAdjuntos,
                NMaxCantidadAdjuntos = configuracion.NMaxCantidadAdjuntos,
                BPermitirImagenes = true, // Derivado de tipos permitidos
                BPermitirDocumentos = true, // Derivado de tipos permitidos
                BPermitirVideos = false, // Derivado de tipos permitidos
                BPermitirAudio = false, // Derivado de tipos permitidos
                BRequiereAprobacionAdjuntos = false // No está en la nueva estructura
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Actualiza solo la configuración de adjuntos de una aplicación
    /// </summary>
    public async Task<ConfiguracionAdjuntosDto> ActualizarConfiguracionAdjuntosAsync(int aplicacionId, ConfiguracionAdjuntosDto adjuntosDto)
    {
        try
        {
            _logger.LogInformation("Actualizando configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);
            
            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
                throw new ArgumentException($"No se encontró la aplicación con ID {aplicacionId}");

            // Obtener configuración existente
            var configuracionExistente = await _repository.ObtenerPorCodigoAplicacionAsync(aplicacion.Code);
            if (configuracionExistente == null)
                throw new ArgumentException($"No se encontró configuración para la aplicación {aplicacion.Code}");

            // Actualizar configuraciones de adjuntos
            configuracionExistente.NMaxTamanoArchivo = adjuntosDto.NMaxTamanoArchivo ?? configuracionExistente.NMaxTamanoArchivo;
            configuracionExistente.CTiposArchivosPermitidos = adjuntosDto.CTiposArchivosPermitidos ?? configuracionExistente.CTiposArchivosPermitidos;
            configuracionExistente.BPermitirAdjuntos = adjuntosDto.BPermitirAdjuntos ?? configuracionExistente.BPermitirAdjuntos;
            configuracionExistente.NMaxCantidadAdjuntos = adjuntosDto.NMaxCantidadAdjuntos ?? configuracionExistente.NMaxCantidadAdjuntos;
            
            configuracionExistente.UpdatedAt = DateTime.UtcNow;

            await _repository.UpsertAsync(aplicacion.Code, configuracionExistente);
            
            _logger.LogInformation("Configuración de adjuntos actualizada para aplicación: {AplicacionId}", aplicacionId);
            
            return adjuntosDto;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);
            throw new Exception($"Error al actualizar configuración de adjuntos: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Verifica si una aplicación tiene configuración
    /// </summary>
    public async Task<bool> ExisteConfiguracionAsync(int aplicacionId)
    {
        try
        {
            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
            {
                return false;
            }

            return await _repository.ExistePorCodigoAplicacionAsync(aplicacion.Code);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración para aplicación: {AplicacionId}", aplicacionId);
            return false;
        }
    }

    /// <summary>
    /// Elimina una configuración de aplicación (desactivación lógica)
    /// </summary>
    public async Task<bool> EliminarConfiguracionAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Eliminando configuración para aplicación: {AplicacionId}", aplicacionId);
            
            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
            {
                _logger.LogWarning("No se encontró la aplicación con ID: {AplicacionId}", aplicacionId);
                return false;
            }

            // Verificar si existe la configuración
            var configuracionExistente = await _repository.ObtenerPorCodigoAplicacionAsync(aplicacion.Code);
            if (configuracionExistente == null)
            {
                _logger.LogWarning("No se encontró configuración para la aplicación: {CodigoAplicacion}", aplicacion.Code);
                return false;
            }

            // Desactivar la configuración
            configuracionExistente.BEsActiva = false;
            configuracionExistente.UpdatedAt = DateTime.UtcNow;

            await _repository.UpsertAsync(aplicacion.Code, configuracionExistente);
            
            _logger.LogInformation("Configuración desactivada para aplicación: {AplicacionId}", aplicacionId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar configuración para aplicación: {AplicacionId}", aplicacionId);
            throw new Exception($"Error al eliminar configuración: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Obtiene todas las configuraciones activas
    /// </summary>
    public async Task<List<ConfiguracionAplicacionUnificadaDto>> ObtenerConfiguracionesActivasAsync()
    {
        try
        {
            _logger.LogInformation("Obteniendo todas las configuraciones activas");

            var configuraciones = await _repository.ListarConfiguracionesActivasAsync();
            return configuraciones.Select(MapDtoToUnificadaDto).ToList();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones activas");
            throw;
        }
    }

    /// <summary>
    /// Restaura la configuración de una aplicación a valores por defecto
    /// </summary>
    public async Task<ConfiguracionAplicacionUnificadaDto> RestaurarConfiguracionPorDefectoAsync(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Restaurando configuración por defecto para aplicación: {AplicacionId}", aplicacionId);

            // Obtener el código de la aplicación
            var aplicacion = await _applicationRepository.GetApplicationByIdAsync(aplicacionId);
            if (aplicacion == null)
                throw new ArgumentException($"No se encontró la aplicación con ID {aplicacionId}");

            // Crear configuración por defecto
            var configuracionDefecto = new ConfiguracionAplicacionDto
            {
                CAppCodigo = aplicacion.Code,
                
                // Configuraciones de adjuntos por defecto
                NMaxTamanoArchivo = 10485760, // 10MB
                CTiposArchivosPermitidos = "jpg,jpeg,png,gif,pdf,doc,docx,txt",
                BPermitirAdjuntos = true,
                NMaxCantidadAdjuntos = 5,
                
                // Configuraciones de chat por defecto
                NMaxLongitudMensaje = 1000,
                BPermitirEmojis = true,
                
                // Configuraciones de notificaciones por defecto
                BPermitirNotificaciones = true,
                
                // Configuraciones de seguridad por defecto
                BRequiereAutenticacion = true,
                NTiempoExpiracionSesion = 1440, // 24 horas
                
                // Metadatos
                BEsActiva = true
            };

            // Usar UpsertAsync para crear o actualizar
            var configuracionCreada = await _repository.UpsertAsync(aplicacion.Code, configuracionDefecto);
            
            _logger.LogInformation("Configuración restaurada por defecto para aplicación: {AplicacionId}", aplicacionId);
            return MapDtoToUnificadaDto(configuracionCreada);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restaurar configuración por defecto para aplicación: {AplicacionId}", aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Valida que los tipos de archivos permitidos sean válidos
    /// </summary>
    public async Task<bool> ValidarTiposArchivosAsync(string tiposArchivos)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(tiposArchivos))
                return false;

            var tiposPermitidos = new HashSet<string>
            {
                "jpg", "jpeg", "png", "gif", "bmp", "webp", "svg",
                "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx",
                "txt", "rtf", "csv", "xml", "json",
                "mp4", "avi", "mov", "wmv", "flv", "webm",
                "mp3", "wav", "ogg", "m4a", "flac",
                "zip", "rar", "7z", "tar", "gz"
            };

            var tipos = tiposArchivos.Split(',', StringSplitOptions.RemoveEmptyEntries)
                                   .Select(t => t.Trim().ToLowerInvariant());

            return tipos.All(tipo => tiposPermitidos.Contains(tipo));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar tipos de archivos: {TiposArchivos}", tiposArchivos);
            return false;
        }
    }

    /// <summary>
    /// Obtiene los tipos de archivos permitidos por defecto
    /// </summary>
    public async Task<string> ObtenerTiposArchivosPorDefectoAsync()
    {
        return await Task.FromResult("jpg,jpeg,png,gif,pdf,doc,docx,txt");
    }

    #region Métodos Privados de Mapeo

    /// <summary>
    /// Mapea de ConfiguracionAplicacionDto a ConfiguracionAplicacionUnificadaDto
    /// </summary>
    private ConfiguracionAplicacionUnificadaDto MapDtoToUnificadaDto(ConfiguracionAplicacionDto dto)
    {
        return new ConfiguracionAplicacionUnificadaDto
        {
            nConfiguracionAplicacionId = 0, // Se asignará automáticamente por la base de datos
            nAplicacionesId = 0, // Se debe obtener por separado si es necesario
            
            // Configuraciones de adjuntos
            nAdjuntosMaxTamanoArchivo = dto.NMaxTamanoArchivo,
            cAdjuntosTiposArchivosPermitidos = dto.CTiposArchivosPermitidos,
            bAdjuntosPermitirAdjuntos = dto.BPermitirAdjuntos,
            nAdjuntosMaxArchivosSimultaneos = dto.NMaxCantidadAdjuntos,
            bAdjuntosPermitirImagenes = true, // Derivado de tipos permitidos
            bAdjuntosPermitirDocumentos = true, // Derivado de tipos permitidos
            bAdjuntosPermitirVideos = false, // Derivado de tipos permitidos
            bAdjuntosPermitirAudio = false, // Derivado de tipos permitidos
            
            // Configuraciones de chat
            nChatMaxLongitudMensaje = dto.NMaxLongitudMensaje,
            bChatPermitirEmojis = dto.BPermitirEmojis,
            bChatPermitirMenciones = true, // Valor por defecto
            bChatPermitirReacciones = true, // Valor por defecto
            
            // Configuraciones de conversaciones
            nConversacionesMaxSimultaneas = 10, // Valor por defecto
            bConversacionesPermitirChatsGrupales = true, // Valor por defecto
            nConversacionesMaxParticipantesGrupo = 50, // Valor por defecto
            
            // Configuraciones de contactos
            bContactosPermitirAgregar = true, // Valor por defecto
            bContactosPermitirInvitaciones = true, // Valor por defecto
            nContactosMaxContactos = 500, // Valor por defecto
            
            // Configuraciones de notificaciones
            bNotificacionesEmail = dto.BPermitirNotificaciones,
            bNotificacionesPush = dto.BPermitirNotificaciones,
            bNotificacionesSonido = dto.BPermitirNotificaciones,
            
            // Configuraciones de seguridad
            nSeguridadTiempoSesionMinutos = dto.NTiempoExpiracionSesion,
            bSeguridadRequiereAutenticacion = dto.BRequiereAutenticacion,
            bSeguridadPermitirSesionesMultiples = true, // Valor por defecto
            bSeguridadRequiere2FA = false, // Valor por defecto
            bSeguridadEncriptarMensajes = true, // Valor por defecto
            bSeguridadLogearActividad = true, // Valor por defecto
            
            // Configuraciones de interfaz - usando propiedades disponibles
            bInterfazModoOscuro = true, // Valor por defecto
            cInterfazIdioma = "es", // Valor por defecto
            cInterfazTema = "default" // Valor por defecto
        };
    }

    /// <summary>
    /// Mapea de CreateDto a ConfiguracionAplicacionDto
    /// </summary>
    private ConfiguracionAplicacionDto MapCreateDtoToConfiguracionDto(CreateConfiguracionAplicacionUnificadaDto createDto, string codigoAplicacion)
    {
        return new ConfiguracionAplicacionDto
        {
            CAppCodigo = codigoAplicacion,
            
            // Configuraciones de adjuntos
            NMaxTamanoArchivo = createDto.NMaxTamanoArchivo ?? 10485760, // 10MB por defecto
            CTiposArchivosPermitidos = createDto.CTiposArchivosPermitidos ?? "jpg,jpeg,png,gif,pdf,doc,docx,txt",
            BPermitirAdjuntos = createDto.BPermitirAdjuntos ?? true,
            NMaxCantidadAdjuntos = createDto.NMaxArchivosSimultaneos ?? 5,
            
            // Configuraciones de chat
            NMaxLongitudMensaje = createDto.NMaxLongitudMensaje ?? 1000,
            BPermitirEmojis = createDto.BPermitirEmojis ?? true,
            
            // Configuraciones de notificaciones
            BPermitirNotificaciones = createDto.BNotificacionesPush ?? true,
            
            // Configuraciones de seguridad
            BRequiereAutenticacion = createDto.BRequiereAutenticacion ?? true,
            NTiempoExpiracionSesion = createDto.NTiempoSesionMinutos ?? 1440,
            
            // Metadatos
            BEsActiva = true
        };
    }

    /// <summary>
    /// Mapea de UpdateDto a ConfiguracionAplicacionDto
    /// </summary>
    private ConfiguracionAplicacionDto MapUpdateDtoToConfiguracionDto(UpdateConfiguracionAplicacionUnificadaDto updateDto, ConfiguracionAplicacionDto existente)
    {
        return new ConfiguracionAplicacionDto
        {
            Id = existente.Id,
            CAppCodigo = existente.CAppCodigo,
            
            // Configuraciones de adjuntos
            NMaxTamanoArchivo = updateDto.NMaxTamanoArchivo ?? existente.NMaxTamanoArchivo,
            CTiposArchivosPermitidos = updateDto.CTiposArchivosPermitidos ?? existente.CTiposArchivosPermitidos,
            BPermitirAdjuntos = updateDto.BPermitirAdjuntos ?? existente.BPermitirAdjuntos,
            NMaxCantidadAdjuntos = updateDto.NMaxArchivosSimultaneos ?? existente.NMaxCantidadAdjuntos,
            
            // Configuraciones de chat
            NMaxLongitudMensaje = updateDto.NMaxLongitudMensaje ?? existente.NMaxLongitudMensaje,
            BPermitirEmojis = updateDto.BPermitirEmojis ?? existente.BPermitirEmojis,
            
            // Configuraciones de notificaciones
            BPermitirNotificaciones = updateDto.BNotificacionesPush ?? existente.BPermitirNotificaciones,
            
            // Configuraciones de seguridad
            BRequiereAutenticacion = updateDto.BRequiereAutenticacion ?? existente.BRequiereAutenticacion,
            NTiempoExpiracionSesion = updateDto.NTiempoExpiracionSesion ?? existente.NTiempoExpiracionSesion,
            
            // Metadatos
            BEsActiva = updateDto.BEsActiva ?? existente.BEsActiva,
            CreatedAt = existente.CreatedAt,
            UpdatedAt = DateTime.UtcNow
        };
    }

    /// <summary>
    /// Mapea de entidad a DTO
    /// </summary>
    private ConfiguracionAplicacionUnificadaDto MapEntityToDto(ConfiguracionAplicacionUnificada entity)
    {
        return new ConfiguracionAplicacionUnificadaDto
        {
            nConfiguracionAplicacionId = entity.NConfiguracionAplicacionId,
            nAplicacionesId = entity.NAplicacionesId,
            
            // Configuraciones de adjuntos
            nAdjuntosMaxTamanoArchivo = entity.NMaxTamanoArchivo ?? 0,
            cAdjuntosTiposArchivosPermitidos = entity.CTiposArchivosPermitidos ?? string.Empty,
            bAdjuntosPermitirAdjuntos = entity.BPermitirAdjuntos ?? false,
            nAdjuntosMaxArchivosSimultaneos = entity.NMaxArchivosSimultaneos ?? 0,
            bAdjuntosPermitirImagenes = entity.BPermitirImagenes ?? false,
            bAdjuntosPermitirDocumentos = entity.BPermitirDocumentos ?? false,
            bAdjuntosPermitirVideos = entity.BPermitirVideos ?? false,
            bAdjuntosPermitirAudio = entity.BPermitirAudio ?? false,
            bAdjuntosRequiereAprobacion = entity.BRequiereAprobacionAdjuntos ?? false,
            
            // Configuraciones de chat
            nChatMaxLongitudMensaje = entity.NMaxLongitudMensaje ?? 0,
            bChatPermitirEmojis = entity.BPermitirEmojis ?? false,
            bChatPermitirMenciones = entity.BPermitirMenciones ?? false,
            bChatPermitirReacciones = entity.BPermitirReacciones ?? false,
            bChatPermitirEdicionMensajes = entity.BPermitirEdicionMensajes ?? false,
            bChatPermitirEliminacionMensajes = entity.BPermitirEliminacionMensajes ?? false,
            nChatTiempoLimiteEdicion = entity.NTiempoLimiteEdicion ?? 0,
            bChatPermitirMensajesPrivados = entity.BPermitirMensajesPrivados ?? false,
            
            // Configuraciones de conversaciones
            nConversacionesMaxSimultaneas = entity.NMaxConversacionesSimultaneas ?? 0,
            bConversacionesPermitirChatsGrupales = entity.BPermitirChatsGrupales ?? false,
            nConversacionesMaxParticipantesGrupo = entity.NMaxParticipantesGrupo ?? 0,
            bConversacionesPermitirCrearGrupos = entity.BPermitirCrearGrupos ?? false,
            bConversacionesRequiereAprobacionGrupos = entity.BRequiereAprobacionGrupos ?? false,
            
            // Configuraciones de contactos
            bContactosPermitirAgregar = entity.BPermitirAgregarContactos ?? false,
            bContactosRequiereAprobacion = entity.BRequiereAprobacionContactos ?? false,
            bContactosPermitirBusquedaGlobal = entity.BPermitirBusquedaGlobal ?? false,
            bContactosPermitirInvitaciones = entity.BPermitirInvitaciones ?? false,
            nContactosMaxContactos = entity.NMaxContactos ?? 0,
            
            // Configuraciones de notificaciones
            bNotificacionesEmail = entity.BNotificacionesEmail ?? false,
            bNotificacionesPush = entity.BNotificacionesPush ?? false,
            bNotificacionesEnTiempoReal = entity.BNotificacionesEscritorio ?? false,
            bNotificacionesSonido = entity.BNotificacionesSonido ?? false,
            bNotificacionesVibracion = entity.BNotificarMenciones ?? false,
            
            // Configuraciones de seguridad
            nSeguridadTiempoSesionMinutos = entity.NTiempoExpiracionSesion ?? 0,
            bSeguridadRequiereAutenticacion = entity.BRequiereAutenticacion ?? false,
            bSeguridadPermitirSesionesMultiples = entity.BPermitirSesionesMultiples ?? false,
            bSeguridadRequiere2FA = entity.BRequiere2FA ?? false,
            bSeguridadEncriptarMensajes = entity.BEncriptarMensajes ?? false,
            
            // Configuraciones de integraciones
            bIntegracionHabilitarWebhooks = entity.BPermitirWebhooks ?? false,
            bIntegracionHabilitarAPI = entity.BPermitirAPI ?? false,
            
            // Metadatos
            dConfiguracionFechaCreacion = entity.DFechaCreacion,
            dConfiguracionFechaModificacion = entity.DFechaActualizacion,
            cConfiguracionCreadoPor = entity.CCreadoPor ?? string.Empty,
            cConfiguracionModificadoPor = entity.CActualizadoPor ?? string.Empty,
            bConfiguracionEstaActiva = entity.BEsActiva
        };
    }



    #endregion

    /// <summary>
    /// Lista todas las configuraciones de aplicaciones
    /// </summary>
    public async Task<IEnumerable<ConfiguracionAplicacionUnificadaDto>> ListarConfiguracionesAsync()
    {
        try
        {
            _logger.LogInformation("Listando todas las configuraciones de aplicaciones");

            var configuraciones = await _repository.ListarConfiguracionesActivasAsync();
            return configuraciones.Select(MapConfiguracionDtoToUnificadaDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar configuraciones de aplicaciones");
            return new List<ConfiguracionAplicacionUnificadaDto>();
        }
    }

    /// <summary>
    /// Mapea de ConfiguracionAplicacionDto a ConfiguracionAplicacionUnificadaDto
    /// </summary>
    private ConfiguracionAplicacionUnificadaDto MapConfiguracionDtoToUnificadaDto(ConfiguracionAplicacionDto dto)
    {
        return new ConfiguracionAplicacionUnificadaDto
        {
            nConfiguracionAplicacionId = dto.Id,
            nAplicacionesId = 0, // No disponible en ConfiguracionAplicacionDto
            
            // Configuraciones de adjuntos
            nAdjuntosMaxTamanoArchivo = dto.NMaxTamanoArchivo,
            cAdjuntosTiposArchivosPermitidos = dto.CTiposArchivosPermitidos ?? string.Empty,
            bAdjuntosPermitirAdjuntos = dto.BPermitirAdjuntos,
            
            // Configuraciones de chat
            nChatMaxLongitudMensaje = dto.NMaxLongitudMensaje,
            bChatPermitirEmojis = dto.BPermitirEmojis,
            
            // Configuraciones de seguridad
            nSeguridadTiempoSesionMinutos = dto.NTiempoExpiracionSesion,
            bSeguridadRequiereAutenticacion = dto.BRequiereAutenticacion,
            
            // Metadatos
            dConfiguracionFechaCreacion = dto.CreatedAt,
            dConfiguracionFechaModificacion = dto.UpdatedAt,
            cConfiguracionCreadoPor = string.Empty, // No disponible en ConfiguracionAplicacionDto
            cConfiguracionModificadoPor = string.Empty, // No disponible en ConfiguracionAplicacionDto
            bConfiguracionEstaActiva = dto.BEsActiva
        };
    }
}

}
