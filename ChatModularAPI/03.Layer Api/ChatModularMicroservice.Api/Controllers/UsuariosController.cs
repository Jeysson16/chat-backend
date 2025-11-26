using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Domain;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador para la gestión de usuarios
/// </summary>
[ApiController]
[Route("api/v1/usuarios")]
[Produces("application/json")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;
    private readonly ILogger<UsuariosController> _logger;

    public UsuariosController(IUsuarioService usuarioService, ILogger<UsuariosController> logger)
    {
        _usuarioService = usuarioService;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los usuarios
    /// </summary>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerTodos()
    {
        try
        {
            var usuarios = await _usuarioService.ObtenerTodosAsync();
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un usuario por su ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorId(Guid id)
    {
        try
        {
            var usuario = await _usuarioService.ObtenerPorIdAsync(id);
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por ID: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un usuario por su código de usuario
    /// </summary>
    /// <param name="codigo">Código del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("codigo/{codigo}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorCodigo(string codigo)
    {
        try
        {
            var usuario = await _usuarioService.ObtenerPorCodigoUsuarioAsync(codigo);
            if (usuario == null)
            {
                return NotFound($"Usuario con código {codigo} no encontrado");
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por código: {Codigo}", codigo);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un usuario por su email
    /// </summary>
    /// <param name="email">Email del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("email/{email}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorEmail(string email)
    {
        try
        {
            var usuario = await _usuarioService.ObtenerPorEmailAsync(email);
            if (usuario == null)
            {
                return NotFound($"Usuario con email {email} no encontrado");
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por email: {Email}", email);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene un usuario por su username
    /// </summary>
    /// <param name="username">Username del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("username/{username}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorUsername(string username)
    {
        try
        {
            var usuario = await _usuarioService.ObtenerPorUsernameAsync(username);
            if (usuario == null)
            {
                return NotFound($"Usuario con username {username} no encontrado");
            }
            return Ok(usuario);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por username: {Username}", username);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene usuarios por código de aplicación
    /// </summary>
    /// <param name="appCodigo">Código de la aplicación</param>
    /// <returns>Lista de usuarios de la aplicación</returns>
    [HttpGet("aplicacion/{appCodigo}")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerPorAplicacion(string appCodigo)
    {
        try
        {
            var usuarios = await _usuarioService.ObtenerPorAplicacionAsync(appCodigo);
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios por aplicación: {AppCodigo}", appCodigo);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene usuarios activos con paginación
    /// </summary>
    /// <param name="pagina">Número de página (por defecto 1)</param>
    /// <param name="tamanoPagina">Tamaño de página (por defecto 10)</param>
    /// <returns>Lista paginada de usuarios activos</returns>
    [HttpGet("activos")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> ObtenerActivos(
        [FromQuery] int pagina = 1,
        [FromQuery] int tamanoPagina = 10)
    {
        try
        {
            var usuarios = await _usuarioService.ObtenerActivosAsync(pagina, tamanoPagina);
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Busca usuarios según criterios específicos
    /// </summary>
    /// <param name="buscarDto">Criterios de búsqueda</param>
    /// <returns>Lista de usuarios que coinciden con los criterios</returns>
    [HttpPost("buscar")]
    [ProducesResponseType(typeof(IEnumerable<UsuarioDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<IEnumerable<UsuarioDto>>> BuscarUsuarios([FromBody] BuscarUsuarioDto buscarDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuarios = await _usuarioService.BuscarUsuariosAsync(buscarDto);
            return Ok(usuarios);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al buscar usuarios");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="createDto">Datos del usuario a crear</param>
    /// <returns>Usuario creado</returns>
    [HttpPost]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> Crear([FromBody] CreateUsuarioDto createDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioService.CrearAsync(createDto);
            return CreatedAtAction(nameof(ObtenerPorId), new { id = usuario.nUsuariosId }, usuario);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflicto al crear usuario");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear usuario");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="updateDto">Datos a actualizar</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPut("{id:guid}")]
    [ProducesResponseType(typeof(UsuarioDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioDto>> Actualizar(Guid id, [FromBody] UpdateUsuarioDto updateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _usuarioService.ActualizarAsync(id, updateDto);
            if (usuario == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(usuario);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflicto al actualizar usuario");
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Elimina un usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Eliminar(Guid id)
    {
        try
        {
            var resultado = await _usuarioService.EliminarAsync(id);
            if (!resultado)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza la última conexión del usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPatch("{id:guid}/ultima-conexion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarUltimaConexion(Guid id)
    {
        try
        {
            var resultado = await _usuarioService.ActualizarUltimaConexionAsync(id);
            if (!resultado)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(new { mensaje = "Última conexión actualizada correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar última conexión del usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Actualiza el estado de conexión del usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="estadoDto">Estado de conexión</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPatch("{id:guid}/estado-conexion")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ActualizarEstadoConexion(Guid id, [FromBody] UpdateEstadoConexionDto estadoDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _usuarioService.ActualizarEstadoConexionAsync(id, estadoDto);
            if (!resultado)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(new { mensaje = "Estado de conexión actualizado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado de conexión del usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Establece el estado activo del usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="estaActivo">Estado activo</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPatch("{id:guid}/estado-activo")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EstablecerEstadoActivo(Guid id, [FromBody] bool estaActivo)
    {
        try
        {
            var resultado = await _usuarioService.EstablecerEstadoActivoAsync(id, estaActivo);
            if (!resultado)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(new { mensaje = "Estado activo actualizado correctamente" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al establecer estado activo del usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Cambia la contraseña del usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="cambiarPasswordDto">Datos para cambiar contraseña</param>
    /// <returns>Resultado de la operación</returns>
    [HttpPatch("{id:guid}/cambiar-password")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CambiarPassword(Guid id, [FromBody] CambiarPasswordDto cambiarPasswordDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var resultado = await _usuarioService.CambiarPasswordAsync(id, cambiarPasswordDto);
            if (!resultado)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(new { mensaje = "Contraseña cambiada correctamente" });
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Intento de cambio de contraseña no autorizado para usuario: {Id}", id);
            return Unauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al cambiar contraseña del usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica si existe un usuario con el email especificado
    /// </summary>
    /// <param name="email">Email a verificar</param>
    /// <returns>True si existe</returns>
    [HttpGet("existe/email/{email}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ExistePorEmail(string email)
    {
        try
        {
            var existe = await _usuarioService.ExistePorEmailAsync(email);
            return Ok(existe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia por email: {Email}", email);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica si existe un usuario con el username especificado
    /// </summary>
    /// <param name="username">Username a verificar</param>
    /// <returns>True si existe</returns>
    [HttpGet("existe/username/{username}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ExistePorUsername(string username)
    {
        try
        {
            var existe = await _usuarioService.ExistePorUsernameAsync(username);
            return Ok(existe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia por username: {Username}", username);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Verifica si existe un usuario con el código especificado
    /// </summary>
    /// <param name="codigo">Código del usuario a verificar</param>
    /// <returns>True si existe</returns>
    [HttpGet("existe/codigo/{codigo}")]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<bool>> ExistePorCodigo(string codigo)
    {
        try
        {
            var existe = await _usuarioService.ExistePorCodigoUsuarioAsync(codigo);
            return Ok(existe);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia por código: {Codigo}", codigo);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene estadísticas del usuario
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Estadísticas del usuario</returns>
    [HttpGet("{id:guid}/estadisticas")]
    [ProducesResponseType(typeof(UsuarioEstadisticasDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<UsuarioEstadisticasDto>> ObtenerEstadisticas(Guid id)
    {
        try
        {
            var estadisticas = await _usuarioService.ObtenerEstadisticasAsync(id);
            if (estadisticas == null)
            {
                return NotFound($"Usuario con ID {id} no encontrado");
            }

            return Ok(estadisticas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estadísticas del usuario: {Id}", id);
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene el total de usuarios
    /// </summary>
    /// <returns>Número total de usuarios</returns>
    [HttpGet("estadisticas/total")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> ObtenerTotalUsuarios()
    {
        try
        {
            var total = await _usuarioService.ObtenerTotalUsuariosAsync();
            return Ok(total);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total de usuarios");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene el total de usuarios activos
    /// </summary>
    /// <returns>Número total de usuarios activos</returns>
    [HttpGet("estadisticas/activos")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> ObtenerTotalUsuariosActivos()
    {
        try
        {
            var total = await _usuarioService.ObtenerTotalUsuariosActivosAsync();
            return Ok(total);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total de usuarios activos");
            return StatusCode(500, "Error interno del servidor");
        }
    }

    /// <summary>
    /// Obtiene el total de usuarios en línea
    /// </summary>
    /// <returns>Número total de usuarios en línea</returns>
    [HttpGet("estadisticas/en-linea")]
    [ProducesResponseType(typeof(int), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<int>> ObtenerTotalUsuariosEnLinea()
    {
        try
        {
            var total = await _usuarioService.ObtenerTotalUsuariosEnLineaAsync();
            return Ok(total);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener total de usuarios en línea");
            return StatusCode(500, "Error interno del servidor");
        }
    }
}