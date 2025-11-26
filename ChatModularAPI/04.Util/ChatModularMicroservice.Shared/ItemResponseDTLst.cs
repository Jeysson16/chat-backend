namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Clase de respuesta para operaciones que devuelven una lista de elementos
/// </summary>
public class ItemResponseDTLst
{
    /// <summary>
    /// Lista de elementos devueltos por la operación
    /// </summary>
    public List<object>? LstItem { get; set; }

    /// <summary>
    /// Información de paginación
    /// </summary>
    public Pagination? Pagination { get; set; }

    /// <summary>
    /// Indica si la operación fue exitosa
    /// </summary>
    public bool IsSuccess { get; set; } = true;

    /// <summary>
    /// Mensaje de error si la operación falló
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public ItemResponseDTLst()
    {
        LstItem = new List<object>();
        IsSuccess = true;
    }

    /// <summary>
    /// Constructor con lista de elementos
    /// </summary>
    /// <param name="items">Lista de elementos a devolver</param>
    public ItemResponseDTLst(List<object> items)
    {
        LstItem = items;
        IsSuccess = true;
    }

    /// <summary>
    /// Constructor con lista de elementos y paginación
    /// </summary>
    /// <param name="items">Lista de elementos a devolver</param>
    /// <param name="pagination">Información de paginación</param>
    public ItemResponseDTLst(List<object> items, Pagination pagination)
    {
        LstItem = items;
        Pagination = pagination;
        IsSuccess = true;
    }

    /// <summary>
    /// Constructor para error
    /// </summary>
    /// <param name="errorMessage">Mensaje de error</param>
    public ItemResponseDTLst(string errorMessage)
    {
        LstItem = new List<object>();
        IsSuccess = false;
        ErrorMessage = errorMessage;
    }
}