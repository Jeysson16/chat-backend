namespace ChatModularMicroservice.Entities;

/// <summary>
/// Enumeración para los tipos de filtro de token (item individual)
/// </summary>
public enum TokenFilterItemType
{
    /// <summary>
    /// Filtrar por ID de token
    /// </summary>
    TokenId = 1,
    
    /// <summary>
    /// Filtrar por token de acceso
    /// </summary>
    TokenAcceso = 2,
    
    /// <summary>
    /// Filtrar por código de aplicación
    /// </summary>
    CodigoAplicacion = 3,
    
    /// <summary>
    /// Filtrar por estado activo
    /// </summary>
    TokenEsActivo = 4,
    
    /// <summary>
    /// Filtrar por fecha de expiración
    /// </summary>
    FechaExpiracion = 5
}

/// <summary>
/// Enumeración para los tipos de filtro de token (lista)
/// </summary>
public enum TokenFilterListType
{
    /// <summary>
    /// Obtener todos los tokens activos
    /// </summary>
    TokensActivos = 1,
    
    /// <summary>
    /// Obtener todos los tokens
    /// </summary>
    TodosTokens = 2,
    
    /// <summary>
    /// Filtrar tokens por aplicación
    /// </summary>
    TokensPorAplicacion = 3,
    
    /// <summary>
    /// Filtrar tokens expirados
    /// </summary>
    TokensExpirados = 4,
    
    /// <summary>
    /// Filtrar tokens próximos a expirar
    /// </summary>
    TokensProximosExpirar = 5
}

/// <summary>
/// Clase para filtros de token
/// </summary>
public class TokenFilter
{
    /// <summary>
    /// ID del token
    /// </summary>
    public Guid? TokenId { get; set; }
    
    /// <summary>
    /// Token de acceso
    /// </summary>
    public string? TokenAcceso { get; set; }
    
    /// <summary>
    /// Código de la aplicación
    /// </summary>
    public string? CodigoAplicacion { get; set; }
    
    /// <summary>
    /// Estado activo del token
    /// </summary>
    public bool? TokenEsActivo { get; set; }
    
    /// <summary>
    /// Fecha de expiración desde
    /// </summary>
    public DateTime? FechaExpiracionDesde { get; set; }
    
    /// <summary>
    /// Fecha de expiración hasta
    /// </summary>
    public DateTime? FechaExpiracionHasta { get; set; }
    
    /// <summary>
    /// Días para expirar (para tokens próximos a expirar)
    /// </summary>
    public int? DiasParaExpirar { get; set; }
}