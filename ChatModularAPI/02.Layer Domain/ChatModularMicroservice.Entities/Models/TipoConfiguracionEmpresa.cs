namespace ChatModularMicroservice.Entities.Models
{

/// <summary>
/// Tipos de configuración de empresa
/// </summary>
public enum TipoConfiguracionEmpresa
{
    /// <summary>
    /// Configuración de texto
    /// </summary>
    Texto = 1,
    
    /// <summary>
    /// Configuración numérica
    /// </summary>
    Numero = 2,
    
    /// <summary>
    /// Configuración booleana
    /// </summary>
    Booleano = 3,
    
    /// <summary>
    /// Configuración de fecha
    /// </summary>
    Fecha = 4,
    
    /// <summary>
    /// Configuración JSON
    /// </summary>
    Json = 5,
    
    /// <summary>
    /// Configuración de URL
    /// </summary>
    Url = 6,
    
    /// <summary>
    /// Configuración de email
    /// </summary>
    Email = 7,
    
    /// <summary>
    /// Configuración de chat
    /// </summary>
    Chat = 8
}
}
