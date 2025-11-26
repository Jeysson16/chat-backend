using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Domain
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioService> _logger;

        public UsuarioService(IUsuarioRepository usuarioRepository, ILogger<UsuarioService> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto?> ObtenerPorIdAsync(Guid id)
        {
            var u = await _usuarioRepository.GetByIdAsync(id);
            return u == null ? null : MapToDto(u);
        }

        public async Task<UsuarioDto?> ObtenerPorEmailAsync(string email)
        {
            var u = await _usuarioRepository.GetByEmailAsync(email);
            return u == null ? null : MapToDto(u);
        }

        public async Task<UsuarioDto?> ObtenerPorUsernameAsync(string username)
        {
            var u = await _usuarioRepository.GetByUsernameAsync(username);
            return u == null ? null : MapToDto(u);
        }

        public async Task<UsuarioDto?> ObtenerPorCodigoUsuarioAsync(string codigo)
        {
            var u = await _usuarioRepository.GetByUserCodeAsync(codigo);
            return u == null ? null : MapToDto(u);
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerPorAplicacionAsync(string appCodigo)
        {
            // Stub: no repo method; return empty list for now
            _logger.LogWarning("ObtenerPorAplicacionAsync no implementado: appCodigo={App}", appCodigo);
            return Enumerable.Empty<UsuarioDto>();
        }

        public async Task<IEnumerable<UsuarioDto>> ObtenerActivosAsync(int pagina, int tamanoPagina)
        {
            // Stub de paginación: usa GetAllAsync y filtra por activo si existe el campo
            var usuarios = await _usuarioRepository.GetAllAsync();
            var activos = usuarios.Where(x => x.bUsuariosChatEstaActivo);
            return activos.Skip((pagina - 1) * tamanoPagina).Take(tamanoPagina).Select(MapToDto);
        }

        public async Task<IEnumerable<UsuarioDto>> BuscarUsuariosAsync(BuscarUsuarioDto buscarDto)
        {
            var term = buscarDto.cTerminoBusqueda ?? string.Empty;
            var usuarios = await _usuarioRepository.SearchUsersAsync(term);
            return usuarios.Select(MapToDto);
        }

        public async Task<UsuarioDto> CrearAsync(CreateUsuarioDto createDto)
        {
            var entity = new ChatModularMicroservice.Entities.Models.Usuario
            {
                cNombre = createDto.Nombre,
                cEmail = createDto.Email,
                bActivo = true,
            };
            var creado = await _usuarioRepository.CreateAsync(entity);
            return MapToDto(creado);
        }

        public async Task<UsuarioDto?> ActualizarAsync(Guid id, UpdateUsuarioDto updateDto)
        {
            var existente = await _usuarioRepository.GetByIdAsync(id);
            if (existente == null) return null;
            // UpdateUsuarioDto tiene campos no-nullables obligatorios para Nombre y Email
            existente.cNombre = updateDto.Nombre;
            existente.cEmail = updateDto.Email;
            existente.cTelefono = updateDto.Telefono ?? existente.cTelefono;
            existente.cAvatar = updateDto.Avatar ?? existente.cAvatar;
            existente.bActivo = updateDto.EsActivo;
            var actualizado = await _usuarioRepository.UpdateAsync(existente);
            return MapToDto(actualizado);
        }

        public Task<bool> EliminarAsync(Guid id) => _usuarioRepository.DeleteAsync(id);

        public async Task<bool> ActualizarUltimaConexionAsync(Guid id)
        {
            return await _usuarioRepository.UpdateLastConnectionAsync(id.ToString());
        }

        public async Task<bool> ActualizarEstadoConexionAsync(Guid id, UpdateEstadoConexionDto estadoDto)
        {
            // Stub: no repo method para estado conexión; simula usando UpdateAsync
            var u = await _usuarioRepository.GetByIdAsync(id);
            if (u == null) return false;
            u.bUsuariosChatEstaEnLinea = estadoDto.EstaEnLinea;
            u.dUsuariosChatUltimaConexion = estadoDto.FechaConexion ?? DateTime.UtcNow;
            await _usuarioRepository.UpdateAsync(u);
            return true;
        }

        public async Task<bool> EstablecerEstadoActivoAsync(Guid id, bool estaActivo)
        {
            return await _usuarioRepository.SetActiveStatusAsync(id.ToString(), estaActivo);
        }

        public async Task<bool> CambiarPasswordAsync(Guid id, CambiarPasswordDto cambiarPasswordDto)
        {
            // Stub: requiere validación; usamos UpdatePasswordAsync
            if (string.IsNullOrWhiteSpace(cambiarPasswordDto.NuevaPassword)) return false;
            return await _usuarioRepository.UpdatePasswordAsync(id.ToString(), cambiarPasswordDto.NuevaPassword);
        }

        public Task<bool> ExistePorEmailAsync(string email) => _usuarioRepository.ExistsByEmailAsync(email);
        public Task<bool> ExistePorUsernameAsync(string username) => _usuarioRepository.ExistsByUsernameAsync(username);
        public async Task<bool> ExistePorCodigoUsuarioAsync(string codigo)
        {
            var u = await _usuarioRepository.GetByUserCodeAsync(codigo);
            return u != null;
        }

        public async Task<UsuarioEstadisticasDto?> ObtenerEstadisticasAsync(Guid id)
        {
            // Stub: no repo method; devuelve estructura básica
            var u = await _usuarioRepository.GetByIdAsync(id);
            if (u == null) return null;
            return new UsuarioEstadisticasDto
            {
                UsuarioId = id,
                UltimaConexion = u.dUsuariosChatUltimaConexion,
                TotalMensajes = 0,
                TotalContactos = 0,
                ConversacionesActivas = 0,
                ConversacionesArchivadas = 0,
                TokensActivos = 0,
                TokensRevocados = 0
            };
        }

        public async Task<int> ObtenerTotalUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Count();
        }

        public async Task<int> ObtenerTotalUsuariosActivosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Count(u => u.bUsuariosChatEstaActivo);
        }

        public async Task<int> ObtenerTotalUsuariosEnLineaAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Count(u => u.bUsuariosChatEstaEnLinea);
        }

        private static UsuarioDto MapToDto(ChatModularMicroservice.Entities.Models.Usuario u)
        {
            return new UsuarioDto
            {
                nUsuariosId = u.cUsuariosId,
                cUsuariosNombre = u.cNombre,
                cUsuariosEmail = u.cEmail,
                cUsuariosTelefono = u.cTelefono ?? string.Empty,
                bUsuariosActivo = u.bActivo,
                dUsuariosFechaCreacion = u.dFechaCreacion,
                bUsuariosEstaEnLinea = u.bUsuariosChatEstaEnLinea,
                bUsuariosChatEstaActivo = u.bUsuariosChatEstaActivo,
                cUsuariosChatEmail = u.cUsuariosChatEmail,
                cUsuariosChatId = u.cUsuariosChatId,
                cUsuariosChatNombre = u.cUsuariosChatNombre,
                cUsuariosChatUsername = u.cUsuariosChatUsername ?? string.Empty,
                cUsuariosChatAvatar = u.cUsuariosChatAvatar ?? string.Empty,
                dUsuariosUltimaConexion = u.dUsuariosChatUltimaConexion ?? DateTime.MinValue,
                cUsuariosChatRol = u.cUsuariosChatRol ?? string.Empty,
                nUsuariosAplicacionId = u.nAplicacionId,
                nUsuariosEmpresaId = u.nEmpresaId
            };
        }
    }
}