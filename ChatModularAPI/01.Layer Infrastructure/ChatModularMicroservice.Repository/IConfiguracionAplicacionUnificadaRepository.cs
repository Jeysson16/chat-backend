using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de configuración unificada de aplicaciones
/// </summary>
public interface IConfiguracionAplicacionUnificadaRepository : IDeleteIntRepository, IInsertIntRepository<ConfiguracionAplicacionUnificada>, IUpdateRepository<ConfiguracionAplicacionUnificada>
{
    Task<ConfiguracionAplicacionUnificada> GetItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterItemType filterType);
    Task<IEnumerable<ConfiguracionAplicacionUnificada>> GetLstItem(ConfiguracionAplicacionUnificadaFilter filter, ConfiguracionAplicacionUnificadaFilterListType filterType, Utils.Pagination pagination);
    
    // Métodos específicos para la nueva implementación
    Task<ConfiguracionAplicacionDto?> ObtenerPorCodigoAplicacionAsync(string codigoAplicacion);
    Task<ConfiguracionAplicacionUnificada?> ObtenerPorAplicacionIdAsync(int aplicacionId);
    Task<ConfiguracionAplicacionUnificadaDto?> ObtenerUnificadaPorCodigoAplicacionAsync(string codigoAplicacion);
    Task<ConfiguracionAplicacionDto> UpsertAsync(string codigoAplicacion, ConfiguracionAplicacionDto configuracion);
    Task<ConfiguracionAplicacionDto?> ObtenerConfiguracionAdjuntosAsync(string codigoAplicacion);
    Task<List<ConfiguracionAplicacionDto>> ListarConfiguracionesActivasAsync();
    Task<bool> ExistePorCodigoAplicacionAsync(string codigoAplicacion);
    Task<bool> ExistePorAplicacionIdAsync(int aplicacionId);
}