using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de categorías de activos
/// </summary>
public interface ICategoriaActivoRepository : IDeleteIntRepository, IInsertIntRepository<CategoriaActivoEntity>, IUpdateRepository<CategoriaActivoEntity>
{
    Task<CategoriaActivoEntity?> GetItem(CategoriaActivoFilter filter, CategoriaActivoFilterItemType filterType);
    Task<IEnumerable<CategoriaActivoEntity>> GetLstItem(CategoriaActivoFilter filter, CategoriaActivoFilterListType filterType, Utils.Pagination pagination);
}