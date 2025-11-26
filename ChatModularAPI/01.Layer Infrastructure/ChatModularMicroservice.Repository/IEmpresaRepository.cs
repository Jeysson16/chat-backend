using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository
{
    public interface IEmpresaRepository : IDeleteIntRepository, IInsertIntRepository<Empresa>, IUpdateRepository<Empresa>
    {
        Task<Empresa> GetItem(EmpresaFilter filter, EmpresaFilterItemType filterType);
        Task<IEnumerable<Empresa>> GetLstItem(EmpresaFilter filter, EmpresaFilterListType filterType, Utils.Pagination pagination);
        Task<bool> ExistsEmpresaByCodigoAsync(string codigo, int? excludeId = null);
        Task<bool> ExistsEmpresaByIdAsync(int id);
        Task<IEnumerable<Empresa>> GetAllEmpresasAsync();
        Task<Empresa?> GetEmpresaByIdAsync(int id);
        Task<EmpresaDto> UpdateEmpresaAsync(int id, UpdateEmpresaDto updateDto);
        Task<bool> DeleteEmpresaAsync(int id);
        Task<IEnumerable<EmpresaDto>> GetEmpresasActivasAsync();
        Task<IEnumerable<EmpresaDto>> SearchEmpresasAsync(string searchTerm);
        Task<Empresa?> GetEmpresaByCodigoAsync(string codigo);
        Task<EmpresaDto> CreateEmpresaAsync(CreateEmpresaDto createDto);
        
        /// <summary>
        /// Valida si un código de empresa es válido
        /// </summary>
        /// <param name="empresaCode">Código de la empresa a validar</param>
        /// <returns>True si el código es válido, false en caso contrario</returns>
        Task<bool> ValidateEmpresaCodeAsync(string empresaCode);
        
        /// <summary>
        /// Obtiene las empresas asociadas a una aplicación específica
        /// </summary>
        /// <param name="applicationId">ID de la aplicación</param>
        /// <returns>Lista de empresas asociadas a la aplicación</returns>
        Task<List<object>?> GetEmpresasByApplicationAsync(string applicationId);
    }
}