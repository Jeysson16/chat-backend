using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Repository;

/// <summary>
/// Interfaz para el repositorio de configuración de empresas
/// </summary>
public interface IConfiguracionEmpresaRepository : IDeleteIntRepository, IInsertIntRepository<ConfiguracionEmpresa>, IUpdateRepository<ConfiguracionEmpresa>
{
    Task<ConfiguracionEmpresa> GetItem(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterItemType filterType);
    Task<IEnumerable<ConfiguracionEmpresa>> GetLstItem(ConfiguracionEmpresaFilter filter, ConfiguracionEmpresaFilterListType filterType, Utils.Pagination pagination);
    
    /// <summary>
    /// Crea una nueva configuración de empresa de forma asíncrona
    /// </summary>
    /// <param name="configuracionEmpresa">Configuración de empresa a crear</param>
    /// <returns>La configuración de empresa creada</returns>
    Task<ConfiguracionEmpresa> CreateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa);
    
    /// <summary>
    /// Actualiza una configuración de empresa de forma asíncrona
    /// </summary>
    /// <param name="configuracionEmpresa">Configuración de empresa a actualizar</param>
    /// <returns>True si la actualización fue exitosa</returns>
    Task<bool> UpdateConfiguracionEmpresaAsync(ConfiguracionEmpresa configuracionEmpresa);
    
    /// <summary>
    /// Elimina una configuración de empresa de forma asíncrona
    /// </summary>
    /// <param name="configuracionId">ID de la configuración a eliminar</param>
    /// <returns>True si la eliminación fue exitosa</returns>
    Task<bool> DeleteConfiguracionEmpresaAsync(string configuracionId);
    
    /// <summary>
    /// Verifica si existe una configuración de empresa
    /// </summary>
    /// <param name="configuracionId">ID de la configuración a verificar</param>
    /// <returns>True si la configuración existe</returns>
    Task<bool> ConfiguracionEmpresaExistsAsync(string configuracionId);
}