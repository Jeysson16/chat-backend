using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using System.Security.Claims;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador para la gestión de personas
/// </summary>
[ApiController]
[Route("api/v1/personas")]
[Authorize]
public class PersonaController : BaseController
{
    private readonly IPersonaService _personaService;

    public PersonaController(IPersonaService personaService, ILogger<PersonaController> logger) : base(logger)
    {
        _personaService = personaService;
    }

    /// <summary>
    /// Obtiene una persona por su código
    /// </summary>
    /// <param name="personaCodigo">Código de la persona</param>
    /// <returns>ResponseBase con los datos de la persona</returns>
    [HttpGet("{personaCodigo}")]
    public async Task<IActionResult> GetPersona(string personaCodigo)
    {
        try
        {
            var response = await _personaService.GetPersonaByCodigoAsync(personaCodigo);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return NotFound(CreateErrorResponse($"Persona no encontrada: {personaCodigo}", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene todas las personas activas con paginación
    /// </summary>
    /// <param name="page">Número de página (default: 1)</param>
    /// <param name="pageSize">Tamaño de página (default: 20, max: 100)</param>
    /// <returns>ResponseBase con lista paginada de personas</returns>
    [HttpGet]
    public async Task<IActionResult> GetPersonasActivas(
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var response = await _personaService.GetPersonasActivasAsync(page, pageSize);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponseWithPagination(response.data, response.pagination, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al obtener personas activas", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Busca personas por criterios específicos
    /// </summary>
    /// <param name="searchCriteria">Criterios de búsqueda</param>
    /// <returns>ResponseBase con lista de personas encontradas</returns>
    [HttpPost("search")]
    public async Task<IActionResult> BuscarPersonas([FromBody] BuscarPersonaDto searchCriteria)
    {
        try
        {
            var response = await _personaService.BuscarPersonasAsync(searchCriteria);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al buscar personas", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza el perfil de una persona
    /// </summary>
    /// <param name="personaCodigo">Código de la persona</param>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>ResponseBase con los datos actualizados</returns>
    [HttpPut("{personaCodigo}")]
    public async Task<IActionResult> ActualizarPerfil(
        string personaCodigo, 
        [FromBody] ActualizarPerfilDto updateDto)
    {
        try
        {
            // Verificar que el usuario solo pueda actualizar su propio perfil o sea admin
            var currentUserId = GetCurrentUserId();
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserRole != "ADMIN")
            {
                var currentUserPerCodigo = GetCurrentUserPerCodigo();
                if (currentUserPerCodigo != personaCodigo)
                {
                    return StatusCode(403, CreateErrorResponse("No tienes permisos para actualizar este perfil", GetClientName(), GetUserName()));
                }
            }

            var response = await _personaService.ActualizarPerfilAsync(personaCodigo, updateDto);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al actualizar perfil", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar perfil: {PersonaCodigo}", personaCodigo);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza el estado de conexión de una persona
    /// </summary>
    /// <param name="personaCodigo">Código de la persona</param>
    /// <param name="statusDto">Estado de conexión</param>
    /// <returns>ResponseBase con el resultado de la operación</returns>
    [HttpPatch("{personaCodigo}/connection-status")]
    public async Task<IActionResult> ActualizarEstadoConexion(
        string personaCodigo, 
        [FromBody] UpdateConnectionStatusDto statusDto)
    {
        try
        {
            // Verificar que el usuario solo pueda actualizar su propio estado o sea admin
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserRole != "ADMIN")
            {
                var currentUserPerCodigo = GetCurrentUserPerCodigo();
                if (currentUserPerCodigo != personaCodigo)
                {
                    return StatusCode(403, CreateErrorResponse("No tienes permisos para actualizar este estado", GetClientName(), GetUserName()));
                }
            }

            var isOnline = statusDto.Estado.ToLower() == "online";
            var response = await _personaService.ActualizarEstadoConexionAsync(personaCodigo, isOnline);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al actualizar estado de conexión", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado de conexión: {PersonaCodigo}", personaCodigo);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene las configuraciones de privacidad de una persona
    /// </summary>
    /// <param name="personaCodigo">Código de la persona</param>
    /// <returns>ResponseBase con las configuraciones de privacidad</returns>
    [HttpGet("{personaCodigo}/privacy-settings")]
    public async Task<IActionResult> GetConfiguracionPrivacidad(string personaCodigo)
    {
        try
        {
            // Verificar que el usuario solo pueda ver su propia configuración o sea admin
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserRole != "ADMIN")
            {
                var currentUserPerCodigo = GetCurrentUserPerCodigo();
                if (currentUserPerCodigo != personaCodigo)
                {
                    return StatusCode(403, CreateErrorResponse("No tienes permisos para ver esta configuración", GetClientName(), GetUserName()));
                }
            }

            var response = await _personaService.GetConfiguracionPrivacidadAsync(personaCodigo);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return NotFound(CreateErrorResponse("Configuración de privacidad no encontrada", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener configuración de privacidad: {PersonaCodigo}", personaCodigo);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza las configuraciones de privacidad de una persona
    /// </summary>
    /// <param name="personaCodigo">Código de la persona</param>
    /// <param name="privacySettings">Nuevas configuraciones de privacidad</param>
    /// <returns>ResponseBase con el resultado de la operación</returns>
    [HttpPut("{personaCodigo}/privacy-settings")]
    public async Task<IActionResult> ActualizarConfiguracionPrivacidad(
        string personaCodigo, 
        [FromBody] PrivacySettingsDto privacySettings)
    {
        try
        {
            // Verificar que el usuario solo pueda actualizar su propia configuración o sea admin
            var currentUserRole = GetCurrentUserRole();
            
            if (currentUserRole != "ADMIN")
            {
                var currentUserPerCodigo = GetCurrentUserPerCodigo();
                if (currentUserPerCodigo != personaCodigo)
                {
                    return StatusCode(403, CreateErrorResponse("No tienes permisos para actualizar esta configuración", GetClientName(), GetUserName()));
                }
            }

            var response = await _personaService.ActualizarConfiguracionPrivacidadAsync(personaCodigo, privacySettings);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al actualizar configuración de privacidad", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar configuración de privacidad: {PersonaCodigo}", personaCodigo);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene el perfil del usuario actual
    /// </summary>
    /// <returns>ResponseBase con los datos del usuario actual</returns>
    [HttpGet("me")]
    public async Task<IActionResult> GetMiPerfil()
    {
        try
        {
            var currentUserPerCodigo = GetCurrentUserPerCodigo();
            var response = await _personaService.GetPersonaByCodigoAsync(currentUserPerCodigo);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return NotFound(CreateErrorResponse("Mi perfil no encontrado", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener mi perfil");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Actualiza el perfil del usuario actual
    /// </summary>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>ResponseBase con los datos actualizados</returns>
    [HttpPut("me")]
    public async Task<IActionResult> ActualizarMiPerfil([FromBody] ActualizarPerfilDto updateDto)
    {
        try
        {
            var currentUserPerCodigo = GetCurrentUserPerCodigo();
            var response = await _personaService.ActualizarPerfilAsync(currentUserPerCodigo, updateDto);
            
            if (response.isSuccess)
            {
                return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
            }
            
            return BadRequest(CreateErrorResponse("Error al actualizar mi perfil", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar mi perfil");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }
}