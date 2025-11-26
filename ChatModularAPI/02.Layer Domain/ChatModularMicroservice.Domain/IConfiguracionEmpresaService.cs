using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IConfiguracionEmpresaService
    {
        Task<List<ConfiguracionEmpresaDto>> GetAllAsync();
        Task<ConfiguracionEmpresaDto?> GetByIdAsync(int id);
        Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAsync(int empresaId);
        Task<List<ConfiguracionEmpresaDto>> GetByAplicacionAsync(int aplicacionId);
        Task<List<ConfiguracionEmpresaDto>> GetByEmpresaAndAplicacionAsync(int empresaId, int aplicacionId);
        Task<ConfiguracionEmpresaDto?> GetByClaveAsync(string clave, int empresaId, int aplicacionId);
        Task<List<ConfiguracionEmpresaDto>> GetActivasAsync();
        Task<List<ConfiguracionEmpresaDto>> SearchAsync(string terminoBusqueda);
        Task<List<ConfiguracionEmpresaAgrupadaDto>> GetAgrupadasAsync(int empresaId, int aplicacionId);
        Task<ConfiguracionEmpresaDto> CreateAsync(CreateConfiguracionEmpresaDto createDto);
        Task<ConfiguracionEmpresaDto?> UpdateAsync(int id, UpdateConfiguracionEmpresaDto updateDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByClaveAsync(string clave, int empresaId, int aplicacionId);
        Task<List<ConfiguracionEmpresaDto>> CopiarConfiguracionesDeAplicacionAsync(int empresaId, int aplicacionId);
        Task<bool> RestaurarConfiguracionesPorDefectoAsync(int empresaId, int aplicacionId);
        Task<List<ConfiguracionHeredadaDto>> GetConfiguracionesHeredadasAsync(int empresaId, int aplicacionId);
    }
}