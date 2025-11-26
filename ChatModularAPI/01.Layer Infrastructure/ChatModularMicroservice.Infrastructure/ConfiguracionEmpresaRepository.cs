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
        try
        {
            _logger.LogInformation("Obteniendo todas las configuraciones de empresa");

            var parameters = new Dictionary<string, object>();
            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetAll", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones de empresa", result.lstItem.Count);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones de empresa");
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las configuraciones de empresa");
            throw;
        }
    }

    /// <summary>
    /// Obtiene una configuración por ID
    /// </summary>
    public async Task<ConfiguracionEmpresaDto?> GetByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración de empresa por ID: {ConfiguracionId}", id);

            var parameters = new Dictionary<string, object>
            {
                { "pConfiguracionId", id }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetById", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                _logger.LogInformation("Configuración de empresa encontrada: {ConfiguracionId}", id);
                return (ConfiguracionEmpresaDto)result.lstItem.First();
            }
            
            _logger.LogWarning("Configuración de empresa no encontrada: {ConfiguracionId}", id);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de empresa por ID: {ConfiguracionId}", id);
            throw;
        }
    }

    /// <summary>
    /// Obtiene configuraciones por empresa
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAsync(int empresaId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por empresa: {EmpresaId}", empresaId);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", empresaId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetByEmpresa", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones para la empresa: {EmpresaId}", result.lstItem.Count, empresaId);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones para la empresa: {EmpresaId}", empresaId);
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por empresa: {EmpresaId}", empresaId);
            throw;
        }
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
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_GetByEmpresaAndAplicacion", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se encontraron {Count} configuraciones para empresa {EmpresaId} y aplicación {AplicacionId}", result.lstItem.Count, empresaId, aplicacionId);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se encontraron configuraciones para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            throw;
        }
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
        try
        {
            _logger.LogInformation("Creando nueva configuración de empresa para empresa {EmpresaId}, aplicación {AplicacionId}, clave {Clave}", 
                createDto.NEmpresasId, createDto.NAplicacionesId, createDto.CConfiguracionEmpresaClave);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", createDto.NEmpresasId },
                { "pAplicacionId", createDto.NAplicacionesId },
                { "pClave", createDto.CConfiguracionEmpresaClave },
                { "pValor", createDto.CConfiguracionEmpresaValor },
                { "pTipo", createDto.CConfiguracionEmpresaTipo },
                { "pDescripcion", createDto.CConfiguracionEmpresaDescripcion ?? string.Empty },
                { "pEsActiva", createDto.BConfiguracionEmpresaEsActiva }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_Create", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                var configuracion = (ConfiguracionEmpresaDto)result.lstItem.First();
                _logger.LogInformation("Configuración de empresa creada exitosamente: {ConfiguracionId}", configuracion.nConfiguracionEmpresaId);
                return configuracion;
            }
            
            throw new InvalidOperationException("Error al crear la configuración de empresa");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear configuración de empresa");
            throw;
        }
    }

    /// <summary>
    /// Actualiza una configuración existente
    /// </summary>
    public async Task<ConfiguracionEmpresaDto> UpdateAsync(int id, UpdateConfiguracionEmpresaDto updateDto)
    {
        try
        {
            _logger.LogInformation("Actualizando configuración de empresa: {ConfiguracionId}", id);

            var parameters = new Dictionary<string, object>
            {
                { "pConfiguracionId", id },
                { "pValor", updateDto.CConfiguracionEmpresaValor ?? string.Empty },
                { "pTipo", updateDto.CConfiguracionEmpresaTipo ?? string.Empty },
                { "pDescripcion", updateDto.CConfiguracionEmpresaDescripcion ?? string.Empty },
                { "pEsActiva", updateDto.BConfiguracionEmpresaEsActiva ?? true }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_Update", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                var configuracion = (ConfiguracionEmpresaDto)result.lstItem.First();
                _logger.LogInformation("Configuración de empresa actualizada exitosamente: {ConfiguracionId}", id);
                return configuracion;
            }
            
            throw new InvalidOperationException("Error al actualizar la configuración de empresa");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración de empresa: {ConfiguracionId}", id);
            throw;
        }
    }

    /// <summary>
    /// Elimina una configuración
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando configuración de empresa: {ConfiguracionId}", id);

            var parameters = new Dictionary<string, object>
            {
                { "pConfiguracionId", id }
            };

            var result = await ExecuteStoredProcedureListAsync<dynamic>("USP_ConfiguracionEmpresa_Delete", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                dynamic resultData = result.lstItem.First();
                bool success = Convert.ToBoolean(resultData.success);
                _logger.LogInformation("Configuración de empresa eliminada: {Success}", success);
                return success;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar configuración de empresa: {ConfiguracionId}", id);
            throw;
        }
    }

    /// <summary>
    /// Verifica si existe una configuración con la clave especificada para una empresa y aplicación
    /// </summary>
    public async Task<bool> ExistsByClaveAsync(string clave, int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);

            var parameters = new Dictionary<string, object>
            {
                { "pClave", clave },
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<dynamic>("USP_ConfiguracionEmpresa_ExistsByClave", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                dynamic resultData = result.lstItem.First();
                bool exists = Convert.ToBoolean(resultData.exists);
                _logger.LogInformation("Configuración existe por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}: {Exists}", clave, empresaId, aplicacionId, exists);
                return exists;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            throw;
        }
    }

    /// <summary>
    /// Verifica si existe una configuración con el ID especificado
    /// </summary>
    public async Task<bool> ExistsAsync(int id)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de configuración por ID: {ConfiguracionId}", id);

            var parameters = new Dictionary<string, object>
            {
                { "pConfiguracionId", id }
            };

            var result = await ExecuteStoredProcedureListAsync<dynamic>("USP_ConfiguracionEmpresa_Exists", parameters);
            
            if (result.isSuccess && result.lstItem != null && result.lstItem.Any())
            {
                dynamic resultData = result.lstItem.First();
                bool exists = Convert.ToBoolean(resultData.exists);
                _logger.LogInformation("Configuración existe por ID {ConfiguracionId}: {Exists}", id, exists);
                return exists;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración por ID: {ConfiguracionId}", id);
            throw;
        }
    }

    /// <summary>
    /// Copia configuraciones de aplicación a empresa
    /// </summary>
    public async Task<List<ConfiguracionEmpresaDto>> CopiarConfiguracionesDeAplicacionAsync(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Copiando configuraciones de aplicación {AplicacionId} a empresa {EmpresaId}", aplicacionId, empresaId);

            var parameters = new Dictionary<string, object>
            {
                { "pEmpresaId", empresaId },
                { "pAplicacionId", aplicacionId }
            };

            var result = await ExecuteStoredProcedureListAsync<ConfiguracionEmpresaDto>("USP_ConfiguracionEmpresa_CopiarDeAplicacion", parameters);
            
            if (result.isSuccess && result.lstItem != null)
            {
                _logger.LogInformation("Se copiaron {Count} configuraciones de aplicación a empresa", result.lstItem.Count);
                return result.lstItem.Cast<ConfiguracionEmpresaDto>().ToList();
            }
            
            _logger.LogWarning("No se copiaron configuraciones de aplicación a empresa");
            return new List<ConfiguracionEmpresaDto>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al copiar configuraciones de aplicación {AplicacionId} a empresa {EmpresaId}", aplicacionId, empresaId);
            throw;
        }
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