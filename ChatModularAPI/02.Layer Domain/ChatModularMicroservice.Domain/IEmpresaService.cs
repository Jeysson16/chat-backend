using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IEmpresaService
    {
        Task<List<EmpresaDto>> GetAllEmpresasAsync();
        Task<EmpresaDto?> GetEmpresaByIdAsync(int id);
        Task<EmpresaDto?> GetEmpresaByCodigoAsync(string codigo);
        Task<List<EmpresaDto>> GetEmpresasActivasAsync();
        Task<List<EmpresaDto>> SearchEmpresasAsync(string terminoBusqueda);
        Task<EmpresaDto> CreateEmpresaAsync(CreateEmpresaDto createEmpresaDto);
        Task<EmpresaDto> UpdateEmpresaAsync(int id, UpdateEmpresaDto updateEmpresaDto);
        Task<bool> DeleteEmpresaAsync(int id);
        Task<bool> ExistsEmpresaByCodigoAsync(string codigo, int? excludeId = null);
    }
}