using ChatModularMicroservice.Entities; // Para AdquisicionFilterItemType y AdquisicionFilterListType
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Filter;
using ChatModularMicroservice.Entities.Model;

namespace ChatModularMicroservice.Domain
{
    public interface IAdquisicionService
    {
        Task<object> GetAdquisicionList(AdquisicionFilter filter, AdquisicionFilterListType listType, object? options);
        Task<object> GetAdquisicionById(AdquisicionFilter filter, AdquisicionFilterItemType itemType);
        Task<object> CreateAdquisicion(AdquisicionInsertDTO entidad);
        Task<object> EditAdquisicion(AdquisicionEntity entidad);
        Task<object> DeleteAdquisicion(int nAdquisicionCodigo);
    }
}