using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using Utils = ChatModularMicroservice.Shared.Utils;
using System.ComponentModel.DataAnnotations;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador para la gestión de configuraciones unificadas de aplicaciones
/// </summary>
[ApiController]
[Route("api/v1/configuracion-aplicacion")]
// [Authorize] // Temporarily disabled for testing
    public class ConfiguracionAplicacionUnificadaController : BaseController
    {
    private readonly IConfiguracionAplicacionUnificadaService _configuracionService;

    public ConfiguracionAplicacionUnificadaController(
        IConfiguracionAplicacionUnificadaService configuracionService,
        ILogger<ConfiguracionAplicacionUnificadaController> logger) : base(logger)
    {
        _configuracionService = configuracionService;
        }

    

    /// <summary>
    /// Obtiene la configuración completa de una aplicación por ID
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Configuración completa de la aplicación</returns>
    [HttpGet("{aplicacionId:int}")]
    public async Task<IActionResult> ObtenerConfiguracion(
        [Required] int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación: {AplicacionId}", aplicacionId);

            var configuracion = await _configuracionService.ObtenerConfiguracionAsync(aplicacionId);
            
            if (configuracion == null)
            {
                return NotFound(CreateErrorResponse(
                    $"No se encontró configuración para la aplicación {aplicacionId}", GetClientName(), GetUserName()));
            }

            return Ok(CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene la configuración completa de una aplicación por código
    /// </summary>
    /// <param name="codigoAplicacion">Código de la aplicación</param>
    /// <returns>Configuración completa de la aplicación</returns>
    [HttpGet("por-codigo/{codigoAplicacion}")]
    public async Task<IActionResult> ObtenerConfiguracionPorCodigo(
        [Required] string codigoAplicacion)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración para aplicación con código: {CodigoAplicacion}", codigoAplicacion);

            var configuracion = await _configuracionService.ObtenerConfiguracionPorCodigoAsync(codigoAplicacion);
            
            if (configuracion == null)
            {
                return NotFound(CreateErrorResponse(
                    $"No se encontró configuración para la aplicación con código {codigoAplicacion}", GetClientName(), GetUserName()));
            }

            return Ok(CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración para aplicación con código: {CodigoAplicacion}", codigoAplicacion);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Crea una nueva configuración para una aplicación
    /// </summary>
    /// <param name="createDto">Datos para crear la configuración</param>
    /// <returns>Configuración creada</returns>
    [HttpPost]
    public async Task<IActionResult> CrearConfiguracion(
        [FromBody] CreateConfiguracionAplicacionUnificadaDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse(
                    "Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            _logger.LogInformation("Creando configuración para aplicación: {AplicacionId}", createDto.NAplicacionesId);

            var configuracion = await _configuracionService.CrearConfiguracionAsync(createDto);

            return CreatedAtAction(
                nameof(ObtenerConfiguracion),
                new { aplicacionId = createDto.NAplicacionesId },
                CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al crear configuración");
            return BadRequest(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear configuración para aplicación: {AplicacionId}", createDto.NAplicacionesId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza la configuración de una aplicación
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <param name="updateDto">Datos para actualizar la configuración</param>
    /// <returns>Configuración actualizada</returns>
    [HttpPut("{aplicacionId:int}")]
    public async Task<IActionResult> ActualizarConfiguracion(
        [Required] int aplicacionId,
        [FromBody] UpdateConfiguracionAplicacionUnificadaDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse(
                    "Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            _logger.LogInformation("Actualizando configuración para aplicación: {AplicacionId}", aplicacionId);

            var configuracion = await _configuracionService.ActualizarConfiguracionAsync(aplicacionId, updateDto);

            return Ok(CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error de validación al actualizar configuración");
            return BadRequest(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene solo la configuración de adjuntos de una aplicación
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Configuración de adjuntos</returns>
    [HttpGet("{aplicacionId:int}/adjuntos")]
    public async Task<IActionResult> ObtenerConfiguracionAdjuntos(
        [Required] int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Obteniendo configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);

            var configuracion = await _configuracionService.ObtenerConfiguracionAdjuntosAsync(aplicacionId);
            
            if (configuracion == null)
            {
                return NotFound(CreateErrorResponse(
                    $"No se encontró configuración de adjuntos para la aplicación {aplicacionId}", GetClientName(), GetUserName()));
            }

            return Ok(CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza solo la configuración de adjuntos de una aplicación
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <param name="adjuntosDto">Configuración de adjuntos a actualizar</param>
    /// <returns>Configuración de adjuntos actualizada</returns>
    [HttpPut("{aplicacionId:int}/adjuntos")]
    public async Task<IActionResult> ActualizarConfiguracionAdjuntos(
        [Required] int aplicacionId,
        [FromBody] ConfiguracionAdjuntosDto adjuntosDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            _logger.LogInformation("Actualizando configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);

            var resultado = await _configuracionService.ActualizarConfiguracionAdjuntosAsync(aplicacionId, adjuntosDto);

            return Ok(CreateSuccessResponse(resultado, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración de adjuntos para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Verifica si una aplicación tiene configuración
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>True si existe configuración, false en caso contrario</returns>
    [HttpGet("{aplicacionId:int}/existe")]
    public async Task<IActionResult> ExisteConfiguracion(
        [Required] int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Verificando existencia de configuración para aplicación: {AplicacionId}", aplicacionId);

            var existe = await _configuracionService.ExisteConfiguracionAsync(aplicacionId);

            return Ok(CreateSuccessResponse(existe, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar configuración para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Elimina la configuración de una aplicación
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{aplicacionId:int}")]
    public async Task<IActionResult> EliminarConfiguracion(
        [Required] int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Eliminando configuración para aplicación: {AplicacionId}", aplicacionId);

            var resultado = await _configuracionService.EliminarConfiguracionAsync(aplicacionId);

            if (resultado)
            {
                return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
            }

            return NotFound(CreateErrorResponse(
                $"No se encontró configuración para eliminar para la aplicación {aplicacionId}", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar configuración para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene todas las configuraciones activas
    /// </summary>
    /// <returns>Lista de configuraciones activas</returns>
    [HttpGet("activas")]
    public async Task<IActionResult> ObtenerConfiguracionesActivas()
    {
        try
        {
            _logger.LogInformation("Obteniendo todas las configuraciones activas");

            var configuraciones = await _configuracionService.ObtenerConfiguracionesActivasAsync();

            return Ok(CreateSuccessResponse(configuraciones, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuraciones activas");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Restaura la configuración de una aplicación a valores por defecto
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Configuración restaurada</returns>
    [HttpPost("{aplicacionId:int}/restaurar-defecto")]
    public async Task<IActionResult> RestaurarConfiguracionPorDefecto(
        [Required] int aplicacionId)
    {
        try
        {
            _logger.LogInformation("Restaurando configuración por defecto para aplicación: {AplicacionId}", aplicacionId);

            var configuracion = await _configuracionService.RestaurarConfiguracionPorDefectoAsync(aplicacionId);

            return Ok(CreateSuccessResponse(configuracion, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al restaurar configuración por defecto para aplicación: {AplicacionId}", aplicacionId);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Valida que los tipos de archivos permitidos sean válidos
    /// </summary>
    /// <param name="tiposArchivos">Tipos de archivos separados por coma</param>
    /// <returns>True si son válidos, false en caso contrario</returns>
    [HttpPost("validar-tipos-archivos")]
    public async Task<IActionResult> ValidarTiposArchivos(
        [FromBody] string tiposArchivos)
    {
        try
        {
            _logger.LogInformation("Validando tipos de archivos: {TiposArchivos}", tiposArchivos);

            var esValido = await _configuracionService.ValidarTiposArchivosAsync(tiposArchivos);

            return Ok(CreateSuccessResponse(esValido, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al validar tipos de archivos: {TiposArchivos}", tiposArchivos);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene los tipos de archivos permitidos por defecto
    /// </summary>
    /// <returns>Tipos de archivos por defecto</returns>
    [HttpGet("tipos-archivos-defecto")]
    public async Task<IActionResult> ObtenerTiposArchivosPorDefecto()
    {
        try
        {
            _logger.LogInformation("Obteniendo tipos de archivos por defecto");

            var tiposDefecto = await _configuracionService.ObtenerTiposArchivosPorDefectoAsync();

            return Ok(CreateSuccessResponse(tiposDefecto, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener tipos de archivos por defecto");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }
}
