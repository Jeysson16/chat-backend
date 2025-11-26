using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IApplicationService
    {
        Task<List<ApplicationDto>> GetAllApplicationsAsync();
        Task<List<ApplicationDto>> GetActiveApplicationsAsync();
        Task<ApplicationDto?> GetApplicationByIdAsync(int id);
        Task<ApplicationDto?> GetApplicationByCodeAsync(string codigo);
        Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto);
        Task<ApplicationDto?> UpdateApplicationAsync(int id, UpdateApplicationDto updateDto);
        Task<bool> DeleteApplicationAsync(int id);
        Task<ConfiguracionAplicacionDto?> GetApplicationConfigurationAsync(int id);
        Task<ConfiguracionAplicacionDto?> UpdateApplicationConfigurationAsync(int id, ConfiguracionAplicacionDto configurationDto);
    }
}