using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;

namespace ChatModularMicroservice.Api.Controllers
{
    /// <summary>
    /// Controlador para la gestión de aplicaciones
    /// </summary>
    [ApiController]
    [Route("api/v1/aplicaciones")]
    public class AplicacionesController : BaseController
    {
        private readonly IApplicationService _applicationService;
        private readonly ITokenService _tokenService;
        private readonly IAppRegistroRepository _appRegistroRepository;

        public AplicacionesController(
            IApplicationService applicationService,
            ITokenService tokenService,
            IAppRegistroRepository appRegistroRepository,
            ILogger<AplicacionesController> logger) : base(logger)
        {
            _applicationService = applicationService;
            _tokenService = tokenService;
            _appRegistroRepository = appRegistroRepository;
        }

        /// <summary>
        /// Obtiene todas las aplicaciones
        /// </summary>
        /// <returns>Lista de aplicaciones</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllApplications()
        {
            try
            {
                var applicationDtos = await _applicationService.GetAllApplicationsAsync();
                
                return Ok(CreateSuccessResponse(applicationDtos, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene aplicaciones activas
        /// </summary>
        /// <returns>Lista de aplicaciones activas</returns>
        [HttpGet("activas")]
        public async Task<IActionResult> GetActiveApplications()
        {
            try
            {
                var applicationDtos = await _applicationService.GetActiveApplicationsAsync();
                
                return Ok(CreateSuccessResponse(applicationDtos, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene una aplicación por ID
        /// </summary>
        /// <param name="id">ID de la aplicación</param>
        /// <returns>Aplicación encontrada</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetApplicationById(string id)
        {
            try
            {
                if (!int.TryParse(id, out int applicationId))
                {
                    return BadRequest(CreateErrorResponse("ID de aplicación inválido", GetClientName(), GetUserName()));
                }

                var applicationDto = await _applicationService.GetApplicationByIdAsync(applicationId);
                if (applicationDto == null)
                {
                    return NotFound(CreateErrorResponse("Aplicación no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(applicationDto, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene una aplicación por código
        /// </summary>
        /// <param name="codigo">Código de la aplicación</param>
        /// <returns>Aplicación encontrada</returns>
        [HttpGet("codigo/{codigo}")]
        public async Task<IActionResult> GetApplicationByCode(string codigo)
        {
            try
            {
                var applicationDto = await _applicationService.GetApplicationByCodeAsync(codigo);
                if (applicationDto == null)
                {
                    return NotFound(CreateErrorResponse("Aplicación no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(applicationDto, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Crea una nueva aplicación con tokens automáticos
        /// </summary>
        /// <param name="createApplicationDto">Datos de la aplicación a crear</param>
        /// <returns>Aplicación creada con tokens generados</returns>
        [HttpPost]
        public async Task<IActionResult> CreateApplication([FromBody] CreateApplicationDto createApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
                }

                // Crear la aplicación (el stored procedure ya crea el AppRegistro automáticamente)
                var applicationDto = await _applicationService.CreateApplicationAsync(createApplicationDto);

                // Obtener el AppRegistro creado automáticamente
                var appRegistro = await _appRegistroRepository.GetByApplicationIdAsync(applicationDto.nAplicacionesId);
                
                if (appRegistro == null)
                {
                    _logger.LogWarning("No se encontró AppRegistro para la aplicación recién creada ID: {ApplicationId}", applicationDto.nAplicacionesId);
                    return Ok(CreateSuccessResponse(applicationDto, GetClientName(), GetUserName()));
                }

                // Crear respuesta con los datos de la aplicación y los tokens del AppRegistro
                var response = new CreateApplicationResponseDto
                {
                    nAplicacionesId = applicationDto.nAplicacionesId,
                    cAplicacionesNombre = applicationDto.cAplicacionesNombre,
                    cAplicacionesDescripcion = applicationDto.cAplicacionesDescripcion,
                    cAplicacionesCodigo = applicationDto.cAplicacionesCodigo,
                    bAplicacionesEsActiva = applicationDto.bAplicacionesEsActiva,
                    dAplicacionesFechaCreacion = applicationDto.dAplicacionesFechaCreacion,
                    cAppRegistroTokenAcceso = appRegistro.cAppRegistroTokenAcceso,
                    cAppRegistroSecretoApp = appRegistro.cAppRegistroSecretoApp ?? string.Empty,
                    dAppRegistroFechaExpiracion = appRegistro.dAppRegistroFechaExpiracion
                };

                return CreatedAtAction(nameof(GetApplicationById), new { id = applicationDto.nAplicacionesId }, 
                    CreateSuccessResponse(response, GetClientName(), GetUserName()));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Genera un código único para la aplicación basado en el nombre
        /// </summary>
        /// <param name="nombreAplicacion">Nombre de la aplicación</param>
        /// <returns>Código único de aplicación</returns>
        private string GenerarCodigoAplicacion(string nombreAplicacion)
        {
            // Limpiar el nombre y crear un código único
            var codigoBase = nombreAplicacion
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("_", "")
                .ToUpper();

            // Limitar a 10 caracteres y agregar timestamp para unicidad
            if (codigoBase.Length > 10)
                codigoBase = codigoBase.Substring(0, 10);

            var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            return $"{codigoBase}_{timestamp}";
        }

        /// <summary>
        /// Actualiza una aplicación existente
        /// </summary>
        /// <param name="id">ID de la aplicación</param>
        /// <param name="updateApplicationDto">Datos actualizados de la aplicación</param>
        /// <returns>Aplicación actualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApplication(string id, [FromBody] UpdateApplicationDto updateApplicationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
                }

                if (!int.TryParse(id, out int applicationId))
                {
                    return BadRequest(CreateErrorResponse("ID de aplicación inválido", GetClientName(), GetUserName()));
                }

                var applicationDto = await _applicationService.UpdateApplicationAsync(applicationId, updateApplicationDto);
                if (applicationDto == null)
                {
                    return NotFound(CreateErrorResponse("Aplicación no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(applicationDto, GetClientName(), GetUserName()));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(CreateErrorResponse(ex.Message, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Elimina una aplicación
        /// </summary>
        /// <param name="id">ID de la aplicación</param>
        /// <returns>Resultado de la operación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApplication(string id)
        {
            try
            {
                if (!int.TryParse(id, out int applicationId))
                {
                    return BadRequest(CreateErrorResponse("ID de aplicación inválido", GetClientName(), GetUserName()));
                }

                var result = await _applicationService.DeleteApplicationAsync(applicationId);
                if (!result)
                {
                    return NotFound(CreateErrorResponse("Aplicación no encontrada", GetClientName(), GetUserName()));
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Obtiene la configuración de una aplicación
        /// </summary>
        /// <param name="id">ID de la aplicación</param>
        /// <returns>Configuración de la aplicación</returns>
        [HttpGet("{id}/configuration")]
        public async Task<IActionResult> GetApplicationConfiguration(string id)
        {
            try
            {
                if (!int.TryParse(id, out int applicationId))
                {
                    return BadRequest(CreateErrorResponse("ID de aplicación inválido", GetClientName(), GetUserName()));
                }

                var configurationDto = await _applicationService.GetApplicationConfigurationAsync(applicationId);
                if (configurationDto == null)
                {
                    return NotFound(CreateErrorResponse("Configuración no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(configurationDto, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }

        /// <summary>
        /// Actualiza la configuración de una aplicación
        /// </summary>
        /// <param name="id">ID de la aplicación</param>
        /// <param name="configurationDto">Nueva configuración</param>
        /// <returns>Configuración actualizada</returns>
        [HttpPut("{id}/configuration")]
        public async Task<IActionResult> UpdateApplicationConfiguration(string id, [FromBody] ConfiguracionAplicacionDto configurationDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
                }

                if (!int.TryParse(id, out int applicationId))
                {
                    return BadRequest(CreateErrorResponse("ID de aplicación inválido", GetClientName(), GetUserName()));
                }

                var updatedConfigDto = await _applicationService.UpdateApplicationConfigurationAsync(applicationId, configurationDto);
                if (updatedConfigDto == null)
                {
                    return NotFound(CreateErrorResponse("Aplicación no encontrada", GetClientName(), GetUserName()));
                }

                return Ok(CreateSuccessResponse(updatedConfigDto, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }


    }
}