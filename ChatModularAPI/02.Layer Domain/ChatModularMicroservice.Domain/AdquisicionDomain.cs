using ChatModularMicroservice.Entities; // Para AdquisicionFilterItemType y AdquisicionFilterListType
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Filter;
using ChatModularMicroservice.Entities.Model;

namespace ChatModularMicroservice.Domain
{
    // Clase mínima para resolver referencias del controlador
    public class AdquisicionDomain
    {
        public AdquisicionDomain() { }
        public Task<object> GetAdquisicionList(AdquisicionFilter filter, AdquisicionFilterListType listType, object? options)
            => Task.FromResult<object>(new { });
        public Task<object> GetAdquisicionById(AdquisicionFilter filter, AdquisicionFilterItemType itemType)
            => Task.FromResult<object>(new { });
        public Task<object> CreateAdquisicion(AdquisicionInsertDTO entidad)
            => Task.FromResult<object>(new { });
        public Task<object> EditAdquisicion(AdquisicionEntity entidad)
            => Task.FromResult<object>(new { });
        public Task<object> DeleteAdquisicion(int nAdquisicionCodigo)
            => Task.FromResult<object>(new { });
    }
}