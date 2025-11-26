using ChatModularMicroservice.Entities.DTOs;
using System;

namespace ChatModularMicroservice.Domain
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> ObtenerTodosAsync();
        Task<UsuarioDto?> ObtenerPorIdAsync(Guid id);
        Task<UsuarioDto?> ObtenerPorEmailAsync(string email);
        Task<UsuarioDto?> ObtenerPorUsernameAsync(string username);
        Task<UsuarioDto?> ObtenerPorCodigoUsuarioAsync(string codigo);
        Task<IEnumerable<UsuarioDto>> ObtenerPorAplicacionAsync(string appCodigo);
        Task<IEnumerable<UsuarioDto>> ObtenerActivosAsync(int pagina, int tamanoPagina);
        Task<IEnumerable<UsuarioDto>> BuscarUsuariosAsync(BuscarUsuarioDto buscarDto);
        Task<UsuarioDto> CrearAsync(CreateUsuarioDto createDto);
        Task<UsuarioDto?> ActualizarAsync(Guid id, UpdateUsuarioDto updateDto);
        Task<bool> EliminarAsync(Guid id);
        Task<bool> ActualizarUltimaConexionAsync(Guid id);
        Task<bool> ActualizarEstadoConexionAsync(Guid id, UpdateEstadoConexionDto estadoDto);
        Task<bool> EstablecerEstadoActivoAsync(Guid id, bool estaActivo);
        Task<bool> CambiarPasswordAsync(Guid id, CambiarPasswordDto cambiarPasswordDto);
        Task<bool> ExistePorEmailAsync(string email);
        Task<bool> ExistePorUsernameAsync(string username);
        Task<bool> ExistePorCodigoUsuarioAsync(string codigo);
        Task<UsuarioEstadisticasDto?> ObtenerEstadisticasAsync(Guid id);
        Task<int> ObtenerTotalUsuariosAsync();
        Task<int> ObtenerTotalUsuariosActivosAsync();
        Task<int> ObtenerTotalUsuariosEnLineaAsync();
    }
}