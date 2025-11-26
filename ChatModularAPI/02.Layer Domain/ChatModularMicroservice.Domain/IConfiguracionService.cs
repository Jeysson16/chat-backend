using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de configuración
    /// </summary>
    public interface IConfiguracionService
    {
        /// <summary>
        /// Obtiene la configuración por código de aplicación
        /// </summary>
        /// <param name="codigoAplicacion">Código de la aplicación</param>
        /// <returns>Configuración de la aplicación</returns>
        Task<ConfiguracionDto?> ObtenerConfiguracionAsync(string codigoAplicacion);

        /// <summary>
        /// Crea o actualiza una configuración
        /// </summary>
        /// <param name="configuracion">Datos de la configuración</param>
        /// <returns>Configuración creada o actualizada</returns>
        Task<ConfiguracionDto> GuardarConfiguracionAsync(ConfiguracionDto configuracion);

        /// <summary>
        /// Elimina una configuración
        /// </summary>
        /// <param name="codigoAplicacion">Código de la aplicación</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> EliminarConfiguracionAsync(string codigoAplicacion);

        /// <summary>
        /// Lista todas las configuraciones activas
        /// </summary>
        /// <returns>Lista de configuraciones</returns>
        Task<IEnumerable<ConfiguracionDto>> ListarConfiguracionesAsync();
    }
}