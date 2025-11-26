using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de personas
/// </summary>
public interface IPersonaRepository : IDeleteIntRepository, IInsertIntRepository<Usuario>, IUpdateRepository<Usuario>
{
    Task<Usuario?> GetByIdAsync(Guid id);
    Task<Usuario?> GetByCodigoAsync(string codigo);
    Task<Usuario> CreateAsync(Usuario persona);
    Task<Usuario> UpdateAsync(Usuario persona);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
    Task<bool> ExistsByCodigoAsync(string codigo);
    Task<IEnumerable<Usuario>> GetAllAsync();
    Task<IEnumerable<Usuario>> GetByFilterAsync(PersonaFilter filter);
    Task<IEnumerable<Usuario>> SearchPersonasAsync(string searchTerm);
    
    /// <summary>
    /// Obtiene una persona específica basada en el filtro y tipo de filtro
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <returns>Persona encontrada</returns>
    Task<Usuario> GetItem(PersonaFilter filter, PersonaFilterItemType filterType);
    
    /// <summary>
    /// Obtiene una lista de personas basada en el filtro y tipo de filtro
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <param name="pagination">Configuración de paginación</param>
    /// <returns>Lista de personas encontradas</returns>
    Task<IEnumerable<Usuario>> GetLstItem(PersonaFilter filter, PersonaFilterListType filterType, Utils.Pagination pagination);
}