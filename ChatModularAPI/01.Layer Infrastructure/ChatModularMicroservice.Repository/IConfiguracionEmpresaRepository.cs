using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de configuración de empresas
/// </summary>
public interface IConfiguracionEmpresaRepository : IDeleteIntRepository, IInsertIntRepository<ConfiguracionEmpresa>, IUpdateRepository<ConfiguracionEmpresa>
{
    Task<ConfiguracionEmpresa> GetItem(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterItemType filterType);
    Task<IEnumerable<ConfiguracionEmpresa>> GetLstItem(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterListType filterType, Utils.Pagination pagination);

    Task<List<ConfiguracionEmpresaDto>> GetAllAsync();
    Task<ConfiguracionEmpresaDto?> GetByIdAsync(int id);
    Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAsync(int empresaId);
    Task<List<ConfiguracionEmpresaDto>> GetByAplicacionAsync(int aplicacionId);
    Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAndAplicacionAsync(int empresaId, int aplicacionId);
    Task<ConfiguracionEmpresaDto?> GetByClaveAsync(string clave, int empresaId, int aplicacionId);
    Task<List<ConfiguracionEmpresaDto>> GetActivasAsync();
    Task<List<ConfiguracionEmpresaDto>> SearchAsync(string searchTerm);
    Task<List<ConfiguracionEmpresaAgrupadaDto>> GetAgrupadasAsync();
    Task<List<ConfiguracionHeredadaDto>> GetConfiguracionesHeredadasAsync(int empresaId, int aplicacionId);

    Task<ConfiguracionEmpresaDto> CreateAsync(CreateConfiguracionEmpresaDto createDto);
    Task<ConfiguracionEmpresaDto> UpdateAsync(int id, UpdateConfiguracionEmpresaDto updateDto);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByClaveAsync(string clave, int empresaId, int aplicacionId);
    Task<bool> ExistsAsync(int id);
    Task<List<ConfiguracionEmpresaDto>> CopiarConfiguracionesDeAplicacionAsync(int empresaId, int aplicacionId);
    Task<bool> RestaurarConfiguracionesPorDefectoAsync(int empresaId, int aplicacionId);

    Task<ConfiguracionEmpresa> CreateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa);
    Task<bool> UpdateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa);
    Task<bool> DeleteConfiguracionEmpresaAsync(string configuracionId);
    Task<bool> ConfiguracionEmpresaExistsAsync(string configuracionId);
}
