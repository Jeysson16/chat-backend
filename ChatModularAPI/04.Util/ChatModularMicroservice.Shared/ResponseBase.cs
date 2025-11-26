namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Modelo de respuesta estándar para todos los endpoints de la API
/// </summary>
/// <typeparam name="T">Tipo de datos que contiene la respuesta</typeparam>
public class ResponseBase<T>
{
    /// <summary>
    /// Nombre del cliente que realiza la petición
    /// </summary>
    public string clientName { get; set; } = string.Empty;

    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool isSuccess { get; set; }

    /// <summary>
    /// Lista de errores ocurridos durante la operación
    /// </summary>
    public List<string> lstError { get; set; } = new();

    /// <summary>
    /// Lista de elementos devueltos por la operación
    /// </summary>
    public List<T>? lstItem { get; set; } = new();

    /// <summary>
    /// Información de paginación cuando aplique
    /// </summary>
    public object? pagination { get; set; }

    /// <summary>
    /// Resultado específico de la operación (para casos especiales)
    /// </summary>
    public object? resultado { get; set; }

    /// <summary>
    /// Nombre del servidor que procesa la petición
    /// </summary>
    public string serverName { get; set; } = Environment.MachineName;

    /// <summary>
    /// Ticket único para rastrear la petición
    /// </summary>
    public string ticket { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Nombre del usuario que realiza la petición
    /// </summary>
    public string userName { get; set; } = string.Empty;

    /// <summary>
    /// Lista de advertencias no críticas
    /// </summary>
    public List<string> warnings { get; set; } = new();

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public ResponseBase()
    {
        serverName = Environment.MachineName;
        ticket = Guid.NewGuid().ToString();
    }

    /// <summary>
    /// Constructor con parámetros básicos
    /// </summary>
    /// <param name="clientName">Nombre del cliente</param>
    /// <param name="userName">Nombre del usuario</param>
    public ResponseBase(string clientName, string userName) : this()
    {
        this.clientName = clientName;
        this.userName = userName;
    }

    /// <summary>
    /// Método para marcar la respuesta como exitosa con datos
    /// </summary>
    /// <param name="items">Lista de elementos a devolver</param>
    public void SetSuccess(List<T> items)
    {
        isSuccess = true;
        lstItem = items;
        lstError.Clear();
    }

    /// <summary>
    /// Método para marcar la respuesta como exitosa con un solo elemento
    /// </summary>
    /// <param name="item">Elemento a devolver</param>
    public void SetSuccess(T item)
    {
        isSuccess = true;
        lstItem = new List<T> { item };
        lstError.Clear();
    }

    /// <summary>
    /// Método para marcar la respuesta como exitosa sin datos
    /// </summary>
    public void SetSuccess()
    {
        isSuccess = true;
        lstError.Clear();
    }

    /// <summary>
    /// Método para marcar la respuesta como fallida
    /// </summary>
    /// <param name="error">Mensaje de error</param>
    public void SetError(string error)
    {
        isSuccess = false;
        lstError.Add(error);
    }

    /// <summary>
    /// Método para marcar la respuesta como fallida con múltiples errores
    /// </summary>
    /// <param name="errors">Lista de errores</param>
    public void SetError(List<string> errors)
    {
        isSuccess = false;
        lstError.AddRange(errors);
    }

    /// <summary>
    /// Método para agregar una advertencia
    /// </summary>
    /// <param name="warning">Mensaje de advertencia</param>
    public void AddWarning(string warning)
    {
        warnings.Add(warning);
    }
}