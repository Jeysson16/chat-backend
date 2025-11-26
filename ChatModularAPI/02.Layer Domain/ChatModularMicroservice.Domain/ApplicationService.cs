using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using System.Linq;

namespace ChatModularMicroservice.Domain
{
    public class ApplicationService : IApplicationService
    {
        private readonly IApplicationRepository _repo;

        public ApplicationService(IApplicationRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<ApplicationDto>> GetAllApplicationsAsync()
        {
            var items = await _repo.GetAllApplicationsAsync();
            return items.Select(ToDto).ToList();
        }

        public async Task<List<ApplicationDto>> GetActiveApplicationsAsync()
        {
            var items = await _repo.GetAllApplicationsAsync();
            return items.Where(a => a.bAplicacionesEsActiva).Select(ToDto).ToList();
        }

        public async Task<ApplicationDto?> GetApplicationByIdAsync(int id)
        {
            var item = await _repo.GetApplicationByIdAsync(id);
            return item == null ? null : ToDto(item);
        }

        public async Task<ApplicationDto?> GetApplicationByCodeAsync(string codigo)
        {
            var item = await _repo.GetApplicationByCodeAsync(codigo);
            return item == null ? null : ToDto(item);
        }

        public async Task<ApplicationDto> CreateApplicationAsync(CreateApplicationDto createDto)
        {
            var entity = new ChatModularMicroservice.Entities.Models.Application
            {
                cAplicacionesNombre = createDto.cAplicacionesNombre,
                cAplicacionesDescripcion = createDto.cAplicacionesDescripcion,
                cAplicacionesCodigo = string.IsNullOrWhiteSpace(createDto.cAplicacionesCodigo) ? createDto.cAplicacionesNombre : createDto.cAplicacionesCodigo,
                bAplicacionesEsActiva = true
            };
            var created = await _repo.CreateApplicationAsync(entity);
            return ToDto(created);
        }

        public async Task<ApplicationDto?> UpdateApplicationAsync(int id, UpdateApplicationDto updateDto)
        {
            var existing = await _repo.GetApplicationByIdAsync(id);
            if (existing == null) return null;
            if (!string.IsNullOrWhiteSpace(updateDto.cAplicacionesNombre)) existing.cAplicacionesNombre = updateDto.cAplicacionesNombre!;
            if (!string.IsNullOrWhiteSpace(updateDto.cAplicacionesDescripcion)) existing.cAplicacionesDescripcion = updateDto.cAplicacionesDescripcion!;
            if (!string.IsNullOrWhiteSpace(updateDto.cAplicacionesCodigo)) existing.cAplicacionesCodigo = updateDto.cAplicacionesCodigo!;
            if (updateDto.bAplicacionesEsActiva.HasValue) existing.bAplicacionesEsActiva = updateDto.bAplicacionesEsActiva.Value;
            var updated = await _repo.UpdateApplicationAsync(existing);
            return ToDto(updated);
        }

        public Task<bool> DeleteApplicationAsync(int id)
        {
            return _repo.DeleteApplicationAsync(id);
        }

        public async Task<ConfiguracionAplicacionDto?> GetApplicationConfigurationAsync(int id)
        {
            var c = await _repo.GetApplicationConfigurationAsync(id);
            if (c == null) return null;
            return new ConfiguracionAplicacionDto
            {
                Id = c.nConfiguracionAplicacionId,
                NMaxTamanoArchivo = c.nMaxTamanoArchivo,
                CTiposArchivosPermitidos = c.cTiposArchivosPermitidos,
                BPermitirAdjuntos = c.bPermitirAdjuntos,
                NMaxCantidadAdjuntos = c.nMaxCantidadAdjuntos,
                BPermitirVisualizacionAdjuntos = c.bPermitirVisualizacionAdjuntos,
                NMaxLongitudMensaje = c.nMaxLongitudMensaje,
                BPermitirEmojis = c.bPermitirEmojis,
                BPermitirMensajesVoz = c.bPermitirMensajesVoz,
                BPermitirNotificaciones = c.bPermitirNotificaciones,
                BRequiereAutenticacion = c.bRequiereAutenticacion,
                NTiempoExpiracionSesion = c.nTiempoExpiracionSesion,
                BPermitirMensajesAnonimos = c.bPermitirMensajesAnonimos,
                BEsActiva = c.bEsActiva,
                CreatedAt = c.dFechaCreacion,
                UpdatedAt = c.dFechaActualizacion
            };
        }

        public async Task<ConfiguracionAplicacionDto?> UpdateApplicationConfigurationAsync(int id, ConfiguracionAplicacionDto configurationDto)
        {
            var current = await _repo.GetApplicationConfigurationAsync(id);
            if (current == null) return null;
            current.nMaxTamanoArchivo = configurationDto.NMaxTamanoArchivo;
            current.cTiposArchivosPermitidos = configurationDto.CTiposArchivosPermitidos;
            current.bPermitirAdjuntos = configurationDto.BPermitirAdjuntos;
            current.nMaxCantidadAdjuntos = configurationDto.NMaxCantidadAdjuntos;
            current.bPermitirVisualizacionAdjuntos = configurationDto.BPermitirVisualizacionAdjuntos;
            current.nMaxLongitudMensaje = configurationDto.NMaxLongitudMensaje;
            current.bPermitirEmojis = configurationDto.BPermitirEmojis;
            current.bPermitirMensajesVoz = configurationDto.BPermitirMensajesVoz;
            current.bPermitirNotificaciones = configurationDto.BPermitirNotificaciones;
            current.bRequiereAutenticacion = configurationDto.BRequiereAutenticacion;
            current.nTiempoExpiracionSesion = configurationDto.NTiempoExpiracionSesion;
            current.bPermitirMensajesAnonimos = configurationDto.BPermitirMensajesAnonimos;
            current.bEsActiva = configurationDto.BEsActiva;

            var updated = await _repo.UpdateApplicationConfigurationAsync(current);
            return new ConfiguracionAplicacionDto
            {
                Id = updated.nConfiguracionAplicacionId,
                NMaxTamanoArchivo = updated.nMaxTamanoArchivo,
                CTiposArchivosPermitidos = updated.cTiposArchivosPermitidos,
                BPermitirAdjuntos = updated.bPermitirAdjuntos,
                NMaxCantidadAdjuntos = updated.nMaxCantidadAdjuntos,
                BPermitirVisualizacionAdjuntos = updated.bPermitirVisualizacionAdjuntos,
                NMaxLongitudMensaje = updated.nMaxLongitudMensaje,
                BPermitirEmojis = updated.bPermitirEmojis,
                BPermitirMensajesVoz = updated.bPermitirMensajesVoz,
                BPermitirNotificaciones = updated.bPermitirNotificaciones,
                BRequiereAutenticacion = updated.bRequiereAutenticacion,
                NTiempoExpiracionSesion = updated.nTiempoExpiracionSesion,
                BPermitirMensajesAnonimos = updated.bPermitirMensajesAnonimos,
                BEsActiva = updated.bEsActiva,
                CreatedAt = updated.dFechaCreacion,
                UpdatedAt = updated.dFechaActualizacion
            };
        }

        private static ApplicationDto ToDto(ChatModularMicroservice.Entities.Models.Application a)
        {
            return new ApplicationDto
            {
                nAplicacionesId = a.nAplicacionesId,
                cAplicacionesNombre = a.cAplicacionesNombre,
                cAplicacionesDescripcion = a.cAplicacionesDescripcion,
                cAplicacionesCodigo = a.cAplicacionesCodigo,
                bAplicacionesEsActiva = a.bAplicacionesEsActiva,
                dAplicacionesFechaCreacion = a.dAplicacionesFechaCreacion
            };
        }
    }
}
