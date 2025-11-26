using ChatModularMicroservice.Entities.Models;
namespace ChatModularMicroservice.Domain
{

/// <summary>
/// Entidad Usuario
/// </summary>
public class Usuario
{
    /// <summary>
    /// ID único del usuario
    /// </summary>
    public int cUsuariosId { get; set; }

    /// <summary>
    /// Nombre del usuario
    /// </summary>
    public string cNombre { get; set; } = string.Empty;

    /// <summary>
    /// Apellido del usuario
    /// </summary>
    public string cApellido { get; set; } = string.Empty;

    /// <summary>
    /// Email del usuario
    /// </summary>
    public string cEmail { get; set; } = string.Empty;

    /// <summary>
    /// Teléfono del usuario
    /// </summary>
    public string? cTelefono { get; set; }

    /// <summary>
    /// Avatar del usuario
    /// </summary>
    public string? cAvatar { get; set; }

    /// <summary>
    /// Estado del usuario
    /// </summary>
    public bool bActivo { get; set; } = true;

    /// <summary>
    /// Fecha de creación
    /// </summary>
    public DateTime dFechaCreacion { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// ID de la empresa
    /// </summary>
    public int nEmpresaId { get; set; }

    /// <summary>
    /// ID de la aplicación
    /// </summary>
    public int nAplicacionId { get; set; }
}

}