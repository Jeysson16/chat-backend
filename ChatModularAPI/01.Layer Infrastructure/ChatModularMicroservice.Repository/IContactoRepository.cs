using System;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.Models;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de contactos en el dominio
/// </summary>
public interface IContactoRepository : IDeleteIntRepository, IInsertIntRepository<Contacto>, IUpdateRepository<Contacto>
{
    Task<List<ChatUsuarioDto>> BuscarUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado);
    
    /// <summary>
    /// Busca usuarios directamente en la tabla Usuarios (no en contactos)
    /// </summary>
    /// <param name="terminoBusqueda">Término de búsqueda</param>
    /// <param name="usuarioId">ID del usuario que busca (para excluirse)</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <param name="tipoListado">Tipo de listado (1=Todos, 2=Activos, 3=En línea)</param>
    /// <returns>Lista de usuarios encontrados</returns>
    Task<List<ChatUsuarioDto>> BuscarUsuariosEnTablaUsuariosAsync(string terminoBusqueda, string usuarioId, string empresaId, string aplicacionId, string tipoListado);
    Task<List<SolicitudContactoDto>> ListarSolicitudesPendientesAsync(string usuarioId, string empresaId, string aplicacionId);
    Task<List<ContactoDto>> ListarContactosAsync(string usuarioId, string empresaId, string aplicacionId);
    Task<bool> EnviarSolicitudContactoAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId, string? mensaje = null);
    Task<ContactoDto?> AceptarSolicitudContactoAsync(string contactoId, string usuarioId);
    Task<bool> ResponderSolicitudContactoAsync(string solicitudId, string usuarioId, bool aceptada, string empresaId, string aplicacionId);
    Task<bool> BloquearContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);
    Task<bool> DesbloquearContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);
    Task<bool> EliminarContactoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);
    Task<bool> VerificarPermisoChatDirectoAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId);
    Task<bool> VerificarSonContactosAsync(string usuario1Id, string usuario2Id, string empresaId, string aplicacionId);
    Task<bool> VerificarContactoBloqueadoAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);
    Task<bool> ExisteSolicitudPendienteAsync(string usuarioSolicitanteId, string usuarioDestinoId, string empresaId, string aplicacionId);
    Task<bool> AreContactsAsync(Guid usuario1Id, Guid usuario2Id);
    Task<bool> HasPendingRequestAsync(Guid usuarioSolicitanteId, Guid usuarioDestinoId);
    Task<bool> RechazarSolicitudContactoAsync(string contactoId, string usuarioId, string empresaId, string aplicacionId);
    Task<List<ContactoDto>> ListarContactosPorUsuarioAsync(string usuarioId, string empresaId, string aplicacionId, string? estado = null);

    // === NUEVOS MÉTODOS PARA MODOS DE GESTIÓN DE CONTACTOS ===
    
    /// <summary>
    /// Busca contactos en el sistema local
    /// </summary>
    /// <param name="terminoBusqueda">Término de búsqueda</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="usuarioId">ID del usuario que busca</param>
    /// <returns>Lista de contactos encontrados</returns>
    Task<List<ContactoDto>> BuscarContactosLocalAsync(string terminoBusqueda, string empresaId, string usuarioId);

    /// <summary>
    /// Sincroniza un contacto desde API externa al sistema local
    /// </summary>
    /// <param name="contacto">Contacto a sincronizar</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>True si se sincronizó correctamente</returns>
    Task<bool> SincronizarContactoAsync(ContactoDto contacto, string empresaId, string aplicacionId);

    /// <summary>
    /// Verifica si un contacto ya existe en el sistema
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <param name="contactoId">ID del contacto</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>True si el contacto ya existe</returns>
    Task<bool> VerificarContactoExistenteAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);

    /// <summary>
    /// Valida si un contacto existe en el sistema local
    /// </summary>
    /// <param name="usuarioId">ID del usuario</param>
    /// <param name="contactoId">ID del contacto</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>True si el contacto existe localmente</returns>
    Task<bool> ValidarContactoLocalAsync(string usuarioId, string contactoId, string empresaId, string aplicacionId);

    /// <summary>
    /// Obtiene el código de aplicación por su ID
    /// </summary>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Código de la aplicación o null si no se encuentra</returns>
    Task<string?> GetApplicationCodeAsync(int aplicacionId);

    // === MÉTODOS ESTÁNDAR DEL PATRÓN ===
    
    /// <summary>
    /// Obtiene un contacto específico según el filtro y tipo de filtro
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <returns>Contacto encontrado</returns>
    Task<Contacto> GetItem(ContactoFilter filter, ContactoFilterItemType filterType);

    /// <summary>
    /// Obtiene una lista de contactos según el filtro, tipo de filtro y paginación
    /// </summary>
    /// <param name="filter">Filtro de búsqueda</param>
    /// <param name="filterType">Tipo de filtro a aplicar</param>
    /// <param name="pagination">Configuración de paginación</param>
    /// <returns>Lista de contactos encontrados</returns>
    Task<IEnumerable<Contacto>> GetLstItem(ContactoFilter filter, ContactoFilterListType filterType, Utils.Pagination pagination);
    
    /// <summary>
    /// Elimina un contacto específico
    /// </summary>
    /// <param name="contactoId">ID del contacto a eliminar</param>
    /// <returns>True si se eliminó correctamente, false en caso contrario</returns>
    Task<bool> DeleteContactoAsync(string contactoId);
    
    /// <summary>
    /// Busca usuarios en el sistema
    /// </summary>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <param name="empresaId">ID de la empresa</param>
    /// <param name="aplicacionId">ID de la aplicación</param>
    /// <returns>Lista de usuarios encontrados</returns>
    Task<List<object>> SearchUsuariosAsync(string searchTerm, string empresaId, string aplicacionId);
    
    /// <summary>
    /// Verifica si un contacto existe
    /// </summary>
    /// <param name="contactoId">ID del contacto</param>
    /// <returns>True si el contacto existe, false en caso contrario</returns>
    Task<bool> ContactoExistsAsync(string contactoId);
    
    /// <summary>
    /// Agrega un nuevo contacto
    /// </summary>
    /// <param name="contacto">Contacto a agregar</param>
    /// <returns>True si se agregó correctamente, false en caso contrario</returns>
    Task<bool> AddContactoAsync(Contacto contacto);
    
    /// <summary>
    /// Remueve un contacto
    /// </summary>
    /// <param name="contactoId">ID del contacto a remover</param>
    /// <returns>True si se removió correctamente, false en caso contrario</returns>
    Task<bool> RemoveContactoAsync(string contactoId);
    
    /// <summary>
    /// Crea un nuevo contacto
    /// </summary>
    /// <param name="contacto">Contacto a crear</param>
    /// <returns>True si se creó correctamente, false en caso contrario</returns>
    Task<bool> CreateContactoAsync(Contacto contacto);
    
    /// <summary>
    /// Actualiza un contacto existente
    /// </summary>
    /// <param name="contacto">Contacto a actualizar</param>
    /// <returns>True si se actualizó correctamente, false en caso contrario</returns>
    Task<bool> UpdateContactoAsync(Contacto contacto);
}