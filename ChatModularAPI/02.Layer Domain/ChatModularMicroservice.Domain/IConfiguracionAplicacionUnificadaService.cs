using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    /// <summary>
    /// Interfaz para el servicio de configuración de aplicación unificada
    /// </summary>
    public interface IConfiguracionAplicacionUnificadaService
    {
        /// <summary>
        /// Obtiene la configuración de una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Configuración de la aplicación</returns>
        Task<ConfiguracionAplicacionUnificadaDto?> ObtenerConfiguracionAsync(int aplicacionId);

        /// <summary>
        /// Obtiene la configuración de una aplicación por código
        /// </summary>
        /// <param name="codigoAplicacion">Código de la aplicación</param>
        /// <returns>Configuración de la aplicación</returns>
        Task<ConfiguracionAplicacionUnificadaDto?> ObtenerConfiguracionPorCodigoAsync(string codigoAplicacion);

        /// <summary>
        /// Crea una nueva configuración para una aplicación
        /// </summary>
        /// <param name="createDto">Datos para crear la configuración</param>
        /// <returns>Configuración creada</returns>
        Task<ConfiguracionAplicacionUnificadaDto> CrearConfiguracionAsync(CreateConfiguracionAplicacionUnificadaDto createDto);

        /// <summary>
        /// Actualiza la configuración de una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="updateDto">Datos para actualizar la configuración</param>
        /// <returns>Configuración actualizada</returns>
        Task<ConfiguracionAplicacionUnificadaDto> ActualizarConfiguracionAsync(int aplicacionId, UpdateConfiguracionAplicacionUnificadaDto updateDto);

        /// <summary>
        /// Elimina la configuración de una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>True si se eliminó correctamente</returns>
        Task<bool> EliminarConfiguracionAsync(int aplicacionId);

        /// <summary>
        /// Lista todas las configuraciones de aplicaciones
        /// </summary>
        /// <returns>Lista de configuraciones</returns>
        Task<IEnumerable<ConfiguracionAplicacionUnificadaDto>> ListarConfiguracionesAsync();

        /// <summary>
        /// Actualiza solo la configuración de adjuntos de una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <param name="adjuntosDto">Configuración de adjuntos</param>
        /// <returns>Configuración de adjuntos actualizada</returns>
        Task<ConfiguracionAdjuntosDto> ActualizarConfiguracionAdjuntosAsync(int aplicacionId, ConfiguracionAdjuntosDto adjuntosDto);

        /// <summary>
        /// Obtiene solo la configuración de adjuntos de una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Configuración de adjuntos si existe</returns>
        Task<ConfiguracionAdjuntosDto?> ObtenerConfiguracionAdjuntosAsync(int aplicacionId);

        /// <summary>
        /// Restaura la configuración de una aplicación a los valores por defecto
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>Configuración restaurada</returns>
        Task<ConfiguracionAplicacionUnificadaDto> RestaurarConfiguracionPorDefectoAsync(int aplicacionId);

        /// <summary>
        /// Verifica si existe configuración para una aplicación
        /// </summary>
        /// <param name="aplicacionId">ID de la aplicación</param>
        /// <returns>True si existe</returns>
        Task<bool> ExisteConfiguracionAsync(int aplicacionId);

        /// <summary>
        /// Valida que los tipos de archivos permitidos sean válidos
        /// </summary>
        /// <param name="tiposArchivos">Tipos de archivos separados por coma</param>
        /// <returns>True si son válidos</returns>
        Task<bool> ValidarTiposArchivosAsync(string tiposArchivos);

        /// <summary>
        /// Obtiene los tipos de archivos permitidos por defecto
        /// </summary>
        /// <returns>Tipos de archivos por defecto</returns>
        Task<string> ObtenerTiposArchivosPorDefectoAsync();

        /// <summary>
        /// Obtiene las configuraciones activas de aplicaciones
        /// </summary>
        /// <returns>Lista de configuraciones activas</returns>
        Task<List<ConfiguracionAplicacionUnificadaDto>> ObtenerConfiguracionesActivasAsync();
    }
}