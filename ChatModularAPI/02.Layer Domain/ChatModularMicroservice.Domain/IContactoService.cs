using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Domain
{
    public interface IContactoService
    {
        Task<ServiceResponse<List<ChatUsuarioDto>>> BuscarUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado);
        Task<ServiceResponse<bool>> EnviarSolicitudContactoAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId, string mensaje = "");
        Task<ServiceResponse<ContactoDto?>> AceptarSolicitudContactoAsync(string contactoId, string usuarioId);
        Task<ServiceResponse<bool>> RechazarSolicitudContactoAsync(string solicitudId, string usuarioId, string empresaId, string aplicacionId);
        Task<ServiceResponse<List<List<ContactoDto>>>> ListarContactosPorUsuarioAsync(string usuarioId, string empresaId, string aplicacionId, string? estado = null);
        Task<ServiceResponse<List<List<SolicitudContactoDto>>>> ListarSolicitudesPendientesAsync(string usuarioId, string empresaId, string aplicacionId);
        Task<ServiceResponse<bool>> VerificarPermisoChatDirectoAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId);
        Task<ServiceResponse<bool>> BloquearContactoAsync(string usuarioId, string contactoUsuarioId);
        Task<ServiceResponse<bool>> DesbloquearContactoAsync(string usuarioId, string contactoUsuarioId);
        Task<ServiceResponse<bool>> EliminarContactoAsync(string usuarioId, string contactoUsuarioId, string empresaId, string aplicacionId);
        Task<ServiceResponse<List<ChatUsuarioDto>>> BuscarUsuariosEnTablaUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado);
    }
}