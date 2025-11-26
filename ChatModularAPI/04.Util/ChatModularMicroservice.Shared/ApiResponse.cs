namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Modelo de respuesta compatible con el frontend
/// </summary>
/// <typeparam name="T">Tipo de datos que contiene la respuesta</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// Datos de la respuesta
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// Mensaje de la respuesta
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool Success { get; set; }

    /// <summary>
    /// Lista de errores
    /// </summary>
    public List<string> Errors { get; set; } = new();

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public ApiResponse()
    {
    }

    /// <summary>
    /// Constructor para respuesta exitosa
    /// </summary>
    /// <param name="data">Datos a devolver</param>
    /// <param name="message">Mensaje opcional</param>
    public ApiResponse(T data, string message = "")
    {
        Data = data;
        Message = message;
        Success = true;
    }

    /// <summary>
    /// Constructor para respuesta de error
    /// </summary>
    /// <param name="message">Mensaje de error</param>
    /// <param name="errors">Lista de errores</param>
    public ApiResponse(string message, List<string>? errors = null)
    {
        Message = message;
        Success = false;
        Errors = errors ?? new List<string>();
    }

    /// <summary>
    /// Método estático para crear respuesta exitosa
    /// </summary>
    /// <param name="data">Datos</param>
    /// <param name="message">Mensaje</param>
    /// <returns>ApiResponse exitoso</returns>
    public static ApiResponse<T> SuccessResponse(T data, string message = "Operación exitosa")
    {
        return new ApiResponse<T>(data, message);
    }

    /// <summary>
    /// Método estático para crear respuesta de error
    /// </summary>
    /// <param name="message">Mensaje de error</param>
    /// <param name="errors">Lista de errores</param>
    /// <returns>ApiResponse con error</returns>
    public static ApiResponse<T> ErrorResponse(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>(message, errors);
    }
}