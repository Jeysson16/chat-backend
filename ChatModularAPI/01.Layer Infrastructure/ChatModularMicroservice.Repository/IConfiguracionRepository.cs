using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de configuración de aplicaciones
/// </summary>
public interface IConfiguracionRepository : IDeleteIntRepository, IInsertIntRepository<ConfiguracionAplicacion>, IUpdateRepository<ConfiguracionAplicacion>
{
    Task<ConfiguracionAplicacion> GetItem(ConfiguracionFilter filter, ConfiguracionFilterItemType filterType);
    Task<IEnumerable<ConfiguracionAplicacion>> GetLstItem(ConfiguracionFilter filter, ConfiguracionFilterListType filterType, Utils.Pagination pagination);
}