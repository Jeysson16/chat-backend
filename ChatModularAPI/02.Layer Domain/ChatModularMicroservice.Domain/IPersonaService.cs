using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Domain
{
    public interface IPersonaService
    {
        Task<ServiceResponse<PersonaDto?>> GetPersonaByCodigoAsync(string personaCodigo);
        Task<ServiceResponse<List<PersonaDto>>> GetPersonasActivasAsync(int page, int pageSize);
        Task<ServiceResponse<List<PersonaDto>>> BuscarPersonasAsync(BuscarPersonaDto searchCriteria);
        Task<ServiceResponse<PersonaDto?>> ActualizarPerfilAsync(string personaCodigo, ActualizarPerfilDto updateDto);
        Task<ServiceResponse<bool>> ActualizarEstadoConexionAsync(string personaCodigo, bool isOnline);
        Task<ServiceResponse<PrivacySettingsDto?>> GetConfiguracionPrivacidadAsync(string personaCodigo);
        Task<ServiceResponse<PrivacySettingsDto?>> ActualizarConfiguracionPrivacidadAsync(string personaCodigo, PrivacySettingsDto privacySettings);
    }
}