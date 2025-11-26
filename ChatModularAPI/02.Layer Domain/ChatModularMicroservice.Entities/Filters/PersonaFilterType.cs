namespace ChatModularMicroservice.Entities;

/// <summary>
/// Enumeración para los tipos de filtro de persona (item individual)
/// </summary>
public enum PersonaFilterItemType
{
    /// <summary>
    /// Filtrar por ID de persona
    /// </summary>
    PersonaId = 1,
    
    /// <summary>
    /// Filtrar por código de persona
    /// </summary>
    PersonaCodigo = 2,
    
    /// <summary>
    /// Filtrar por email de persona
    /// </summary>
    PersonaEmail = 3,
    
    /// <summary>
    /// Filtrar por nombre de persona
    /// </summary>
    PersonaNombre = 4,
    
    /// <summary>
    /// Filtrar por estado activo
    /// </summary>
    PersonaEsActiva = 5
}

/// <summary>
/// Enumeración para los tipos de filtro de persona (lista)
/// </summary>
public enum PersonaFilterListType
{
    /// <summary>
    /// Obtener todas las personas activas
    /// </summary>
    PersonasActivas = 1,
    
    /// <summary>
    /// Obtener todas las personas
    /// </summary>
    TodasPersonas = 2,
    
    /// <summary>
    /// Filtrar por término de búsqueda
    /// </summary>
    BusquedaPersonas = 3,
    
    /// <summary>
    /// Filtrar por empresa
    /// </summary>
    PersonasPorEmpresa = 4,
    
    /// <summary>
    /// Filtrar por aplicación
    /// </summary>
    PersonasPorAplicacion = 5
}

/// <summary>
/// Clase para filtros de persona
/// </summary>
public class PersonaFilter
{
    /// <summary>
    /// ID de la persona
    /// </summary>
    public Guid? PersonaId { get; set; }
    
    /// <summary>
    /// Código de la persona
    /// </summary>
    public string? PersonaCodigo { get; set; }
    
    /// <summary>
    /// Email de la persona
    /// </summary>
    public string? PersonaEmail { get; set; }
    
    /// <summary>
    /// Nombre de la persona
    /// </summary>
    public string? PersonaNombre { get; set; }
    
    /// <summary>
    /// Estado activo de la persona
    /// </summary>
    public bool? PersonaEsActiva { get; set; }
    
    /// <summary>
    /// Término de búsqueda general
    /// </summary>
    public string? TerminoBusqueda { get; set; }
    
    /// <summary>
    /// ID de empresa para filtrar
    /// </summary>
    public Guid? EmpresaId { get; set; }
    
    /// <summary>
    /// ID de aplicación para filtrar
    /// </summary>
    public Guid? AplicacionId { get; set; }
}