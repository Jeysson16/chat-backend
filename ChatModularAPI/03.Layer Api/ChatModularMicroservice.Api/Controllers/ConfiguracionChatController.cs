using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de configuraciones de chat
    /// </summary>
    [ApiController]
    [Route("api/v1/configuracion-chat")]
    [Authorize]
    public class ConfiguracionChatController : BaseController
    {
        private readonly IConfiguracionChatService _configuracionChatService;
        private new readonly ILogger<ConfiguracionChatController> _logger;

        public ConfiguracionChatController(
            IConfiguracionChatService configuracionChatService,
            ILogger<ConfiguracionChatController> logger) : base(logger)
        {
            _configuracionChatService = configuracionChatService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene la configuración completa de chat para una empresa y aplicación
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Configuración de chat completa</returns>
        [HttpGet("{empresaId:guid}/{aplicacionId:guid}")]
        public async Task<IActionResult> ObtenerConfiguracionChat(
            Guid empresaId, 
            Guid aplicacionId)
        {
            try
            {
                _logger.LogInformation("Obteniendo configuración de chat para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

                var resultado = await _configuracionChatService.ObtenerConfiguracionChatAsync(empresaId, aplicacionId);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(resultado.lstItem?.FirstOrDefault() ?? new ConfiguracionChatDto(), GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al obtener configuración", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración de chat para empresa {EmpresaId}", empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Actualiza la configuración completa de chat para una empresa y aplicación
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="configuracion">Nueva configuración</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{empresaId:guid}/{aplicacionId:guid}")]
        public async Task<IActionResult> ActualizarConfiguracionChat(
            Guid empresaId, 
            Guid aplicacionId, 
            [FromBody] ConfiguracionChatDto configuracion)
        {
            try
            {
                _logger.LogInformation("Actualizando configuración de chat para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

                // Validar que los IDs coincidan con el DTO (nEmpresasId / nAplicacionesId)
                if (configuracion.nEmpresasId != empresaId || configuracion.nAplicacionesId != aplicacionId)
                {
                    return BadRequest(CreateErrorResponse("Los IDs de empresa y aplicación no coinciden", GetClientName(), GetUserName()));
                }

                var resultado = await _configuracionChatService.ActualizarConfiguracionChatAsync(empresaId, aplicacionId, configuracion);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(true, GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al actualizar configuración", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar configuración de chat para empresa {EmpresaId}", empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Inicializa la configuración por defecto para una empresa y aplicación
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("{empresaId:guid}/{aplicacionId:guid}/inicializar")]
        public async Task<IActionResult> InicializarConfiguracionPorDefecto(
            Guid empresaId, 
            Guid aplicacionId)
        {
            try
            {
                _logger.LogInformation("Inicializando configuración por defecto para empresa {EmpresaId} y aplicación {AplicacionId}", empresaId, aplicacionId);

                var resultado = await _configuracionChatService.InicializarConfiguracionPorDefectoAsync(empresaId, aplicacionId);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al inicializar configuración", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al inicializar configuración por defecto para empresa {EmpresaId}", empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene una configuración específica por clave
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="clave">Clave de configuración</param>
        /// <returns>Valor de la configuración</returns>
        [HttpGet("{empresaId:guid}/{aplicacionId:guid}/clave/{clave}")]
        public async Task<IActionResult> ObtenerConfiguracionPorClave(
            Guid empresaId, 
            Guid aplicacionId, 
            string clave)
        {
            try
            {
                _logger.LogInformation("Obteniendo configuración {Clave} para empresa {EmpresaId}", clave, empresaId);

                var resultado = await _configuracionChatService.ObtenerConfiguracionPorClaveAsync(empresaId, aplicacionId, clave);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(resultado.lstItem?.FirstOrDefault() ?? "", GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al obtener configuración", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener configuración {Clave} para empresa {EmpresaId}", clave, empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Establece una configuración específica por clave
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="clave">Clave de configuración</param>
        /// <param name="request">Datos de la configuración</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPut("{empresaId:guid}/{aplicacionId:guid}/clave/{clave}")]
        public async Task<IActionResult> EstablecerConfiguracionPorClave(
            Guid empresaId, 
            Guid aplicacionId, 
            string clave, 
            [FromBody] EstablecerConfiguracionRequest request)
        {
            try
            {
                _logger.LogInformation("Estableciendo configuración {Clave} para empresa {EmpresaId}", clave, empresaId);

                var resultado = await _configuracionChatService.EstablecerConfiguracionPorClaveAsync(
                    empresaId, aplicacionId, clave, request.Valor, request.Descripcion);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al establecer configuración", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al establecer configuración {Clave} para empresa {EmpresaId}", clave, empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Valida si una empresa puede usar una funcionalidad específica
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="funcionalidad">Nombre de la funcionalidad</param>
        /// <returns>True si puede usar la funcionalidad</returns>
        [HttpGet("{empresaId:guid}/{aplicacionId:guid}/validar/{funcionalidad}")]
        public async Task<IActionResult> ValidarFuncionalidad(
            Guid empresaId, 
            Guid aplicacionId, 
            string funcionalidad)
        {
            try
            {
                _logger.LogInformation("Validando funcionalidad {Funcionalidad} para empresa {EmpresaId}", funcionalidad, empresaId);

                var resultado = await _configuracionChatService.ValidarFuncionalidadAsync(empresaId, aplicacionId, funcionalidad);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(resultado.lstItem?.FirstOrDefault() ?? false, GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al validar funcionalidad", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar funcionalidad {Funcionalidad} para empresa {EmpresaId}", funcionalidad, empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene todas las configuraciones de una empresa y aplicación
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Diccionario con todas las configuraciones</returns>
        [HttpGet("{empresaId:guid}/{aplicacionId:guid}/todas")]
        public async Task<IActionResult> ObtenerTodasConfiguraciones(
            Guid empresaId, 
            Guid aplicacionId)
        {
            try
            {
                _logger.LogInformation("Obteniendo todas las configuraciones para empresa {EmpresaId}", empresaId);

                var resultado = await _configuracionChatService.ObtenerTodasConfiguracionesAsync(empresaId, aplicacionId);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(resultado.lstItem?.FirstOrDefault() ?? new Dictionary<string, string>(), GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al obtener configuraciones", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener todas las configuraciones para empresa {EmpresaId}", empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Copia configuraciones de una empresa a otra
        /// </summary>
        /// <param name="empresaOrigenId">ID de la empresa origen</param>
        /// <param name="empresaDestinoId">ID de la empresa destino</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("copiar/{empresaOrigenId:guid}/{empresaDestinoId:guid}/{aplicacionId:guid}")]
        public async Task<IActionResult> CopiarConfiguraciones(
            Guid empresaOrigenId, 
            Guid empresaDestinoId, 
            Guid aplicacionId)
        {
            try
            {
                _logger.LogInformation("Copiando configuraciones de empresa {EmpresaOrigen} a empresa {EmpresaDestino}", empresaOrigenId, empresaDestinoId);

                var resultado = await _configuracionChatService.CopiarConfiguracionesAsync(empresaOrigenId, empresaDestinoId, aplicacionId);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al copiar configuraciones", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al copiar configuraciones de empresa {EmpresaOrigen} a empresa {EmpresaDestino}", empresaOrigenId, empresaDestinoId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Resetea todas las configuraciones a valores por defecto
        /// </summary>
        /// <param name="empresaId">ID de la empresa</param>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpPost("resetear/{empresaId:guid}/{aplicacionId:guid}")]
        public async Task<IActionResult> ResetearConfiguraciones(
            Guid empresaId, 
            Guid aplicacionId)
        {
            try
            {
                _logger.LogInformation("Reseteando configuraciones para empresa {EmpresaId}", empresaId);

                var resultado = await _configuracionChatService.ResetearConfiguracionesAsync(empresaId, aplicacionId);

                if (resultado.isSuccess)
                {
                    return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
                }

                return BadRequest(CreateErrorResponse("Error al resetear configuraciones", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al resetear configuraciones para empresa {EmpresaId}", empresaId);
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }
    }

    /// <summary>
    /// DTO para establecer configuración por clave
    /// </summary>
    public class EstablecerConfiguracionRequest
    {
        /// <summary>
        /// Valor de la configuración
        /// </summary>
        public string Valor { get; set; } = string.Empty;

        /// <summary>
        /// Descripción opcional de la configuración
        /// </summary>
        public string? Descripcion { get; set; }
    }
}