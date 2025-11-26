namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Clase de respuesta para operaciones que devuelven un solo elemento o lista
/// Compatibiliza con usos existentes en infraestructura (propiedades estilo ResponseBase)
/// </summary>
public class ItemResponseDT
{
    // Propiedades estilo ResponseBase
    public string clientName { get; set; } = string.Empty;
    public bool isSuccess { get; set; } = true;
    public List<string> lstError { get; set; } = new();
    public List<object>? lstItem { get; set; } = new();
    public object? pagination { get; set; }
    public object? resultado { get; set; }
    public string serverName { get; set; } = Environment.MachineName;
    public string ticket { get; set; } = Guid.NewGuid().ToString();
    public string userName { get; set; } = string.Empty;
    public List<string> warnings { get; set; } = new();

    // Propiedad para item único (compatibilidad con dominio)
    public object? Item { get; set; }

    // Propiedades de error simplificadas
    public bool IsSuccess { get; set; } = true;
    public string? ErrorMessage { get; set; }

    public ItemResponseDT()
    {
        serverName = Environment.MachineName;
        ticket = Guid.NewGuid().ToString();
        isSuccess = true;
        IsSuccess = true;
        lstItem = new List<object>();
    }

    public ItemResponseDT(object item) : this()
    {
        Item = item;
        // También exponer como lista para compatibilidad
        lstItem = new List<object> { item };
    }

    public ItemResponseDT(string errorMessage) : this()
    {
        ErrorMessage = errorMessage;
        isSuccess = false;
        IsSuccess = false;
        lstError.Add(errorMessage);
    }
}