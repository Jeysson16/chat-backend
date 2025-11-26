using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador para la gestión de configuraciones de empresa
/// </summary>
[ApiController]
[Route("api/v1/configuracion-empresa")]
[Authorize]
public class ConfiguracionEmpresaController : ControllerBase
{
    private readonly IConfiguracionEmpresaService _configuracionEmpresaService;
    private readonly ILogger<ConfiguracionEmpresaController> _logger;

    public ConfiguracionEmpresaController(
        IConfiguracionEmpresaService configuracionEmpresaService,
        ILogger<ConfiguracionEmpresaController> logger)
    {
        _configuracionEmpresaService = configuracionEmpresaService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todas las configuraciones de empresa
    /// </summary>
    /// <returns>Lista de configuraciones de empresa</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> GetAll()
    {
        try
        {
            _logger.LogInformation("Obteniendo todas las configuraciones de empresa");
            var configuraciones = await _configuracionEmpresaService.GetAllAsync();
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones de empresa obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todas las configuraciones de empresa");
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones de empresa"
            });
        }
    }

    /// <summary>
    /// Obtiene una configuración de empresa por ID
    /// </summary>
    /// <param name="id">ID de la configuración</param>
    /// <returns>Configuración de empresa</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionEmpresaDto>>> GetById(int id)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración de empresa por ID: {ConfiguracionId}", id);
            var configuracion = await _configuracionEmpresaService.GetByIdAsync(id);
            
            if (configuracion == null)
            {
                return NotFound(new ApiResponse<ConfiguracionEmpresaDto>
                {
                    Success = false,
                    Message = $"Configuración de empresa con ID {id} no encontrada"
                });
            }

            return Ok(new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = true,
                Data = configuracion,
                Message = "Configuración de empresa obtenida exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de empresa por ID: {ConfiguracionId}", id);
            return StatusCode(500, new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = "Error interno del servidor al obtener la configuración de empresa"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones por empresa
    /// </summary>
    /// <param name="empresaId">ID de la empresa</param>
    /// <returns>Lista de configuraciones de la empresa</returns>
    [HttpGet("empresa/{empresaId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> GetByEmpresa(int empresaId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por empresa: {EmpresaId}", empresaId);
            var configuraciones = await _configuracionEmpresaService.GetByEmpresaAsync(empresaId);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones de empresa obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por empresa: {EmpresaId}", empresaId);
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones de empresa"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones por aplicación
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Lista de configuraciones de la aplicación</returns>
    [HttpGet("aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> GetByAplicacion(int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por aplicación: {AplicacionId}", aplicacionId);
            var configuraciones = await _configuracionEmpresaService.GetByAplicacionAsync(aplicacionId);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones de aplicación obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por aplicación: {AplicacionId}", aplicacionId);
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones de aplicación"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones por empresa y aplicación
    /// </summary>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Lista de configuraciones</returns>
    [HttpGet("empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> GetByEmpresaAndAplicacion(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones por empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            var configuraciones = await _configuracionEmpresaService.GetByEmpresaAndAplicacionAsync(empresaId, aplicacionId);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones por empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones"
            });
        }
    }

    /// <summary>
    /// Obtiene una configuración específica por clave, empresa y aplicación
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Configuración específica</returns>
    [HttpGet("clave/{clave}/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionEmpresaDto>>> GetByClave(string clave, int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            var configuracion = await _configuracionEmpresaService.GetByClaveAsync(clave, empresaId, aplicacionId);
            
            if (configuracion == null)
            {
                return NotFound(new ApiResponse<ConfiguracionEmpresaDto>
                {
                    Success = false,
                    Message = $"Configuración con clave '{clave}' no encontrada para la empresa {empresaId} y aplicación {aplicacionId}"
                });
            }

            return Ok(new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = true,
                Data = configuracion,
                Message = "Configuración obtenida exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            return StatusCode(500, new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = "Error interno del servidor al obtener la configuración"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones activas
    /// </summary>
    /// <returns>Lista de configuraciones activas</returns>
    [HttpGet("activas")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> GetActivas()
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones activas");
            var configuraciones = await _configuracionEmpresaService.GetActivasAsync();
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones activas obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones activas");
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones activas"
            });
        }
    }

    /// <summary>
    /// Busca configuraciones por término
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <returns>Lista de configuraciones que coinciden con el término</returns>
    [HttpGet("buscar")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> Search([FromQuery] string searchTerm)
    {
        try
        {
            _logger.LogInformation("Buscando configuraciones con término: {SearchTerm}", searchTerm);
            var configuraciones = await _configuracionEmpresaService.SearchAsync(searchTerm);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Búsqueda de configuraciones completada exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar configuraciones con término: {SearchTerm}", searchTerm);
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al buscar configuraciones"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones agrupadas por empresa y aplicación
    /// </summary>
    /// <returns>Lista de configuraciones agrupadas</returns>
    [HttpGet("agrupadas/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaAgrupadaDto>>>> GetAgrupadas(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones agrupadas para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            var configuraciones = await _configuracionEmpresaService.GetAgrupadasAsync(empresaId, aplicacionId);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaAgrupadaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones agrupadas obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones agrupadas");
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaAgrupadaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones agrupadas"
            });
        }
    }

    /// <summary>
    /// Obtiene configuraciones heredadas de aplicación para una empresa
    /// </summary>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Lista de configuraciones heredadas</returns>
    [HttpGet("heredadas/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionHeredadaDto>>>> GetConfiguracionesHeredadas(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuraciones heredadas para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            var configuraciones = await _configuracionEmpresaService.GetConfiguracionesHeredadasAsync(empresaId, aplicacionId);
            
            return Ok(new ApiResponse<List<ConfiguracionHeredadaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones heredadas obtenidas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones heredadas");
            return StatusCode(500, new ApiResponse<List<ConfiguracionHeredadaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al obtener las configuraciones heredadas"
            });
        }
    }

    /// <summary>
    /// Crea una nueva configuración de empresa
    /// </summary>
    /// <param name="createDto">Datos para crear la configuración</param>
    /// <returns>Configuración creada</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<ConfiguracionEmpresaDto>>> Create([FromBody] CreateConfiguracionEmpresaDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ConfiguracionEmpresaDto>
                {
                    Success = false,
                    Message = "Datos de entrada inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            _logger.LogInformation("Creando nueva configuración de empresa");
            var configuracion = await _configuracionEmpresaService.CreateAsync(createDto);
            
            return CreatedAtAction(nameof(GetById), new { id = configuracion.nConfiguracionEmpresaId }, 
                new ApiResponse<ConfiguracionEmpresaDto>
                {
                    Success = true,
                    Data = configuracion,
                    Message = "Configuración de empresa creada exitosamente"
                });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear configuración de empresa");
            return BadRequest(new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear configuración de empresa");
            return StatusCode(500, new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = "Error interno del servidor al crear la configuración de empresa"
            });
        }
    }

    /// <summary>
    /// Actualiza una configuración existente
    /// </summary>
    /// <param name="id">ID de la configuración</param>
    /// <param name="updateDto">Datos para actualizar la configuración</param>
    /// <returns>Configuración actualizada</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<ApiResponse<ConfiguracionEmpresaDto>>> Update(int id, [FromBody] UpdateConfiguracionEmpresaDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ApiResponse<ConfiguracionEmpresaDto>
                {
                    Success = false,
                    Message = "Datos de entrada inválidos",
                    Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                });
            }

            _logger.LogInformation("Actualizando configuración de empresa: {ConfiguracionId}", id);
            var configuracion = await _configuracionEmpresaService.UpdateAsync(id, updateDto);
            
            return Ok(new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = true,
                Data = configuracion,
                Message = "Configuración de empresa actualizada exitosamente"
            });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar configuración de empresa: {ConfiguracionId}", id);
            return BadRequest(new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = ex.Message
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración de empresa: {ConfiguracionId}", id);
            return StatusCode(500, new ApiResponse<ConfiguracionEmpresaDto>
            {
                Success = false,
                Message = "Error interno del servidor al actualizar la configuración de empresa"
            });
        }
    }

    /// <summary>
    /// Elimina una configuración
    /// </summary>
    /// <param name="id">ID de la configuración</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResponse<bool>>> Delete(int id)
    {
        try
        {
            _logger.LogInformation("Eliminando configuración de empresa: {ConfiguracionId}", id);
            var result = await _configuracionEmpresaService.DeleteAsync(id);
            
            if (!result)
            {
                return NotFound(new ApiResponse<bool>
                {
                    Success = false,
                    Message = $"Configuración de empresa con ID {id} no encontrada"
                });
            }

            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = result,
                Message = "Configuración de empresa eliminada exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar configuración de empresa: {ConfiguracionId}", id);
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "Error interno del servidor al eliminar la configuración de empresa"
            });
        }
    }

    /// <summary>
    /// Verifica si existe una configuración con la clave especificada
    /// </summary>
    /// <param name="clave">Clave de la configuración</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>True si existe, false en caso contrario</returns>
    [HttpGet("existe/clave/{clave}/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<bool>>> ExistsByClave(string clave, int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            var exists = await _configuracionEmpresaService.ExistsByClaveAsync(clave, empresaId, aplicacionId);
            
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = exists,
                Message = exists ? "La configuración existe" : "La configuración no existe"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de configuración por clave {Clave}, empresa {EmpresaId} y aplicación {AplicacionId}", clave, empresaId, aplicacionId);
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "Error interno del servidor al verificar la existencia de la configuración"
            });
        }
    }

    /// <summary>
    /// Copia configuraciones de aplicación a empresa
    /// </summary>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Lista de configuraciones copiadas</returns>
    [HttpPost("copiar/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<List<ConfiguracionEmpresaDto>>>> CopiarConfiguracionesDeAplicacion(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Copiando configuraciones de aplicación {AplicacionId} a empresa {EmpresaId}", aplicacionId, empresaId);
            var configuraciones = await _configuracionEmpresaService.CopiarConfiguracionesDeAplicacionAsync(empresaId, aplicacionId);
            
            return Ok(new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = true,
                Data = configuraciones,
                Message = "Configuraciones copiadas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al copiar configuraciones de aplicación {AplicacionId} a empresa {EmpresaId}", aplicacionId, empresaId);
            return StatusCode(500, new ApiResponse<List<ConfiguracionEmpresaDto>>
            {
                Success = false,
                Message = "Error interno del servidor al copiar las configuraciones"
            });
        }
    }

    /// <summary>
    /// Restaura configuraciones de empresa a los valores por defecto de la aplicación
    /// </summary>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Resultado de la restauración</returns>
    [HttpPost("restaurar/empresa/{empresaId}/aplicacion/{aplicacionId}")]
    public async Task<ActionResult<ApiResponse<bool>>> RestaurarConfiguracionesPorDefecto(int empresaId, int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Restaurando configuraciones por defecto para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            var result = await _configuracionEmpresaService.RestaurarConfiguracionesPorDefectoAsync(empresaId, aplicacionId);
            
            return Ok(new ApiResponse<bool>
            {
                Success = true,
                Data = result,
                Message = "Configuraciones restauradas exitosamente"
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restaurar configuraciones por defecto para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);
            return StatusCode(500, new ApiResponse<bool>
            {
                Success = false,
                Message = "Error interno del servidor al restaurar las configuraciones"
            });
        }
    }
}