using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository
{
    public interface IAppRegistroRepository : IDeleteIntRepository, IInsertIntRepository<AppRegistro>, IUpdateRepository<AppRegistro>
    {
        /// <summary>
        /// Obtiene un registro de aplicación por su código
        /// </summary>
        /// <param name="appCode">Código de la aplicación</param>
        /// <returns>Registro de aplicación si existe, null en caso contrario</returns>
        Task<AppRegistro?> GetByCodeAsync(string appCode);

        /// <summary>
        /// Obtiene un registro de aplicación por el ID de la aplicación
        /// </summary>
        /// <param name="applicationId">ID de la aplicación</param>
        /// <returns>Registro de aplicación si existe, null en caso contrario</returns>
        Task<AppRegistro?> GetByApplicationIdAsync(int applicationId);

        Task<AppRegistro> GetItem(AppRegistroFilter filter, AppRegistroFilterItemType filterType);
        Task<IEnumerable<AppRegistro>> GetLstItem(AppRegistroFilter filter, AppRegistroFilterListType filterType, Utils.Pagination pagination);
        
        /// <summary>
        /// Verifica si un usuario tiene acceso a una aplicación específica
        /// </summary>
        /// <param name="userId">ID del usuario</param>
        /// <param name="appCode">Código de la aplicación</param>
        /// <returns>True si el usuario tiene acceso, false en caso contrario</returns>
        Task<bool> UserHasAccessToAppAsync(Guid userId, string appCode);
    }
}