namespace ChatModularMicroservice.Shared.Utils;

/// <summary>
/// Clase para manejar información de paginación
/// </summary>
public class Pagination
{
    /// <summary>
    /// Página actual (base 1)
    /// </summary>
    public int currentPage { get; set; } = 1;

    /// <summary>
    /// Número de elementos por página
    /// </summary>
    public int pageSize { get; set; } = 10;

    /// <summary>
    /// Total de elementos disponibles
    /// </summary>
    public int totalItems { get; set; } = 0;

    /// <summary>
    /// Total de páginas calculado
    /// </summary>
    public int totalPages => (int)Math.Ceiling((double)totalItems / pageSize);

    /// <summary>
    /// Indica si hay página anterior
    /// </summary>
    public bool hasPrevious => currentPage > 1;

    /// <summary>
    /// Indica si hay página siguiente
    /// </summary>
    public bool hasNext => currentPage < totalPages;

    /// <summary>
    /// Número de elementos a saltar para la consulta
    /// </summary>
    public int skip => (currentPage - 1) * pageSize;

    /// <summary>
    /// Alias para compatibilidad con código existente
    /// </summary>
    public int PageNumber 
    { 
        get => currentPage; 
        set => currentPage = value; 
    }

    /// <summary>
    /// Alias para compatibilidad con código existente
    /// </summary>
    public int PageSize 
    { 
        get => pageSize; 
        set => pageSize = value; 
    }

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public Pagination() { }

    /// <summary>
    /// Constructor con parámetros
    /// </summary>
    /// <param name="currentPage">Página actual</param>
    /// <param name="pageSize">Tamaño de página</param>
    /// <param name="totalItems">Total de elementos</param>
    public Pagination(int currentPage, int pageSize, int totalItems)
    {
        this.currentPage = Math.Max(1, currentPage);
        this.pageSize = Math.Max(1, pageSize);
        this.totalItems = Math.Max(0, totalItems);
    }

    /// <summary>
    /// Valida que los parámetros de paginación sean correctos
    /// </summary>
    /// <returns>True si son válidos</returns>
    public bool IsValid()
    {
        return currentPage > 0 && pageSize > 0 && totalItems >= 0;
    }
}