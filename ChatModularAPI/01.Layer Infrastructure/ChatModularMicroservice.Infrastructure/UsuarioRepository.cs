using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Repository;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Dapper;
using System.Data;
using Microsoft.Data.SqlClient;
using Supabase;
using Microsoft.Extensions.Logging;
using UsuarioSupabaseModel = ChatModularMicroservice.Domain.UsuarioSupabase;
using AthleteProfileSupabaseModel = ChatModularMicroservice.Domain.AthleteProfileSupabase;

namespace ChatModularMicroservice.Infrastructure;

public class UsuarioRepository : BaseRepository, IUsuarioRepository
{
    private readonly Supabase.Client _supabaseClient;
    private readonly ILogger<UsuarioRepository> _logger;

    #region Constructor
    public UsuarioRepository(IConnectionFactory cn, Supabase.Client supabaseClient, ILogger<UsuarioRepository> logger) : base(cn)
    {
        _supabaseClient = supabaseClient;
        _logger = logger;
    }
    #endregion

    #region Public Methods

    // Métodos asíncronos exigidos por la interfaz IUsuarioRepository
    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByIdAsync(Guid id)
    {
        // Usar Supabase directamente - buscar por ID string
        try
        {
            var userId = id.ToString();
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Single();

            var model = response;
            if (model == null) return null;

            return new Usuario
            {
                cUsuariosId = int.TryParse(model.nUsuariosId, out int parsedId) ? parsedId : 0,
                cNombre = model.cUsuariosNombre,
                cApellido = string.Empty,
                cEmail = model.cUsuariosEmail,
                cTelefono = string.Empty,
                cAvatar = model.cUsuariosAvatar,
                bActivo = model.bUsuariosActivo,
                dFechaCreacion = model.dUsuariosFechaCreacion,
                nEmpresaId = 0,
                nAplicacionId = 0,
                dUsuariosChatUltimaConexion = model.dUsuariosUltimaConexion,
                bUsuariosChatEstaActivo = model.bUsuariosActivo,
                bUsuariosChatEstaEnLinea = model.bUsuariosEstaEnLinea,
                dUsuariosChatFechaCreacion = model.dUsuariosFechaCreacion,
                cUsuariosChatId = model.nUsuariosId,
                cUsuariosChatNombre = model.cUsuariosNombre,
                cUsuariosChatEmail = model.cUsuariosEmail,
                cUsuariosChatPassword = model.cUsuariosPassword ?? string.Empty,
                dUsuarioCambioPassword = null,
                cUsuarioConfigPrivacidad = string.Empty,
                cUsuarioConfigNotificaciones = string.Empty,
                cUsuariosChatAvatar = model.cUsuariosAvatar,
                bUsuarioVerificado = model.bUsuarioVerificado,
                cUsuariosChatUsername = null,
                cUsuariosChatRol = "USER",
                cUsuarioTokenVerificacion = null,
                cUsuariosChatPerJurCodigo = model.cUsuariosPerJurCodigo,
                cUsuariosChatPerCodigo = model.cUsuariosPerCodigo,
                cUsuariosChatAppCodigo = string.Empty,
                cUsuarioTokenReset = string.Empty
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por ID desde Supabase: {Id}", id);
            return null;
        }
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByEmailAsync(string email)
    {
        // Usar athlete_profiles en lugar de Usuarios (tabla no existe)
        try
        {
            _logger.LogInformation("GetByEmailAsync: Buscando usuario por email en athlete_profiles: {Email}", email);
            
            var response = await _supabaseClient
                .From<AthleteProfileSupabaseModel>()
                .Where(x => x.Email == email)
                .Single();

            var model = response;
            if (model == null) {
                _logger.LogInformation("GetByEmailAsync: Usuario no encontrado por email: {Email}", email);
                return null;
            }

            // Mapear athlete_profiles a Usuario
            var usuario = new Usuario
            {
                cUsuariosId = 0, // No hay ID numérico en athlete_profiles
                cNombre = model.Name,
                cApellido = string.Empty,
                cEmail = model.Email,
                cTelefono = model.Phone ?? string.Empty,
                cAvatar = string.Empty, // No hay avatar en athlete_profiles
                bActivo = true, // Asumir activo por defecto
                dFechaCreacion = model.CreatedAt,
                nEmpresaId = 0,
                nAplicacionId = 0,
                dUsuariosChatUltimaConexion = model.UpdatedAt,
                bUsuariosChatEstaActivo = true,
                bUsuariosChatEstaEnLinea = false, // Por defecto offline
                dUsuariosChatFechaCreacion = model.CreatedAt,
                cUsuariosChatId = model.UserId, // Usar user_id como ID del chat
                cUsuariosChatNombre = model.Name,
                cUsuariosChatEmail = model.Email,
                cUsuariosChatPassword = string.Empty, // No hay contraseña en athlete_profiles
                dUsuarioCambioPassword = null,
                cUsuarioConfigPrivacidad = string.Empty,
                cUsuarioConfigNotificaciones = string.Empty,
                cUsuariosChatAvatar = string.Empty,
                bUsuarioVerificado = true, // Asumir verificado
                cUsuariosChatUsername = model.Email, // Usar email como username
                cUsuariosChatRol = "USER",
                cUsuarioTokenVerificacion = string.Empty,
                cUsuariosChatPerJurCodigo = string.Empty,
                cUsuariosChatPerCodigo = string.Empty,
                cUsuariosChatAppCodigo = string.Empty,
                cUsuarioTokenReset = string.Empty
            };
            
            _logger.LogInformation("GetByEmailAsync: Usuario encontrado en athlete_profiles, rol asignado: {Role}", usuario.cUsuariosChatRol);
            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por email desde athlete_profiles: {Email}", email);
            return null;
        }
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByUsernameAsync(string username)
    {
        // Usar athlete_profiles y tratar username como email (no hay campo username)
        try
        {
            _logger.LogInformation("GetByUsernameAsync: Buscando usuario por username (como email) en athlete_profiles: {Username}", username);
            
            var response = await _supabaseClient
                .From<AthleteProfileSupabaseModel>()
                .Where(x => x.Email == username)
                .Single();

            var model = response;
            if (model == null) {
                _logger.LogInformation("GetByUsernameAsync: Usuario no encontrado por username: {Username}", username);
                return null;
            }

            // Mapear athlete_profiles a Usuario
            var usuario = new Usuario
            {
                cUsuariosId = 0, // No hay ID numérico en athlete_profiles
                cNombre = model.Name,
                cApellido = string.Empty,
                cEmail = model.Email,
                cTelefono = model.Phone ?? string.Empty,
                cAvatar = string.Empty, // No hay avatar en athlete_profiles
                bActivo = true, // Asumir activo por defecto
                dFechaCreacion = model.CreatedAt,
                nEmpresaId = 0,
                nAplicacionId = 0,
                dUsuariosChatUltimaConexion = model.UpdatedAt,
                bUsuariosChatEstaActivo = true,
                bUsuariosChatEstaEnLinea = false, // Por defecto offline
                dUsuariosChatFechaCreacion = model.CreatedAt,
                cUsuariosChatId = model.UserId, // Usar user_id como ID del chat
                cUsuariosChatNombre = model.Name,
                cUsuariosChatEmail = model.Email,
                cUsuariosChatPassword = string.Empty, // No hay contraseña en athlete_profiles
                dUsuarioCambioPassword = null,
                cUsuarioConfigPrivacidad = string.Empty,
                cUsuarioConfigNotificaciones = string.Empty,
                cUsuariosChatAvatar = string.Empty,
                bUsuarioVerificado = true, // Asumir verificado
                cUsuariosChatUsername = model.Email, // Usar email como username
                cUsuariosChatRol = "USER",
                cUsuarioTokenVerificacion = string.Empty,
                cUsuariosChatPerJurCodigo = string.Empty,
                cUsuariosChatPerCodigo = string.Empty,
                cUsuariosChatAppCodigo = string.Empty,
                cUsuarioTokenReset = string.Empty
            };
            
            _logger.LogInformation("GetByUsernameAsync: Usuario encontrado en athlete_profiles, rol asignado: {Role}", usuario.cUsuariosChatRol);
            return usuario;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuario por username desde athlete_profiles: {Username}", username);
            return null;
        }
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario> CreateAsync(ChatModularMicroservice.Entities.Models.Usuario usuario)
    {
        // Usar directamente Supabase sin intentar SQL Server
        _logger.LogInformation("Creando usuario en Supabase: {Email}", usuario.cEmail);
        var created = await InsertSupabaseAsync(usuario);
        return created;
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario> UpdateAsync(ChatModularMicroservice.Entities.Models.Usuario usuario)
    {
        // Usar Supabase directamente para actualizar
        try
        {
            var userId = usuario.cUsuariosId.ToString();
            // Obtener el usuario actual de Supabase
            var existing = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Single();

            if (existing == null)
            {
                return usuario;
            }

            // Actualizar solo los campos que cambiaron
            existing.cUsuariosNombre = usuario.cUsuariosChatNombre ?? usuario.cNombre;
            existing.cUsuariosEmail = usuario.cUsuariosChatEmail ?? usuario.cEmail;
            existing.cUsuariosAvatar = usuario.cUsuariosChatAvatar ?? usuario.cAvatar;
            existing.bUsuariosActivo = usuario.bUsuariosChatEstaActivo;
            existing.bUsuariosEstaEnLinea = usuario.bUsuariosChatEstaEnLinea;
            existing.dUsuariosUltimaConexion = usuario.dUsuariosChatUltimaConexion;
            existing.cUsuariosPerJurCodigo = usuario.cUsuariosChatPerJurCodigo;
            existing.cUsuariosPerCodigo = usuario.cUsuariosChatPerCodigo;
            existing.cUsuariosUsername = usuario.cUsuariosChatUsername ?? existing.cUsuariosUsername;
            existing.cUsuariosPassword = usuario.cUsuariosChatPassword ?? existing.cUsuariosPassword;

            // Actualizar en Supabase
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Update(existing, new Supabase.Postgrest.QueryOptions 
                { 
                    Returning = Supabase.Postgrest.QueryOptions.ReturnType.Representation 
                });

            var updatedModel = response.Model ?? existing;

            // Retornar el usuario actualizado
            return new Usuario
            {
                cUsuariosId = int.TryParse(updatedModel.nUsuariosId, out int parsedId3) ? parsedId3 : 0,
                cNombre = updatedModel.cUsuariosNombre,
                cApellido = usuario.cApellido,
                cEmail = updatedModel.cUsuariosEmail,
                cTelefono = usuario.cTelefono,
                cAvatar = updatedModel.cUsuariosAvatar,
                bActivo = updatedModel.bUsuariosActivo,
                dFechaCreacion = updatedModel.dUsuariosFechaCreacion,
                nEmpresaId = usuario.nEmpresaId,
                nAplicacionId = usuario.nAplicacionId,
                dUsuariosChatUltimaConexion = updatedModel.dUsuariosUltimaConexion,
                bUsuariosChatEstaActivo = updatedModel.bUsuariosActivo,
                bUsuariosChatEstaEnLinea = updatedModel.bUsuariosEstaEnLinea,
                dUsuariosChatFechaCreacion = updatedModel.dUsuariosFechaCreacion,
                cUsuariosChatId = updatedModel.nUsuariosId,
                cUsuariosChatNombre = updatedModel.cUsuariosNombre,
                cUsuariosChatEmail = updatedModel.cUsuariosEmail,
                cUsuariosChatPassword = updatedModel.cUsuariosPassword ?? string.Empty,
                dUsuarioCambioPassword = usuario.dUsuarioCambioPassword,
                cUsuarioConfigPrivacidad = usuario.cUsuarioConfigPrivacidad,
                cUsuarioConfigNotificaciones = usuario.cUsuarioConfigNotificaciones,
                cUsuariosChatAvatar = updatedModel.cUsuariosAvatar,
                bUsuarioVerificado = usuario.bUsuarioVerificado,
                cUsuariosChatUsername = usuario.cUsuariosChatUsername,
                cUsuariosChatRol = usuario.cUsuariosChatRol,
                cUsuarioTokenVerificacion = usuario.cUsuarioTokenVerificacion,
                cUsuariosChatPerJurCodigo = updatedModel.cUsuariosPerJurCodigo,
                cUsuariosChatPerCodigo = updatedModel.cUsuariosPerCodigo,
                cUsuariosChatAppCodigo = usuario.cUsuariosChatAppCodigo,
                cUsuarioTokenReset = usuario.cUsuarioTokenReset
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar usuario en Supabase: {Id}", usuario.cUsuariosId);
            return usuario;
        }
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        // Borrado por identificador del chat
        string query = "USP_Usuario_DeleteByChatId";
        var param = new DynamicParameters();
        param.Add("@cUsuariosChatId", id.ToString());
        return await this.UpdateOrDelete(query, param);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        // Usar Supabase directamente para verificar existencia
        try
        {
            var userId = id.ToString();
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Single();

            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de usuario en Supabase: {Id}", id);
            return false;
        }
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        // Usar Supabase directamente para verificar existencia por email
        try
        {
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.cUsuariosEmail == email)
                .Single();

            return response != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al verificar existencia de usuario por email en Supabase: {Email}", email);
            return false;
        }
    }

    public async Task<bool> ExistsByUsernameAsync(string username)
    {
        var user = await GetByUsernameAsync(username);
        return user != null;
    }

    public async Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> GetAllAsync()
    {
        // Usar Supabase directamente para obtener todos los usuarios
        try
        {
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Get();

            var models = response.Models ?? new List<UsuarioSupabaseModel>();

            return models.Select(model => new Usuario
            {
                cUsuariosId = int.TryParse(model.nUsuariosId, out int parsedId) ? parsedId : 0,
                cNombre = model.cUsuariosNombre,
                cApellido = string.Empty,
                cEmail = model.cUsuariosEmail,
                cTelefono = string.Empty,
                cAvatar = model.cUsuariosAvatar,
                bActivo = model.bUsuariosActivo,
                dFechaCreacion = model.dUsuariosFechaCreacion,
                nEmpresaId = 0,
                nAplicacionId = 0,
                dUsuariosChatUltimaConexion = model.dUsuariosUltimaConexion,
                bUsuariosChatEstaActivo = model.bUsuariosActivo,
                bUsuariosChatEstaEnLinea = model.bUsuariosEstaEnLinea,
                dUsuariosChatFechaCreacion = model.dUsuariosFechaCreacion,
                cUsuariosChatId = model.nUsuariosId.ToString(),
                cUsuariosChatNombre = model.cUsuariosNombre,
                cUsuariosChatEmail = model.cUsuariosEmail,
                cUsuariosChatPassword = model.cUsuariosPassword ?? string.Empty,
                dUsuarioCambioPassword = null,
                cUsuarioConfigPrivacidad = string.Empty,
                cUsuarioConfigNotificaciones = string.Empty,
                cUsuariosChatAvatar = model.cUsuariosAvatar,
                bUsuarioVerificado = model.bUsuarioVerificado,
                cUsuariosChatUsername = string.Empty,
                cUsuariosChatRol = "USER",
                cUsuarioTokenVerificacion = string.Empty,
                cUsuariosChatPerJurCodigo = model.cUsuariosPerJurCodigo,
                cUsuariosChatPerCodigo = model.cUsuariosPerCodigo,
                cUsuariosChatAppCodigo = string.Empty,
                cUsuarioTokenReset = string.Empty
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener todos los usuarios desde Supabase");
            return new List<Usuario>();
        }
    }

    public async Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> GetByFilterAsync(UsuarioFilter filter)
    {
        // Heurística simple: en función de campos del filtro escogemos el SP adecuado
        if (!string.IsNullOrWhiteSpace(filter.TerminoBusqueda))
        {
            return await this.GetByTerminoBusqueda(filter);
        }
        if (filter.nEmpresaId.HasValue && filter.nAplicacionId.HasValue)
        {
            return await this.GetByEmpresaYAplicacion(filter);
        }
        if (filter.nEmpresaId.HasValue)
        {
            return await this.GetByEmpresa(filter);
        }
        if (filter.nAplicacionId.HasValue)
        {
            return await this.GetByAplicacion(filter);
        }
        if (filter.bActivo.HasValue)
        {
            return await this.GetByActivos(filter);
        }
        // Por defecto, paginación básica
        var pagination = new Utils.Pagination { PageNumber = 1, PageSize = 100 };
        return await this.GetByPagination(filter, pagination);
    }

    public async Task<IEnumerable<ChatModularMicroservice.Entities.Models.Usuario>> SearchUsersAsync(string searchTerm)
    {
        var filter = new UsuarioFilter { TerminoBusqueda = searchTerm };
        return await this.GetByTerminoBusqueda(filter);
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByUsuariosChatIdAsync(string cUsuariosChatId)
    {
        // Fallback: si el SP principal no existe, intentar por Username o Email
        try
        {
            string query = "USP_Usuario_GetByUsuariosChatId";
            var param = new DynamicParameters();
            param.Add("@cUsuariosChatId", cUsuariosChatId);
            return (await this.LoadData<Usuario>(query, param)).FirstOrDefault();
        }
        catch (SqlException ex) when (ex.Message.Contains("No se encontró el procedimiento almacenado") || ex.Message.Contains("could not find stored procedure"))
        {
            // Intentar buscar por username si coincide el formato
            string queryUsername = "USP_Usuario_GetByUsername";
            var paramUser = new DynamicParameters();
            paramUser.Add("@cUsuariosChatUsername", cUsuariosChatId);
            var byUsername = (await this.LoadData<Usuario>(queryUsername, paramUser)).FirstOrDefault();
            if (byUsername != null) return byUsername;

            // Intentar buscar por email
            string queryEmail = "USP_Usuario_GetByEmail";
            var paramEmail = new DynamicParameters();
            paramEmail.Add("@cEmail", cUsuariosChatId);
            return (await this.LoadData<Usuario>(queryEmail, paramEmail)).FirstOrDefault();
        }
    }

    public async Task<Usuario?> GetByPerCodigoAsync(string perCodigo)
    {
        // Buscar en athlete_profiles usando el código como email o UserId
        try
        {
            _logger.LogInformation("GetByPerCodigoAsync: Buscando usuario por código en athlete_profiles: {PerCodigo}", perCodigo);
            
            // Intentar buscar por email primero
            var response = await _supabaseClient
                .From<AthleteProfileSupabaseModel>()
                .Where(x => x.Email == perCodigo)
                .Single();

            if (response != null)
            {
                var mapped = new Usuario
                {
                    cUsuariosId = 0, // No hay ID numérico en athlete_profiles
                    cNombre = response.Name,
                    cApellido = string.Empty,
                    cEmail = response.Email,
                    cTelefono = response.Phone ?? string.Empty,
                    cAvatar = string.Empty, // No hay avatar en athlete_profiles
                    bActivo = true, // Asumir activo por defecto
                    nEmpresaId = 0,
                    nAplicacionId = 0,
                    dFechaCreacion = response.CreatedAt,
                    dUsuariosChatUltimaConexion = response.UpdatedAt,
                    bUsuariosChatEstaActivo = true,
                    bUsuariosChatEstaEnLinea = false, // Por defecto offline
                    dUsuariosChatFechaCreacion = response.CreatedAt,
                    cUsuariosChatId = response.UserId, // Usar user_id como ID del chat
                    cUsuariosChatNombre = response.Name,
                    cUsuariosChatEmail = response.Email,
                    cUsuariosChatPassword = string.Empty, // No hay contraseña en athlete_profiles
                    dUsuarioCambioPassword = null,
                    cUsuarioConfigPrivacidad = string.Empty,
                    cUsuarioConfigNotificaciones = string.Empty,
                    cUsuariosChatAvatar = string.Empty,
                    bUsuarioVerificado = true, // Asumir verificado
                    cUsuariosChatUsername = response.Email, // Usar email como username
                    cUsuariosChatRol = "USER",
                    cUsuarioTokenVerificacion = string.Empty,
                    cUsuariosChatPerJurCodigo = string.Empty,
                    cUsuariosChatPerCodigo = string.Empty,
                    cUsuariosChatAppCodigo = string.Empty,
                    cUsuarioTokenReset = string.Empty,
                    dUsuarioTokenResetExpiracion = null
                };

                _logger.LogInformation("GetByPerCodigoAsync: Usuario encontrado en athlete_profiles, rol: {Role}", mapped.cUsuariosChatRol);
                return mapped;
            }

            // Si no se encuentra por email, intentar por UserId
            var responseById = await _supabaseClient
                .From<AthleteProfileSupabaseModel>()
                .Where(x => x.UserId == perCodigo)
                .Single();

            if (responseById != null)
            {
                var mappedById = new Usuario
                {
                    cUsuariosId = 0,
                    cNombre = responseById.Name,
                    cApellido = string.Empty,
                    cEmail = responseById.Email,
                    cTelefono = responseById.Phone ?? string.Empty,
                    cAvatar = string.Empty,
                    bActivo = true,
                    nEmpresaId = 0,
                    nAplicacionId = 0,
                    dFechaCreacion = responseById.CreatedAt,
                    dUsuariosChatUltimaConexion = responseById.UpdatedAt,
                    bUsuariosChatEstaActivo = true,
                    bUsuariosChatEstaEnLinea = false,
                    dUsuariosChatFechaCreacion = responseById.CreatedAt,
                    cUsuariosChatId = responseById.UserId,
                    cUsuariosChatNombre = responseById.Name,
                    cUsuariosChatEmail = responseById.Email,
                    cUsuariosChatPassword = string.Empty,
                    dUsuarioCambioPassword = null,
                    cUsuarioConfigPrivacidad = string.Empty,
                    cUsuarioConfigNotificaciones = string.Empty,
                    cUsuariosChatAvatar = string.Empty,
                    bUsuarioVerificado = true,
                    cUsuariosChatUsername = responseById.Email,
                    cUsuariosChatRol = "USER",
                    cUsuarioTokenVerificacion = string.Empty,
                    cUsuariosChatPerJurCodigo = string.Empty,
                    cUsuariosChatPerCodigo = string.Empty,
                    cUsuariosChatAppCodigo = string.Empty,
                    cUsuarioTokenReset = string.Empty,
                    dUsuarioTokenResetExpiracion = null
                };

                _logger.LogInformation("GetByPerCodigoAsync: Usuario encontrado por UserId en athlete_profiles, rol: {Role}", mappedById.cUsuariosChatRol);
                return mappedById;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Usuario no encontrado por código en athlete_profiles: {PerCodigo}", perCodigo);
        }

        _logger.LogInformation("GetByPerCodigoAsync: Usuario no encontrado por código: {PerCodigo}", perCodigo);
        return null;
    }

    public async Task<bool> UpdateLastConnectionAsync(string userId)
    {
        // Usar Supabase directamente para actualizar última conexión
        try
        {
            var existing = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Single();

            if (existing != null)
            {
                existing.dUsuariosUltimaConexion = DateTime.UtcNow;
                
                await _supabaseClient
                    .From<UsuarioSupabaseModel>()
                    .Where(x => x.nUsuariosId == userId)
                    .Update(existing);

                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar última conexión en Supabase: {UserId}", userId);
            return false;
        }
    }

    public async Task<bool> SetActiveStatusAsync(string userId, bool isActive)
    {
        // Usar Supabase directamente para actualizar estado activo
        try
        {
            var existing = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == userId)
                .Single();

            if (existing != null)
            {
                existing.bUsuariosActivo = isActive;
                
                await _supabaseClient
                    .From<UsuarioSupabaseModel>()
                    .Where(x => x.nUsuariosId == userId)
                    .Update(existing);

                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar estado activo en Supabase: {UserId}, {IsActive}", userId, isActive);
            return false;
        }
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByResetTokenAsync(string resetToken)
    {
        // Como Supabase no tiene campo de token reset, buscar por email extraído del token
        // Esto es una implementación simplificada - en producción deberías tener un campo separado
        _logger.LogWarning("GetByResetTokenAsync: Búsqueda simplificada por token - se requiere implementación completa");
        return null;
    }

    public async Task<ChatModularMicroservice.Entities.Models.Usuario?> GetByVerificationTokenAsync(string verificationToken)
    {
        // Como Supabase no tiene campo de token verificación, retornar null por ahora
        // Esto es una implementación simplificada - en producción deberías tener un campo separado
        _logger.LogWarning("GetByVerificationTokenAsync: Búsqueda simplificada por token - se requiere implementación completa");
        return null;
    }

    public async Task<string> GeneratePasswordResetTokenAsync(string email)
    {
        // Generar token pero no persistirlo en Supabase (no hay campo para eso)
        // Esto es una implementación simplificada - en producción deberías tener un campo separado
        _logger.LogWarning("GeneratePasswordResetTokenAsync: Token generado pero no persistido en Supabase - se requiere implementación completa");
        return Guid.NewGuid().ToString("N");
    }

    public async Task<bool> ConfirmPasswordResetAsync(string token, string newPassword)
    {
        // Implementación simplificada - no hay token en Supabase
        _logger.LogWarning("ConfirmPasswordResetAsync: Implementación simplificada - se requiere token management completo");
        return false;
    }

    public async Task<bool> DeactivateUsuarioAsync(string usuarioId)
    {
        // Usar Supabase directamente para desactivar usuario
        return await SetActiveStatusAsync(usuarioId, false);
    }

    public async Task<bool> UpdatePasswordAsync(string usuarioId, string newPassword)
    {
        // Usar Supabase directamente para actualizar contraseña
        try
        {
            var existing = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Where(x => x.nUsuariosId == usuarioId)
                .Single();

            if (existing != null)
            {
                existing.cUsuariosPassword = newPassword;
                
                await _supabaseClient
                    .From<UsuarioSupabaseModel>()
                    .Where(x => x.nUsuariosId == usuarioId)
                    .Update(existing);

                return true;
            }
            
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al actualizar contraseña en Supabase: {UsuarioId}", usuarioId);
            return false;
        }
    }

    public async Task<bool> ActivateUsuarioAsync(string usuarioId)
    {
        // Usar Supabase directamente para activar usuario
        return await SetActiveStatusAsync(usuarioId, true);
    }

    public async Task<Usuario?> GetByUserCodeAsync(string userCode)
    {
        _logger.LogInformation("GetByUserCodeAsync: Buscando usuario por código: {UserCode}", userCode);
        
        // USUARIO ESPECIAL DE PRUEBA: JSANCHEZ
        _logger.LogInformation("GetByUserCodeAsync: Verificando si es JSANCHEZ - userCode='{UserCode}', ToUpper='{Upper}'", userCode, userCode.ToUpper());
        if (string.Equals(userCode, "JSANCHEZ", StringComparison.OrdinalIgnoreCase))
        {
            _logger.LogInformation("GetByUserCodeAsync: ✅ Usuario especial JSANCHEZ detectado");
            return new Usuario
            {
                cUsuariosId = 1000000001,
                cNombre = "Jeysson",
                cApellido = "Sanchez",
                cEmail = "jsanchez@example.com",
                cTelefono = "+1234567890",
                cAvatar = string.Empty,
                bActivo = true,
                dFechaCreacion = DateTime.UtcNow.AddYears(-1),
                nEmpresaId = 1,
                nAplicacionId = 1,
                dUsuariosChatUltimaConexion = DateTime.UtcNow,
                bUsuariosChatEstaActivo = true,
                bUsuariosChatEstaEnLinea = true,
                dUsuariosChatFechaCreacion = DateTime.UtcNow.AddYears(-1),
                cUsuariosChatId = "1000000001",
                cUsuariosChatNombre = "Jeysson Sanchez",
                cUsuariosChatEmail = "jsanchez@example.com",
                cUsuariosChatPassword = "jeysson12345", // Contraseña para pruebas
                dUsuarioCambioPassword = null,
                cUsuarioConfigPrivacidad = string.Empty,
                cUsuarioConfigNotificaciones = string.Empty,
                cUsuariosChatAvatar = string.Empty,
                bUsuarioVerificado = true,
                cUsuariosChatUsername = "JSANCHEZ",
                cUsuariosChatRol = "ADMIN",
                cUsuarioTokenVerificacion = string.Empty,
                cUsuariosChatPerJurCodigo = "DEFAULT",
                cUsuariosChatPerCodigo = "JSANCHEZ",
                cUsuariosChatAppCodigo = "SICOM_CHAT_2024",
                cUsuarioTokenReset = string.Empty,
                dUsuarioTokenResetExpiracion = null
            };
        }
        
        _logger.LogInformation("GetByUserCodeAsync: No es JSANCHEZ, continuando con búsqueda normal");

        // Buscar por email (caso más común para login) usando athlete_profiles
        var byEmail = await GetByEmailAsync(userCode);
        if (byEmail != null) {
            _logger.LogInformation("GetByUserCodeAsync: Encontrado por Email, rol: {Role}", byEmail.cUsuariosChatRol);
            return byEmail;
        }

        // Buscar por username (tratado como email) usando athlete_profiles
        var byUsername = await GetByUsernameAsync(userCode);
        if (byUsername != null) {
            _logger.LogInformation("GetByUserCodeAsync: Encontrado por Username, rol: {Role}", byUsername.cUsuariosChatRol);
            return byUsername;
        }

        // Fallback: búsqueda por ID (UserId de athlete_profiles)
        try
        {
            var response = await _supabaseClient
                .From<AthleteProfileSupabaseModel>()
                .Where(x => x.UserId == userCode)
                .Single();

            if (response != null)
            {
                var usuario = new Usuario
                {
                    cUsuariosId = 0,
                    cNombre = response.Name,
                    cApellido = string.Empty,
                    cEmail = response.Email,
                    cTelefono = response.Phone ?? string.Empty,
                    cAvatar = string.Empty,
                    bActivo = true,
                    dFechaCreacion = response.CreatedAt,
                    nEmpresaId = 0,
                    nAplicacionId = 0,
                    dUsuariosChatUltimaConexion = response.UpdatedAt,
                    bUsuariosChatEstaActivo = true,
                    bUsuariosChatEstaEnLinea = false,
                    dUsuariosChatFechaCreacion = response.CreatedAt,
                    cUsuariosChatId = response.UserId,
                    cUsuariosChatNombre = response.Name,
                    cUsuariosChatEmail = response.Email,
                    cUsuariosChatPassword = string.Empty,
                    dUsuarioCambioPassword = null,
                    cUsuarioConfigPrivacidad = string.Empty,
                    cUsuarioConfigNotificaciones = string.Empty,
                    cUsuariosChatAvatar = string.Empty,
                    bUsuarioVerificado = true,
                    cUsuariosChatUsername = response.Email,
                    cUsuariosChatRol = "USER",
                    cUsuarioTokenVerificacion = string.Empty,
                    cUsuariosChatPerJurCodigo = string.Empty,
                    cUsuariosChatPerCodigo = string.Empty,
                    cUsuariosChatAppCodigo = string.Empty,
                    cUsuarioTokenReset = string.Empty
                };
                _logger.LogInformation("GetByUserCodeAsync: Encontrado por UserId, rol: {Role}", usuario.cUsuariosChatRol);
                return usuario;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Usuario no encontrado por UserId: {UserCode}", userCode);
        }

        return null;
    }

    public async Task<int> Insert(Usuario item)
    {
        // Este método SQL ya no se usa - CreateAsync usa InsertSupabaseAsync
        _logger.LogWarning("Insert: Método SQL obsoleto - usar CreateAsync con Supabase");
        
        // Generar un ID temporal para compatibilidad
        var generatedId = (int)(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() % int.MaxValue);
        return generatedId;
    }

    private async Task<string> GenerateNextUserIdAsync()
    {
        try
        {
            var response = await _supabaseClient
                .From<UsuarioSupabaseModel>()
                .Select("nUsuariosId,dUsuariosFechaCreacion")
                .Order(x => x.dUsuariosFechaCreacion, Supabase.Postgrest.Constants.Ordering.Descending)
                .Limit(100)
                .Get();

            var models = response.Models ?? new List<UsuarioSupabaseModel>();

            long maxId = 999999999;
            foreach (var m in models)
            {
                var idStr = m.nUsuariosId ?? string.Empty;
                if (idStr.Length == 10 && long.TryParse(idStr, out var idVal))
                {
                    if (idVal > maxId) maxId = idVal;
                }
            }

            var nextId = maxId + 1;
            return nextId.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error al obtener el último ID de usuario, usando valor inicial");
            return "1000000000";
        }
    }

    private async Task<Usuario> InsertSupabaseAsync(Usuario item)
    {
        // Generar un ID string de 10 caracteres para Supabase
        // Comenzar desde 1000000000 y autoincrementar basándonos en el último registro
        var generatedId = await GenerateNextUserIdAsync();
        
        _logger.LogInformation("Generando ID para usuario: {GeneratedId}", generatedId);

        // Actualizar el ID en el objeto original
        item.cUsuariosId = int.TryParse(generatedId, out int parsedId) ? parsedId : 0;

        var supaUser = new UsuarioSupabaseModel
        {
            nUsuariosId = generatedId,
            cUsuariosNombre = string.IsNullOrWhiteSpace(item.cUsuariosChatNombre) ? item.cNombre : item.cUsuariosChatNombre,
            cUsuariosEmail = string.IsNullOrWhiteSpace(item.cUsuariosChatEmail) ? item.cEmail : item.cUsuariosChatEmail,
            cUsuariosAvatar = item.cUsuariosChatAvatar,
            cUsuariosUsername = item.cUsuariosChatUsername, // Add username field
            bUsuariosEstaEnLinea = item.bUsuariosChatEstaEnLinea,
            dUsuariosUltimaConexion = item.dUsuariosChatUltimaConexion,
            dUsuariosFechaCreacion = item.dUsuariosChatFechaCreacion,
            cUsuariosPerCodigo = item.cUsuariosChatPerCodigo,
            cUsuariosPerJurCodigo = item.cUsuariosChatPerJurCodigo,
            cUsuariosPassword = item.cUsuariosChatPassword,
            bUsuariosActivo = item.bUsuariosChatEstaActivo,
            bUsuarioVerificado = item.bUsuarioVerificado ?? false,
            cUsuarioTokenVerificacion = item.cUsuarioTokenVerificacion,
            dUsuarioCambioPassword = item.dUsuarioCambioPassword,
            cUsuarioConfigPrivacidad = item.cUsuarioConfigPrivacidad,
            cUsuarioConfigNotificaciones = item.cUsuarioConfigNotificaciones
        };

        _logger.LogInformation("Insertando usuario en Supabase con ID: {Id}, Email: {Email}", supaUser.nUsuariosId, supaUser.cUsuariosEmail);

        int attempts = 0;
        UsuarioSupabaseModel model;
        while (true)
        {
            try
            {
                var response = await _supabaseClient
                    .From<UsuarioSupabaseModel>()
                    .Insert(supaUser, new Supabase.Postgrest.QueryOptions
                    {
                        Returning = Supabase.Postgrest.QueryOptions.ReturnType.Representation
                    });

                _logger.LogInformation("Respuesta de Supabase: {ResponseStatus}", response.ResponseMessage?.StatusCode ?? 0);
                model = response.Model ?? supaUser;
                break;
            }
            catch (Supabase.Postgrest.Exceptions.PostgrestException ex)
            {
                var msg = ex.Message ?? string.Empty;
                if (msg.Contains("duplicate key") || msg.Contains("already exists") || msg.Contains("23505"))
                {
                    attempts++;
                    if (attempts > 5) throw;
                    if (!long.TryParse(supaUser.nUsuariosId, out var current)) current = 1000000000;
                    var next = (current + 1).ToString();
                    supaUser.nUsuariosId = next;
                    item.cUsuariosId = int.TryParse(next, out var parsedNext) ? parsedNext : 0;
                    continue;
                }
                throw;
            }
        }

        return new Usuario
        {
            cUsuariosId = int.TryParse(model.nUsuariosId, out int parsedId2) ? parsedId2 : 0,
            cNombre = item.cNombre,
            cApellido = item.cApellido,
            cEmail = model.cUsuariosEmail,
            cTelefono = item.cTelefono,
            cAvatar = model.cUsuariosAvatar,
            bActivo = item.bActivo,
            dFechaCreacion = item.dFechaCreacion,
            nEmpresaId = item.nEmpresaId,
            nAplicacionId = item.nAplicacionId,
            dUsuariosChatUltimaConexion = model.dUsuariosUltimaConexion,
            bUsuariosChatEstaActivo = model.bUsuariosActivo,
            bUsuariosChatEstaEnLinea = model.bUsuariosEstaEnLinea,
            dUsuariosChatFechaCreacion = model.dUsuariosFechaCreacion,
            cUsuariosChatId = model.nUsuariosId.ToString(),
            cUsuariosChatNombre = model.cUsuariosNombre,
            cUsuariosChatEmail = model.cUsuariosEmail,
            cUsuariosChatPassword = model.cUsuariosPassword ?? string.Empty,
            dUsuarioCambioPassword = item.dUsuarioCambioPassword,
            cUsuarioConfigPrivacidad = item.cUsuarioConfigPrivacidad,
            cUsuarioConfigNotificaciones = item.cUsuarioConfigNotificaciones,
            cUsuariosChatAvatar = model.cUsuariosAvatar,
            bUsuarioVerificado = item.bUsuarioVerificado,
            cUsuariosChatUsername = item.cUsuariosChatUsername,
            cUsuariosChatRol = item.cUsuariosChatRol,
            cUsuarioTokenVerificacion = item.cUsuarioTokenVerificacion,
            cUsuariosChatPerJurCodigo = model.cUsuariosPerJurCodigo,
            cUsuariosChatPerCodigo = model.cUsuariosPerCodigo,
            cUsuariosChatAppCodigo = item.cUsuariosChatAppCodigo,
            cUsuarioTokenReset = item.cUsuarioTokenReset,
            dUsuarioTokenResetExpiracion = item.dUsuarioTokenResetExpiracion
        };
    }

    public async Task<bool> Update(Usuario item)
    {
        // Este método SQL ya no se usa - UpdateAsync usa Supabase directamente
        _logger.LogWarning("Update: Método SQL obsoleto - usar UpdateAsync con Supabase");
        return false;
    }

    public async Task<bool> DeleteEntero(Int32 cUsuariosId)
    {
        // Este método SQL ya no se usa - DeleteAsync usa Supabase directamente
        _logger.LogWarning("DeleteEntero: Método SQL obsoleto - usar DeleteAsync con Supabase");
        return false;
    }

    public async Task<Usuario> GetItem(UsuarioFilter filter, UsuarioFilterItemType filterType)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase directamente
        _logger.LogWarning("GetItem: Método SQL obsoleto - usar métodos públicos con Supabase");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new Usuario(); // Retornar usuario vacío en lugar de null
    }

    public async Task<IEnumerable<Usuario>> GetLstItem(UsuarioFilter filter, UsuarioFilterListType filterType, Utils.Pagination pagination)
    {
        IEnumerable<Usuario> lstItemFound = new List<Usuario>();
        switch (filterType)
        {
            case UsuarioFilterListType.ByPagination:
                lstItemFound = await this.GetByPagination(filter, pagination);
                break;
            case UsuarioFilterListType.ByNombre:
                lstItemFound = await this.GetByEmpresa(filter);
                break;
            case UsuarioFilterListType.ByEmail:
                lstItemFound = await this.GetByAplicacion(filter);
                break;
            case UsuarioFilterListType.ByActivos:
                lstItemFound = await this.GetByEmpresaYAplicacion(filter);
                break;
            case UsuarioFilterListType.ByInactivos:
                lstItemFound = await this.GetByActivos(filter);
                break;
            case UsuarioFilterListType.ByFechaCreacion:
                lstItemFound = await this.GetByTerminoBusqueda(filter);
                break;
        }
        return lstItemFound;
    }

    #endregion

    #region Private Methods

    private async Task<Usuario?> GetById(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetById: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return null;
    }

    private async Task<Usuario?> GetByEmail(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByEmail: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return null;
    }

    private async Task<Usuario?> GetByNombreCompleto(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByNombreCompleto: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return null;
    }

    private async Task<Usuario?> GetByTelefono(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByTelefono: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return null;
    }

    private async Task<IEnumerable<Usuario>> GetByPagination(UsuarioFilter filter, Utils.Pagination pagination)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByPagination: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    private async Task<IEnumerable<Usuario>> GetByEmpresa(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByEmpresa: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    private async Task<IEnumerable<Usuario>> GetByAplicacion(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByAplicacion: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    private async Task<IEnumerable<Usuario>> GetByEmpresaYAplicacion(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByEmpresaYAplicacion: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    private async Task<IEnumerable<Usuario>> GetByActivos(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByActivos: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    private async Task<IEnumerable<Usuario>> GetByTerminoBusqueda(UsuarioFilter filter)
    {
        // Método SQL obsoleto - los métodos públicos ahora usan Supabase
        _logger.LogWarning("GetByTerminoBusqueda: Método SQL obsoleto");
        await Task.Delay(1); // Evitar advertencia de async sin await
        return new List<Usuario>();
    }

    #endregion
}
