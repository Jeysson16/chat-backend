using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Shared.Utils;
using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Domain
{
    public class ContactoService : IContactoService
    {
        private readonly IContactoRepository _repo;
        private readonly ILogger<ContactoService> _logger;
        private readonly IConfiguracionAplicacionUnificadaRepository _configRepo;

        public ContactoService(IContactoRepository repo, ILogger<ContactoService> logger, IConfiguracionAplicacionUnificadaRepository configRepo)
        {
            _repo = repo;
            _logger = logger;
            _configRepo = configRepo;
        }

        public async Task<ServiceResponse<List<ChatUsuarioDto>>> BuscarUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado)
        {
            try
            {
                // Obtener configuración de la aplicación - primero obtener el código de aplicación
                // El aplicacionId puede ser un número o un código de aplicación como string
                string appResult = null;
                
                // Intentar parsear como número primero
                if (int.TryParse(aplicacionId, out int appIdNumerico))
                {
                    appResult = await _repo.GetApplicationCodeAsync(appIdNumerico);
                }
                else
                {
                    // Si no es número, asumir que ya es un código de aplicación
                    appResult = aplicacionId;
                    _logger.LogInformation("AplicacionId proporcionado como código: {CodigoAplicacion}", aplicacionId);
                }
                
                if (string.IsNullOrEmpty(appResult))
                {
                    _logger.LogWarning("No se encontró aplicación con ID: {AplicacionId}, usando modo LOCAL por defecto", aplicacionId);
                    var contactosDefault = await _repo.BuscarUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                    return ServiceResponse<List<ChatUsuarioDto>>.Success(contactosDefault ?? new List<ChatUsuarioDto>());
                }

                // Obtener configuración usando el código de aplicación
                var configuracion = await _configRepo.ObtenerUnificadaPorCodigoAplicacionAsync(appResult);
                var modoGestion = configuracion?.cContactosModoGestion ?? "LOCAL";
                
                _logger.LogInformation("Buscando usuarios con modo de gestión: {ModoGestion}, término: {Termino}, usuario: {UsuarioId}, empresa: {EmpresaId}, aplicación: {AplicacionId}", 
                    modoGestion, terminoBusqueda, usuarioId, empresaId, aplicacionId);

                List<ChatUsuarioDto> resultado = new List<ChatUsuarioDto>();

                switch (modoGestion)
                {
                    case "API_EXTERNA":
                        // Modo API_EXTERNA: Buscar solo en API externa
                        _logger.LogWarning("Modo API_EXTERNA: No implementado - se requeriría integración con API externa");
                        // Por ahora retornar vacío hasta implementar API externa
                        break;

                    case "LOCAL":
                        // Modo LOCAL: Primero buscar en contactos locales, luego en usuarios si no hay contactos
                        _logger.LogInformation("Modo LOCAL: Buscando en contactos locales");
                        var contactosLocales = await _repo.BuscarUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                        resultado = contactosLocales ?? new List<ChatUsuarioDto>();
                        
                        // Si no se encontraron contactos, buscar en usuarios para permitir agregar nuevos contactos
                        if (resultado.Count == 0 && !string.IsNullOrEmpty(terminoBusqueda))
                        {
                            _logger.LogInformation("Modo LOCAL: No se encontraron contactos, buscando en usuarios para permitir agregar nuevos contactos");
                            var usuarios = await _repo.BuscarUsuariosEnTablaUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                            if (usuarios != null && usuarios.Count > 0)
                            {
                                resultado = usuarios;
                                _logger.LogInformation("Modo LOCAL: Se encontraron {Count} usuarios para agregar como contactos", usuarios.Count);
                            }
                        }
                        break;

                    case "HIBRIDO":
                        // Modo HIBRIDO: Buscar en contactos locales + API externa
                        _logger.LogInformation("Modo HIBRIDO: Buscando en contactos locales y API externa");
                        // Primero buscar en contactos locales
                        var contactosHibrido = await _repo.BuscarUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                        resultado = contactosHibrido ?? new List<ChatUsuarioDto>();
                        // Aquí se agregaría la búsqueda en API externa cuando esté implementada
                        break;

                    default:
                        _logger.LogWarning("Modo de gestión no reconocido: {ModoGestion}, usando LOCAL por defecto", modoGestion);
                        var contactosDefault = await _repo.BuscarUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                        resultado = contactosDefault ?? new List<ChatUsuarioDto>();
                        break;
                }

                return ServiceResponse<List<ChatUsuarioDto>>.Success(resultado);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios con modo de gestión");
                return ServiceResponse<List<ChatUsuarioDto>>.Error($"Error al buscar usuarios: {ex.Message}");
            }
        }

        public async Task<ServiceResponse<bool>> EnviarSolicitudContactoAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId, string mensaje = "")
        {
            var ok = await _repo.EnviarSolicitudContactoAsync(usuarioSolicitanteId, usuarioDestinoId, empresaId, aplicacionId, mensaje);
            return ok ? ServiceResponse<bool>.Success(true) : ServiceResponse<bool>.Error("No se pudo enviar la solicitud");
        }

        public async Task<ServiceResponse<ContactoDto?>> AceptarSolicitudContactoAsync(string contactoId, string usuarioId)
        {
            var contacto = await _repo.AceptarSolicitudContactoAsync(contactoId, usuarioId);
            if (contacto == null) return ServiceResponse<ContactoDto?>.Error("Solicitud no encontrada");
            return ServiceResponse<ContactoDto?>.Success(contacto);
        }

        public async Task<ServiceResponse<bool>> RechazarSolicitudContactoAsync(string solicitudId, string usuarioId, string empresaId, string aplicacionId)
        {
            var ok = await _repo.RechazarSolicitudContactoAsync(solicitudId, usuarioId, empresaId, aplicacionId);
            return ok ? ServiceResponse<bool>.Success(true) : ServiceResponse<bool>.Error("No se pudo rechazar la solicitud");
        }

        public async Task<ServiceResponse<List<List<ContactoDto>>>> ListarContactosPorUsuarioAsync(string usuarioId, string empresaId, string aplicacionId, string? estado = null)
        {
            var lista = await _repo.ListarContactosPorUsuarioAsync(usuarioId, empresaId, aplicacionId, estado);
            var agrupado = new List<List<ContactoDto>> { lista };
            return ServiceResponse<List<List<ContactoDto>>>.Success(agrupado);
        }

        public async Task<ServiceResponse<List<List<SolicitudContactoDto>>>> ListarSolicitudesPendientesAsync(string usuarioId, string empresaId, string aplicacionId)
        {
            var lista = await _repo.ListarSolicitudesPendientesAsync(usuarioId, empresaId, aplicacionId);
            var agrupado = new List<List<SolicitudContactoDto>> { lista };
            return ServiceResponse<List<List<SolicitudContactoDto>>>.Success(agrupado);
        }

        public async Task<ServiceResponse<bool>> VerificarPermisoChatDirectoAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId)
        {
            var ok = await _repo.VerificarPermisoChatDirectoAsync(usuario1Id, usuario2Id, empresaId, aplicacionId);
            return ServiceResponse<bool>.Success(ok);
        }

        public async Task<ServiceResponse<bool>> BloquearContactoAsync(string usuarioId, string contactoUsuarioId)
        {
            var ok = await _repo.BloquearContactoAsync(usuarioId, contactoUsuarioId, string.Empty, string.Empty);
            return ServiceResponse<bool>.Success(ok);
        }

        public async Task<ServiceResponse<bool>> DesbloquearContactoAsync(string usuarioId, string contactoUsuarioId)
        {
            var ok = await _repo.DesbloquearContactoAsync(usuarioId, contactoUsuarioId, string.Empty, string.Empty);
            return ServiceResponse<bool>.Success(ok);
        }

        public async Task<ServiceResponse<bool>> EliminarContactoAsync(string usuarioId, string contactoUsuarioId, string empresaId, string aplicacionId)
        {
            var ok = await _repo.EliminarContactoAsync(usuarioId, contactoUsuarioId, empresaId, aplicacionId);
            return ServiceResponse<bool>.Success(ok);
        }

        public async Task<ServiceResponse<List<ChatUsuarioDto>>> BuscarUsuariosEnTablaUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado)
        {
            try
            {
                var usuarios = await _repo.BuscarUsuariosEnTablaUsuariosAsync(terminoBusqueda, usuarioId, empresaId, aplicacionId, tipoListado);
                return ServiceResponse<List<ChatUsuarioDto>>.Success(usuarios);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios en tabla Usuarios: {Error}", ex.Message);
                return ServiceResponse<List<ChatUsuarioDto>>.Error($"Error al buscar usuarios: {ex.Message}");
            }
        }
    }
}