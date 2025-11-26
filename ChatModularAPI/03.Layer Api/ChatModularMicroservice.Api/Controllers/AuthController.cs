using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Entities;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador de autenticación y gestión de tokens
/// </summary>
[ApiController]
[Route("api/v1/auth")]
public class AuthController : BaseController
{
    private readonly ITokenService _tokenService;
    private readonly IAuthService _authService;

    public AuthController(ILogger<AuthController> logger, ITokenService tokenService, IAuthService authService) : base(logger)
    {
        _tokenService = tokenService;
        _authService = authService;
    }

    /// <summary>
    /// Endpoint básico de autenticación
    /// </summary>
    [HttpGet("status")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAuthStatus()
    {
        try
        {
            var statusData = new { Status = "Active", Timestamp = DateTime.UtcNow };
            return Ok(CreateSuccessResponse(statusData, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener estado de autenticación");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Login de usuario
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var result = await _authService.LoginAsync(loginDto);
            
            if (result.IsSuccess)
            {
                // El dominio retorna el objeto en la propiedad Item
                return Ok(CreateSuccessResponse(result.Item, GetClientName(), GetUserName()));
            }
            else
            {
                return Unauthorized(CreateErrorResponse(result.ErrorMessage ?? "Credenciales inválidas", GetClientName(), GetUserName()));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en login para usuario: {UserCode}", loginDto?.cUsuarioCodigo);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Cierra la sesión del usuario actual
    /// </summary>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized(CreateErrorResponse("Usuario no autenticado", GetClientName(), GetUserName()));
            }

            var result = await _authService.LogoutAsync(userId);

            if (result.IsSuccess)
            {
                return Ok(CreateSuccessResponse(GetClientName(), GetUserName()));
            }

            return BadRequest(CreateErrorResponse(result.ErrorMessage ?? "Error al cerrar sesión", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en logout para usuario: {UserId}", GetCurrentUserId());
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Registro de usuario
    /// </summary>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var result = await _authService.RegisterAsync(registerDto);
            
            if (result.IsSuccess)
            {
                // El dominio retorna el objeto en la propiedad Item
                return Ok(CreateSuccessResponse(result.Item, GetClientName(), GetUserName()));
            }
            else
            {
                return BadRequest(CreateErrorResponse(result.ErrorMessage ?? "Error en el registro", GetClientName(), GetUserName()));
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error en registro para usuario: {Email}", registerDto?.cUsuariosEmail ?? "desconocido");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtiene la información del usuario actual
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetCurrentUser()
    {
        try
        {
            var token = GetCurrentToken();
            if (string.IsNullOrWhiteSpace(token))
            {
                return Unauthorized(CreateErrorResponse("Token no encontrado", GetClientName(), GetUserName()));
            }

            var result = await _authService.GetUserByTokenAsync(token);

            if (result.IsSuccess)
            {
                return Ok(CreateSuccessResponse(result.Item, GetClientName(), GetUserName()));
            }

            return BadRequest(CreateErrorResponse(result.ErrorMessage ?? "Error al obtener usuario", GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario actual");
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Generar tokens de acceso y secreto para una aplicación
    /// </summary>
    [HttpPost("generar-tokens-aplicacion")]
    [AllowAnonymous]
    public async Task<IActionResult> GenerarTokensAplicacion([FromBody] GenerarTokensRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var tokens = await _tokenService.GenerarTokensAplicacionAsync(request);
            
            return Ok(CreateSuccessResponse(tokens, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generando tokens para aplicación: {CodigoAplicacion}", request.cCodigoAplicacion);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Validar access token y secret token de una aplicación
    /// </summary>
    [HttpPost("validar-access-token")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidarAccessToken([FromBody] ValidarTokenRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var validacion = await _tokenService.ValidarTokensAplicacionAsync(request);
            
            return Ok(CreateSuccessResponse(validacion, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validando access token para aplicación: {CodigoAplicacion}", request.cCodigoAplicacion);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Renovar tokens de una aplicación
    /// </summary>
    [HttpPost("renovar-tokens")]
    [AllowAnonymous]
    public async Task<IActionResult> RenovarTokens([FromBody] RenovarTokensRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var nuevosTokens = await _tokenService.RenovarTokensAplicacionAsync(request);
            
            return Ok(CreateSuccessResponse(nuevosTokens, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error renovando tokens para aplicación: {CodigoAplicacion}", request.cCodigoAplicacion);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Revocar tokens de una aplicación
    /// </summary>
    [HttpPost("revocar-tokens")]
    [AllowAnonymous]
    public async Task<IActionResult> RevocarTokens([FromBody] ChatModularMicroservice.Entities.DTOs.RevocarTokensRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(CreateErrorResponse("Datos de entrada inválidos", GetClientName(), GetUserName()));
            }

            var resultado = await _tokenService.RevocarTokensAplicacionAsync(request);
            
            return Ok(CreateSuccessResponse(resultado, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error revocando tokens para aplicación: {CodigoAplicacion}", request.cCodigoAplicacion);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }

    /// <summary>
    /// Obtener tokens activos de una aplicación
    /// </summary>
    [HttpGet("tokens-aplicacion/{codigoApp}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetTokensAplicacion(string codigoApp)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(codigoApp))
            {
                return BadRequest(CreateErrorResponse("Código de aplicación requerido", GetClientName(), GetUserName()));
            }

            var tokens = await _tokenService.GetTokensActivosAplicacionAsync(codigoApp);
            
            return Ok(CreateSuccessResponse(tokens, GetClientName(), GetUserName()));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error obteniendo tokens activos para aplicación: {CodigoApp}", codigoApp);
            return HandleException(ex, GetClientName(), GetUserName());
        }
    }
}
