using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;
using System.Linq;

namespace ChatModularMicroservice.Api.Controllers
{
    [ApiController]
    [Route("api/v1/contactos")]
    [Authorize]
    public class ContactoController : BaseController
    {
        private readonly IContactoService _contactoService;

        public ContactoController(IContactoService contactoService, ILogger<ContactoController> logger) : base(logger)
        {
            _contactoService = contactoService;
        }

        /// <summary>
        /// Endpoint de prueba para verificar usuarios en tabla Usuarios
        /// </summary>
        [HttpGet("test-usuarios")]
        [AllowAnonymous]
        public async Task<IActionResult> TestUsuarios([FromQuery] string terminoBusqueda = "")
        {
            try
            {
                _logger.LogInformation("=== TEST: Buscando usuarios en tabla Usuarios con término: {Termino}", terminoBusqueda);
                
                // Usar el servicio para buscar usuarios en tabla Usuarios
                var response = await _contactoService.BuscarUsuariosEnTablaUsuariosAsync(terminoBusqueda, "test-user", "1", "1", "1");
                
                _logger.LogInformation("=== TEST: Respuesta del servicio - Success: {Success}, Count: {Count}", 
                    response.isSuccess, response.data?.Count ?? 0);
                
                if (response.isSuccess && response.data != null && response.data.Count > 0)
                {
                    foreach (var usuario in response.data.Take(3))
                    {
                        _logger.LogInformation("=== TEST: Usuario encontrado: ID={Id}, Nombre={Nombre}, Email={Email}", 
                            usuario.cUsuariosChatId, usuario.cUsuariosChatNombre, usuario.cUsuariosChatEmail);
                    }
                }
                
                return Ok(new { 
                    success = response.isSuccess, 
                    count = response.data?.Count ?? 0, 
                    usuarios = response.data,
                    errors = response.lstError,
                    message = response.isSuccess ? "Usuarios encontrados en tabla Usuarios" : "Error al buscar usuarios"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "=== TEST: Error al buscar usuarios: {Error}", ex.Message);
                return Ok(new { 
                    success = false, 
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        /// <summary>
        /// Busca usuarios para agregar como contactos
        /// </summary>
        /// <param name="terminoBusqueda">Término de búsqueda</param>
        /// <param name="tipoListado">Tipo de listado (1=Todos, 2=Solo contactos, 3=No contactos)</param>
        /// <returns>ResponseBase con lista de usuarios encontrados</returns>
        [HttpGet("search-users")]
        public async Task<IActionResult> BuscarUsuarios(
            [FromQuery] string terminoBusqueda, 
            [FromQuery] string tipoListado = "1")
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.BuscarUsuariosAsync(terminoBusqueda, currentUserId, empresaId, aplicacionId, tipoListado);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data ?? new List<ChatUsuarioDto>(), GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al buscar usuarios", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al buscar usuarios: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Envia una solicitud de contacto a un usuario
        /// </summary>
        /// <param name="request">Datos de la solicitud</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpPost("solicitudes")]
        public async Task<IActionResult> EnviarSolicitudContacto([FromBody] EnviarSolicitudRequest request)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.EnviarSolicitudContactoAsync(
                    currentUserId, 
                    request.UsuarioDestinoId, 
                    empresaId, 
                    aplicacionId, 
                    request.Mensaje ?? string.Empty);

                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al enviar solicitud", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar solicitud de contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al enviar solicitud: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Lista las solicitudes de contacto pendientes del usuario actual
        /// </summary>
        /// <returns>ResponseBase con lista de solicitudes pendientes</returns>
        [HttpGet("solicitudes/pendientes")]
        public async Task<IActionResult> ListarSolicitudesPendientes()
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.ListarSolicitudesPendientesAsync(currentUserId, empresaId, aplicacionId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data ?? new List<List<SolicitudContactoDto>>(), GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al listar solicitudes", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar solicitudes pendientes: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al listar solicitudes: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Acepta una solicitud de contacto
        /// </summary>
        /// <param name="contactoId">ID del contacto</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpPut("solicitudes/{contactoId}/aceptar")]
        public async Task<IActionResult> AceptarSolicitudContacto(string contactoId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                var response = await _contactoService.AceptarSolicitudContactoAsync(contactoId, currentUserId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al aceptar solicitud", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al aceptar solicitud de contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al aceptar solicitud: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Rechaza una solicitud de contacto
        /// </summary>
        /// <param name="solicitudId">ID de la solicitud</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpPut("solicitudes/{solicitudId}/rechazar")]
        public async Task<IActionResult> RechazarSolicitudContacto(string solicitudId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.RechazarSolicitudContactoAsync(solicitudId, currentUserId, empresaId, aplicacionId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al rechazar solicitud", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al rechazar solicitud de contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al rechazar solicitud: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Lista los contactos del usuario actual
        /// </summary>
        /// <param name="estado">Estado de los contactos (opcional)</param>
        /// <returns>ResponseBase con lista de contactos</returns>
        [HttpGet("mis-contactos")]
        [HttpGet("my-contacts")]
        public async Task<IActionResult> ListarMisContactos([FromQuery] string? estado = null)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.ListarContactosPorUsuarioAsync(currentUserId, empresaId, aplicacionId, estado);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data ?? new List<List<ContactoDto>>(), GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al listar contactos", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar contactos: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al listar contactos: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Verifica si dos usuarios pueden chatear directamente
        /// </summary>
        /// <param name="usuarioDestinoId">ID del usuario destino</param>
        /// <returns>ResponseBase con resultado de la verificación</returns>
        [HttpGet("permiso-chat-directo/{usuarioDestinoId}")]
        public async Task<IActionResult> VerificarPermisoChatDirecto(string usuarioDestinoId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.VerificarPermisoChatDirectoAsync(currentUserId, usuarioDestinoId, empresaId, aplicacionId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al verificar permiso", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar permiso de chat directo: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al verificar permiso: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Bloquea a un contacto
        /// </summary>
        /// <param name="contactoUsuarioId">ID del usuario contacto a bloquear</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpPut("bloquear/{contactoUsuarioId}")]
        public async Task<IActionResult> BloquearContacto(string contactoUsuarioId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                var response = await _contactoService.BloquearContactoAsync(currentUserId, contactoUsuarioId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al bloquear contacto", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al bloquear contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al bloquear contacto: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Desbloquea a un contacto
        /// </summary>
        /// <param name="contactoUsuarioId">ID del usuario contacto a desbloquear</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpPut("desbloquear/{contactoUsuarioId}")]
        public async Task<IActionResult> DesbloquearContacto(string contactoUsuarioId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();

                var response = await _contactoService.DesbloquearContactoAsync(currentUserId, contactoUsuarioId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al desbloquear contacto", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al desbloquear contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al desbloquear contacto: {ex.Message}", GetClientName(), GetUserName()));
            }
        }

        /// <summary>
        /// Elimina un contacto
        /// </summary>
        /// <param name="contactoUsuarioId">ID del usuario contacto a eliminar</param>
        /// <returns>ResponseBase con resultado de la operación</returns>
        [HttpDelete("eliminar/{contactoUsuarioId}")]
        public async Task<IActionResult> EliminarContacto(string contactoUsuarioId)
        {
            try
            {
                var currentUserId = GetCurrentUserId();
                var empresaId = GetCurrentEmpresaId();
                var aplicacionId = GetCurrentAplicacionId();

                var response = await _contactoService.EliminarContactoAsync(currentUserId, contactoUsuarioId, empresaId, aplicacionId);
                
                if (response.isSuccess)
                {
                    return Ok(CreateSuccessResponse(response.data, GetClientName(), GetUserName()));
                }
                
                return BadRequest(CreateErrorResponse(response.lstError?.FirstOrDefault() ?? "Error al eliminar contacto", GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar contacto: {Error}", ex.Message);
                return BadRequest(CreateErrorResponse($"Error al eliminar contacto: {ex.Message}", GetClientName(), GetUserName()));
            }
        }
    }

    /// <summary>
    /// Request para enviar solicitud de contacto
    /// </summary>
    public class EnviarSolicitudRequest
    {
        public string UsuarioDestinoId { get; set; } = string.Empty;
        public string? Mensaje { get; set; }
    }
}
