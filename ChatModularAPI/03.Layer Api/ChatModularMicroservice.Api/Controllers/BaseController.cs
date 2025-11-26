using ChatModularMicroservice.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Api.Controllers;

/// <summary>
/// Controlador base con funcionalidades comunes
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class BaseController : ControllerBase
{
    protected readonly ILogger _logger;

    protected BaseController(ILogger logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Crea una respuesta exitosa con datos
    /// </summary>
    /// <typeparam name="T">Tipo de datos</typeparam>
    /// <param name="data">Datos a devolver</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta exitosa</returns>
    protected ItemResponseDTLst CreateSuccessResponse<T>(List<T> data, string clientName = "", string userName = "")
    {
        var response = new ItemResponseDTLst
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = true,
            LstItem = data.Cast<object>()
        };
        return response;
    }

    /// <summary>
    /// Crea una respuesta exitosa con un solo elemento
    /// </summary>
    /// <typeparam name="T">Tipo de datos</typeparam>
    /// <param name="data">Elemento a devolver</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta exitosa</returns>
    protected ItemResponseDT CreateSuccessResponse<T>(T data, string clientName = "", string userName = "")
    {
        var response = new ItemResponseDT
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = true,
            Item = data
        };
        return response;
    }

    /// <summary>
    /// Crea una respuesta exitosa sin datos
    /// </summary>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta exitosa</returns>
    protected BoolItemResponse CreateSuccessResponse(string clientName = "", string userName = "")
    {
        var response = new BoolItemResponse
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = true,
            Item = true
        };
        return response;
    }

    /// <summary>
    /// Crea una respuesta de error
    /// </summary>
    /// <param name="error">Mensaje de error</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta de error</returns>
    protected BadRequestResponse CreateErrorResponse(string error, string clientName = "", string userName = "")
    {
        var response = new BadRequestResponse
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = false,
            Item = null
        };
        response.LstError.Add(new EResponse { cDescripcion = error });
        return response;
    }

    /// <summary>
    /// Crea una respuesta de error con múltiples errores
    /// </summary>
    /// <param name="errors">Lista de errores</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta de error</returns>
    protected BadRequestResponse CreateErrorResponse(List<string> errors, string clientName = "", string userName = "")
    {
        var response = new BadRequestResponse
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = false,
            Item = null
        };
        foreach (var error in errors)
        {
            response.LstError.Add(new EResponse { cDescripcion = error });
        }
        return response;
    }

    /// <summary>
    /// Crea una respuesta paginada
    /// </summary>
    /// <typeparam name="T">Tipo de datos</typeparam>
    /// <param name="data">Datos a devolver</param>
    /// <param name="pagination">Información de paginación</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta paginada</returns>
    protected ItemResponseDTLst CreateSuccessResponseWithPagination<T>(List<T> data, object pagination, string clientName = "", string userName = "")
    {
        var response = new ItemResponseDTLst
        {
            ClientName = clientName,
            UserName = userName,
            ServerName = Environment.MachineName,
            IsSuccess = true,
            LstItem = data.Cast<object>(),
            // Convertir a tipo de paginación del proyecto de Entities si coincide
            Pagination = pagination as ChatModularMicroservice.Entities.Pagination
        };
        return response;
    }

    /// <summary>
    /// Maneja excepciones y devuelve la respuesta apropiada
    /// </summary>
    /// <param name="ex">Excepción a manejar</param>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    /// <returns>Respuesta de error apropiada</returns>
    protected IActionResult HandleException(Exception ex, string clientName = "", string userName = "")
    {
        _logger.LogError(ex, "Error en controlador: {Message}", ex.Message);

        return ex switch
        {
            ArgumentException argEx => BadRequest(CreateErrorResponse(argEx.Message, clientName, userName)),
            UnauthorizedAccessException unauthorizedEx => Unauthorized(CreateErrorResponse(unauthorizedEx.Message, clientName, userName)),
            InvalidOperationException invalidOpEx => Conflict(CreateErrorResponse(invalidOpEx.Message, clientName, userName)),
            _ => StatusCode(500, CreateErrorResponse("Error interno del servidor", clientName, userName))
        };
    }

    /// <summary>
    /// Obtiene información del cliente desde los headers
    /// </summary>
    /// <returns>Nombre del cliente</returns>
    protected string GetClientName()
    {
        return Request.Headers["X-Client-Name"].FirstOrDefault() ?? "Unknown";
    }

    /// <summary>
    /// Obtiene información del usuario desde los claims
    /// </summary>
    /// <returns>Nombre del usuario</returns>
    protected string GetUserName()
    {
        return User?.Identity?.Name ?? "Anonymous";
    }

    /// <summary>
    /// Obtiene el ticket de tracking desde los headers
    /// </summary>
    /// <returns>Ticket de tracking</returns>
    protected string GetTrackingTicket()
    {
        return Request.Headers["X-Tracking-Ticket"].FirstOrDefault() ?? Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Obtiene el token de autorización desde los headers
    /// </summary>
    /// <returns>Token de autorización</returns>
    protected string GetCurrentToken()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        return string.Empty;
    }

    /// <summary>
    /// Obtiene el ID del usuario actual desde los claims
    /// </summary>
    /// <returns>ID del usuario</returns>
    protected string GetCurrentUserId()
    {
        return User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Obtiene el rol del usuario actual desde los claims
    /// </summary>
    /// <returns>Rol del usuario</returns>
    protected string GetCurrentUserRole()
    {
        return User.FindFirst(ClaimTypes.Role)?.Value ?? string.Empty;
    }

    /// <summary>
    /// Obtiene el código de persona del usuario actual desde los claims
    /// </summary>
    /// <returns>Código de persona</returns>
    protected string GetCurrentUserPerCodigo()
    {
        return User.FindFirst("per_codigo")?.Value ?? string.Empty;
    }

    /// <summary>
    /// Obtiene el ID de la empresa actual desde los claims o headers
    /// </summary>
    /// <returns>ID de la empresa</returns>
    protected string GetCurrentEmpresaId()
    {
        // Primero intentar obtener desde claims
        var empresaIdFromClaims = User.FindFirst("empresa_id")?.Value;
        if (!string.IsNullOrEmpty(empresaIdFromClaims))
            return empresaIdFromClaims;

        // Si no está en claims, intentar desde headers
        var empresaIdFromHeaders = Request.Headers["X-Empresa-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(empresaIdFromHeaders))
            return empresaIdFromHeaders;

        // Valor por defecto
        return "1";
    }

    /// <summary>
    /// Obtiene el ID de la aplicación actual desde los claims o headers
    /// </summary>
    /// <returns>ID de la aplicación</returns>
    protected string GetCurrentAplicacionId()
    {
        // Primero intentar obtener desde claims
        var aplicacionIdFromClaims = User.FindFirst("aplicacion_id")?.Value;
        if (!string.IsNullOrEmpty(aplicacionIdFromClaims))
            return aplicacionIdFromClaims;

        // Si no está en claims, intentar desde headers
        var aplicacionIdFromHeaders = Request.Headers["X-Aplicacion-Id"].FirstOrDefault();
        if (!string.IsNullOrEmpty(aplicacionIdFromHeaders))
            return aplicacionIdFromHeaders;

        // Valor por defecto
        return "1";
    }
}