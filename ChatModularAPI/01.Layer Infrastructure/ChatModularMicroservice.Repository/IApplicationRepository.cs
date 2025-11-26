using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de aplicaciones
/// </summary>
public interface IApplicationRepository : IDeleteIntRepository, IInsertIntRepository<Application>, IUpdateRepository<Application>
{
    Task<IEnumerable<Application>> GetAllApplicationsAsync();
    Task<Application?> GetApplicationByIdAsync(int id);
    Task<Application?> GetApplicationByCodeAsync(string code);
    Task<Application> CreateApplicationAsync(Application application);
    Task<Application> UpdateApplicationAsync(Application application);
        Task<bool> DeleteApplicationAsync(int id);
        Task<bool> ApplicationExistsAsync(int id);
        Task<bool> ApplicationExistsByNameAsync(string name, int? excludeId = null);
        
        // Configuration methods
        Task<ConfiguracionAplicacion?> GetApplicationConfigurationAsync(int applicationId);
        Task<ConfiguracionAplicacion> CreateApplicationConfigurationAsync(ConfiguracionAplicacion configuration);
        Task<ConfiguracionAplicacion> UpdateApplicationConfigurationAsync(ConfiguracionAplicacion configuration);
        Task<bool> DeleteApplicationConfigurationAsync(int applicationId);

        /// <summary>
        /// Obtiene una aplicación específica basada en el filtro y tipo de filtro
        /// </summary>
        /// <param name="filter">Filtro de búsqueda</param>
        /// <param name="filterType">Tipo de filtro a aplicar</param>
        /// <returns>Aplicación encontrada</returns>
        Task<Application> GetItem(ApplicationFilter filter, ApplicationFilterItemType filterType);

        /// <summary>
        /// Obtiene una lista de aplicaciones basada en el filtro, tipo de filtro y paginación
        /// </summary>
        /// <param name="filter">Filtro de búsqueda</param>
        /// <param name="filterType">Tipo de filtro a aplicar</param>
        /// <param name="pagination">Configuración de paginación</param>
        /// <returns>Lista de aplicaciones</returns>
        Task<IEnumerable<Application>> GetLstItem(ApplicationFilter filter, ApplicationFilterListType filterType, Utils.Pagination pagination);
}