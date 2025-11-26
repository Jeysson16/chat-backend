namespace ChatModularMicroservice.Entities;

/// <summary>
/// Enumeración para los tipos de filtro de configuración (item individual)
/// </summary>
public enum ConfiguracionFilterItemType
{
    /// <summary>
    /// Filtrar por ID de configuración
    /// </summary>
    ConfiguracionId = 1,
    
    /// <summary>
    /// Filtrar por clave de configuración
    /// </summary>
    ConfiguracionClave = 2,
    
    /// <summary>
    /// Filtrar por valor de configuración
    /// </summary>
    ConfiguracionValor = 3,
    
    /// <summary>
    /// Filtrar por tipo de configuración
    /// </summary>
    ConfiguracionTipo = 4,
    
    /// <summary>
    /// Filtrar por estado activo
    /// </summary>
    ConfiguracionEsActiva = 5
}

/// <summary>
/// Enumeración para los tipos de filtro de configuración (lista)
/// </summary>
public enum ConfiguracionFilterListType
{
    /// <summary>
    /// Obtener todas las configuraciones activas
    /// </summary>
    ConfiguracionesActivas = 1,
    
    /// <summary>
    /// Obtener todas las configuraciones
    /// </summary>
    TodasConfiguraciones = 2,
    
    /// <summary>
    /// Filtrar configuraciones por tipo
    /// </summary>
    ConfiguracionesPorTipo = 3,
    
    /// <summary>
    /// Filtrar configuraciones por aplicación
    /// </summary>
    ConfiguracionesPorAplicacion = 4,
    
    /// <summary>
    /// Filtrar configuraciones por empresa
    /// </summary>
    ConfiguracionesPorEmpresa = 5
}

/// <summary>
/// Clase para filtros de configuración
/// </summary>
public class ConfiguracionFilter
{
    /// <summary>
    /// ID de la configuración
    /// </summary>
    public int? ConfiguracionId { get; set; }
    
    /// <summary>
    /// Clave de configuración
    /// </summary>
    public string? ConfiguracionClave { get; set; }
    
    /// <summary>
    /// Valor de configuración
    /// </summary>
    public string? ConfiguracionValor { get; set; }
    
    /// <summary>
    /// Tipo de configuración
    /// </summary>
    public string? ConfiguracionTipo { get; set; }
    
    /// <summary>
    /// Estado activo de la configuración
    /// </summary>
    public bool? ConfiguracionEsActiva { get; set; }
    
    /// <summary>
    /// ID de aplicación asociada
    /// </summary>
    public int? AplicacionId { get; set; }
    
    /// <summary>
    /// ID de empresa asociada
    /// </summary>
    public int? EmpresaId { get; set; }
    
    /// <summary>
    /// Término de búsqueda general
    /// </summary>
    public string? TerminoBusqueda { get; set; }
}