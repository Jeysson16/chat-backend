namespace ChatModularMicroservice.Entities.Models;

/// <summary>
/// Entidad Contacto
/// </summary>
public class Contacto
{
    /// <summary>
    /// ID único del contacto
    /// </summary>
    public int nContactosId { get; set; }

    /// <summary>
    /// ID del usuario solicitante
    /// </summary>
    public int cUsuarioSolicitanteId { get; set; }

    /// <summary>
    /// ID del usuario destino
    /// </summary>
    public int cUsuarioDestinoId { get; set; }

    /// <summary>
    /// Estado del contacto (Pendiente, Aceptado, Rechazado)
    /// </summary>
    public string cEstado { get; set; } = "Pendiente";

    /// <summary>
    /// Fecha de solicitud
    /// </summary>
    public DateTime dFechaSolicitud { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Fecha de respuesta
    /// </summary>
    public DateTime? dFechaRespuesta { get; set; }

    /// <summary>
    /// ID de la empresa
    /// </summary>
    public int nEmpresaId { get; set; }

    /// <summary>
    /// ID de la aplicación
    /// </summary>
    public int nAplicacionId { get; set; }

    /// <summary>
    /// Usuario solicitante (navegación)
    /// </summary>
    public Usuario? UsuarioSolicitante { get; set; }

    /// <summary>
    /// Usuario destino (navegación)
    /// </summary>
    public Usuario? UsuarioDestino { get; set; }
}