using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Infrastructure.Repositories;
using ChatModularMicroservice.Shared.Configs;
using Microsoft.Extensions.Logging;
using Supabase;
using ChatContactoSupabaseModel = ChatModularMicroservice.Domain.ChatContactoSupabase;

namespace ChatModularMicroservice.Infrastructure
{
    /// <summary>
    /// Repositorio de contactos usando Supabase
    /// </summary>
    public class ContactoSupabaseRepository : SupabaseRepository, IContactoRepository
    {
        public ContactoSupabaseRepository(
            Supabase.Client supabaseClient, 
            ILogger<ContactoSupabaseRepository> logger, 
            SupabaseConfig config) 
            : base(supabaseClient, logger, config)
        {
        }

        public async Task<List<ContactoDto>> ListarContactosAsync(string usuarioId, string empresaId, string aplicacionId)
        {
            // Llamar al método con el mismo nombre que usa el stored procedure
            return await ListarContactosPorUsuarioAsync(usuarioId, empresaId, aplicacionId, null);
        }

        public async Task<List<ContactoDto>> ListarContactosPorUsuarioAsync(string usuarioId, string empresaId, string aplicacionId, string? estado = null)
        {
            try
            {
                _logger.LogInformation("Listando contactos para usuario {UsuarioId} en empresa {EmpresaId} y aplicación {AplicacionId}", 
                    usuarioId, empresaId, aplicacionId);

                // Usar directamente la tabla chatcontacto (singular) que existe
                _logger.LogInformation("Consultando tabla chatcontacto para usuario {UsuarioId}", usuarioId);
                var query = _supabaseClient
                    .From<ChatContactoSupabaseModel>()
                    .Where(x => x.cUsuarioId == usuarioId);

                if (!string.IsNullOrEmpty(estado))
                {
                    query = query.Where(x => x.cEstadoContacto == estado);
                }

                var qResult = await query.Get();
                var models = qResult.Models ?? new List<ChatContactoSupabaseModel>();

                var mapped = models.Select(c => new ContactoDto
                {
                    cContactosId = c.nContactoId,
                    cUsuariosId = c.cUsuarioId,
                    cContactoUsuariosId = c.cContactoUsuarioId,
                    cContactosEstado = c.cEstadoContacto ?? "Activo",
                    dContactosFechaCreacion = c.dFechaCreacion ?? DateTime.UtcNow,
                    dContactosFechaModificacion = c.dFechaModificacion ?? c.dFechaCreacion ?? DateTime.UtcNow,
                    nContactosEmpresaId = int.TryParse(empresaId, out int empresaIdParsed) ? empresaIdParsed : 0, // Usar el parámetro recibido
                    nContactosAplicacionId = int.TryParse(aplicacionId, out int appId) ? appId : c.nAplicacionesId
                }).ToList();

                if (mapped.Count > 0)
                {
                    return mapped;
                }

                _logger.LogWarning("No se encontraron contactos para el usuario {UsuarioId}", usuarioId);
                return new List<ContactoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar contactos para usuario {UsuarioId}: {Error}", usuarioId, ex.Message);
                throw;
            }
        }

        // Implementación de otros métodos del IContactoRepository
        // Por ahora, lanzar NotImplementedException para los métodos no críticos
        
        public async Task<List<ChatUsuarioDto>> BuscarUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado)
        {
            try
            {
                _logger.LogInformation("Buscando usuarios con término {TerminoBusqueda} para usuario {UsuarioId} en empresa {EmpresaId} y aplicación {AplicacionId}", 
                    terminoBusqueda, usuarioId, empresaId, aplicacionId);

                // Buscar en los contactos existentes que coincidan con el término de búsqueda
                var contactos = await ListarContactosPorUsuarioAsync(usuarioId, empresaId, aplicacionId, null);
                
                var usuariosEncontrados = new List<ChatUsuarioDto>();
                var terminoLower = terminoBusqueda.ToLower();
                
                foreach (var contacto in contactos)
                {
                    if (contacto.cContactoUsuariosId.ToLower().Contains(terminoLower) ||
                        contacto.cContactosEstado.ToLower().Contains(terminoLower))
                    {
                        var usuario = new ChatUsuarioDto
                        {
                            cUsuariosChatId = contacto.cContactoUsuariosId,
                            cUsuariosChatNombre = $"Usuario {contacto.cContactoUsuariosId}",
                            cUsuariosChatEmail = $"{contacto.cContactoUsuariosId}@example.com",
                            bUsuariosChatEstaEnLinea = true,
                            bUsuariosChatEstaActivo = string.Equals(contacto.cContactosEstado, "Aceptado", StringComparison.OrdinalIgnoreCase)
                        };

                        if (tipoListado == "1" ||
                            (tipoListado == "2" && string.Equals(contacto.cContactosEstado, "Aceptado", StringComparison.OrdinalIgnoreCase)) ||
                            (tipoListado == "3" && !string.Equals(contacto.cContactosEstado, "Aceptado", StringComparison.OrdinalIgnoreCase)))
                        {
                            if (!usuariosEncontrados.Any(u => u.cUsuariosChatId == usuario.cUsuariosChatId))
                            {
                                usuariosEncontrados.Add(usuario);
                            }
                        }
                    }
                }

                _logger.LogInformation("Se encontraron {Count} usuarios para el término {TerminoBusqueda}", usuariosEncontrados.Count, terminoBusqueda);
                return usuariosEncontrados;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios con término {TerminoBusqueda}: {Error}", terminoBusqueda, ex.Message);
                throw;
            }
        }

        public async Task<List<SolicitudContactoDto>> ListarSolicitudesPendientesAsync(string usuarioId, string empresaId, string aplicacionId)
        {
            try
            {
                _logger.LogInformation("Listando solicitudes pendientes para usuario {UsuarioId} en empresa {EmpresaId} y aplicación {AplicacionId}",
                    usuarioId, empresaId, aplicacionId);

                var parameters = new
                {
                    pusuarioid = usuarioId,
                    pempresaid = empresaId,
                    paplicacionid = aplicacionId
                };

                // Using lowercase function name to match PostgreSQL conventions
                var result = await ExecuteStoredProcedureListAsync<SolicitudContactoDto>(
                    "usp_contactos_listarsolicitudespendientes",
                    parameters,
                    "ChatModularMicroservice");

                if (result.isSuccess && result.lstItem != null)
                {
                    return result.lstItem.Cast<SolicitudContactoDto>().ToList();
                }

                _logger.LogWarning("No se encontraron solicitudes pendientes para el usuario {UsuarioId}", usuarioId);
                return new List<SolicitudContactoDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al listar solicitudes pendientes para usuario {UsuarioId}: {Error}", usuarioId, ex.Message);
                throw;
            }
        }

        public async Task<bool> EnviarSolicitudContactoAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId, string? mensaje = null)
        {
            throw new NotImplementedException("EnviarSolicitudContactoAsync aún no está implementado en Supabase");
        }

        public async Task<ContactoDto?> AceptarSolicitudContactoAsync(string contactoId, string usuarioId)
        {
            throw new NotImplementedException("AceptarSolicitudContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> ResponderSolicitudContactoAsync(string solicitudId, string usuarioId, bool aceptada, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("ResponderSolicitudContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> BloquearContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("BloquearContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> DesbloquearContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("DesbloquearContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> EliminarContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("EliminarContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> VerificarPermisoChatDirectoAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("VerificarPermisoChatDirectoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> VerificarSonContactosAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("VerificarSonContactosAsync aún no está implementado en Supabase");
        }

        public async Task<bool> VerificarContactoBloqueadoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("VerificarContactoBloqueadoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> ExisteSolicitudPendienteAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("ExisteSolicitudPendienteAsync aún no está implementado en Supabase");
        }

        public async Task<bool> AreContactsAsync(Guid usuario1Id, Guid usuario2Id)
        {
            throw new NotImplementedException("AreContactsAsync aún no está implementado en Supabase");
        }

        public async Task<bool> HasPendingRequestAsync(Guid usuarioSolicitanteId, Guid usuarioDestinoId)
        {
            throw new NotImplementedException("HasPendingRequestAsync aún no está implementado en Supabase");
        }

        public async Task<bool> RechazarSolicitudContactoAsync(string contactoId, string usuarioId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("RechazarSolicitudContactoAsync aún no está implementado en Supabase");
        }

        public async Task<List<ContactoDto>> BuscarContactosLocalAsync(string terminoBusqueda, string empresaId, string usuarioId)
        {
            throw new NotImplementedException("BuscarContactosLocalAsync aún no está implementado en Supabase");
        }

        public async Task<bool> SincronizarContactoAsync(ContactoDto contacto, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("SincronizarContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> VerificarContactoExistenteAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("VerificarContactoExistenteAsync aún no está implementado en Supabase");
        }

        public async Task<bool> ValidarContactoLocalAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("ValidarContactoLocalAsync aún no está implementado en Supabase");
        }

        public async Task<List<ChatUsuarioDto>> BuscarUsuariosEnTablaUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado)
        {
            try
            {
                _logger.LogInformation("Buscando usuarios directamente en tabla Usuarios con término {TerminoBusqueda} para empresa {EmpresaId} y aplicación {AplicacionId}", 
                    terminoBusqueda, empresaId, aplicacionId);

                // Buscar usuarios que coincidan con el término de búsqueda
                var terminoLower = terminoBusqueda.ToLower();
                
                var query = _supabaseClient
                    .From<ChatModularMicroservice.Domain.UsuarioSupabase>()
                    .Where(x => x.cUsuariosNombre.ToLower().Contains(terminoLower) ||
                               x.cUsuariosEmail.ToLower().Contains(terminoLower) ||
                               x.cUsuariosUsername.ToLower().Contains(terminoLower));

                var qResult = await query.Get();
                var usuarios = qResult.Models ?? new List<ChatModularMicroservice.Domain.UsuarioSupabase>();

                // Convertir a ChatUsuarioDto y filtrar por tipo de listado
                var usuariosDto = usuarios
                    .Where(u => u.nUsuariosId != usuarioId) // No incluir al usuario actual
                    .Select(u => new ChatUsuarioDto
                    {
                        cUsuariosChatId = u.nUsuariosId,
                        cUsuariosChatNombre = u.cUsuariosNombre,
                        cUsuariosChatEmail = u.cUsuariosEmail,
                        bUsuariosChatEstaEnLinea = u.bUsuariosEstaEnLinea,
                        bUsuariosChatEstaActivo = u.bUsuariosActivo
                    })
                    .ToList();

                // Aplicar filtro por tipo de listado si es necesario
                if (tipoListado == "1") // Todos los usuarios
                {
                    // No filtrar, incluir todos
                }
                else if (tipoListado == "2") // Solo usuarios activos
                {
                    usuariosDto = usuariosDto.Where(u => u.bUsuariosChatEstaActivo).ToList();
                }
                else if (tipoListado == "3") // Solo usuarios en línea
                {
                    usuariosDto = usuariosDto.Where(u => u.bUsuariosChatEstaEnLinea).ToList();
                }

                _logger.LogInformation("Se encontraron {Count} usuarios en tabla Usuarios para el término {TerminoBusqueda}", usuariosDto.Count, terminoBusqueda);
                return usuariosDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar usuarios en tabla Usuarios con término {TerminoBusqueda}: {Error}", terminoBusqueda, ex.Message);
                throw;
            }
        }

        public async Task<string?> GetApplicationCodeAsync(int aplicacionId)
        {
            try
            {
                _logger.LogInformation("Obteniendo código de aplicación para ID: {AplicacionId}", aplicacionId);

                var result = await _supabaseClient
                    .From<ChatModularMicroservice.Domain.Aplicacion>()
                    .Select("c_aplicaciones_codigo")
                    .Where(x => x.nAplicacionesId == aplicacionId)
                    .Single();

                if (result != null)
                {
                    _logger.LogInformation("Código de aplicación encontrado: {Codigo}", result.cAplicacionesCodigo);
                    return result.cAplicacionesCodigo;
                }

                _logger.LogWarning("No se encontró aplicación con ID: {AplicacionId}", aplicacionId);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener código de aplicación para ID: {AplicacionId}", aplicacionId);
                return null;
            }
        }

        // Métodos estándar del patrón CRUD
        public async Task<Contacto> GetItem(ContactoFilter filter, ContactoFilterItemType filterType)
        {
            throw new NotImplementedException("GetItem aún no está implementado en Supabase");
        }

        public async Task<IEnumerable<Contacto>> GetLstItem(ContactoFilter filter, ContactoFilterListType filterType, Utils.Pagination pagination)
        {
            throw new NotImplementedException("GetLstItem aún no está implementado en Supabase");
        }

        public async Task<bool> DeleteContactoAsync(string contactoId)
        {
            throw new NotImplementedException("DeleteContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> ContactoExistsAsync(string contactoId)
        {
            throw new NotImplementedException("ContactoExistsAsync aún no está implementado en Supabase");
        }

        public async Task<bool> AddContactoAsync(Contacto contacto)
        {
            throw new NotImplementedException("AddContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> RemoveContactoAsync(string contactoId)
        {
            throw new NotImplementedException("RemoveContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> CreateContactoAsync(Contacto contacto)
        {
            throw new NotImplementedException("CreateContactoAsync aún no está implementado en Supabase");
        }

        public async Task<bool> UpdateContactoAsync(Contacto contacto)
        {
            throw new NotImplementedException("UpdateContactoAsync aún no está implementado en Supabase");
        }

        // Métodos CRUD básicos
        public async Task<int> Insert(Contacto item)
        {
            throw new NotImplementedException("Insert aún no está implementado en Supabase");
        }

        public async Task<bool> Update(Contacto item)
        {
            throw new NotImplementedException("Update aún no está implementado en Supabase");
        }

        public async Task<bool> DeleteEntero(int nContactosId)
        {
            throw new NotImplementedException("DeleteEntero aún no está implementado en Supabase");
        }

        public async Task<List<object>> SearchUsuariosAsync(string searchTerm, string empresaId, string aplicacionId)
        {
            throw new NotImplementedException("SearchUsuariosAsync aún no está implementado en Supabase");
        }
    }
}