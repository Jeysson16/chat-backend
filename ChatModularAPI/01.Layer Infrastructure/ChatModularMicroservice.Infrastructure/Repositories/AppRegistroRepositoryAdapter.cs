using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Repository;
using Microsoft.Extensions.Logging;
using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Infrastructure.Repositories;

/// <summary>
/// Adaptador que implementa la interfaz de Domain usando la implementación de Infrastructure
/// </summary>
public class AppRegistroRepositoryAdapter : IAppRegistroRepository
{
    private readonly AppRegistroRepository _appRegistroRepository;
    private readonly ILogger<AppRegistroRepositoryAdapter> _logger;

    public AppRegistroRepositoryAdapter(AppRegistroRepository appRegistroRepository, ILogger<AppRegistroRepositoryAdapter> logger)
    {
        _appRegistroRepository = appRegistroRepository;
        _logger = logger;
    }

    // Implementaciones requeridas por IAppRegistroRepository
    public async Task<ChatModularMicroservice.Entities.Models.AppRegistro?> GetByCodeAsync(string appCode)
    {
        var domainItem = await _appRegistroRepository.GetByCodeAsync(appCode);
        return domainItem == null ? null : MapToEntities(domainItem);
    }

    public async Task<ChatModularMicroservice.Entities.Models.AppRegistro?> GetByApplicationIdAsync(int applicationId)
    {
        var domainItem = await _appRegistroRepository.GetByApplicationIdAsync(applicationId);
        return domainItem == null ? null : MapToEntities(domainItem);
    }

    public async Task<ChatModularMicroservice.Entities.Models.AppRegistro> GetItem(AppRegistroFilter filter, AppRegistroFilterItemType filterType)
    {
        switch (filterType)
        {
            case AppRegistroFilterItemType.ByToken:
                if (string.IsNullOrWhiteSpace(filter.cAppRegistroToken))
                {
                    throw new ArgumentException("Se requiere cAppRegistroToken para ByToken");
                }
                var domainByToken = await _appRegistroRepository.GetByAccessTokenAsync(filter.cAppRegistroToken);
                if (domainByToken == null)
                {
                    throw new KeyNotFoundException("No se encontró AppRegistro por token");
                }
                return MapToEntities(domainByToken);
            case AppRegistroFilterItemType.ById:
            case AppRegistroFilterItemType.ByAplicacionId:
            case AppRegistroFilterItemType.ByFechaCreacion:
            case AppRegistroFilterItemType.ByEstadoActivo:
            default:
                _logger.LogWarning("GetItem con filterType {FilterType} no está soportado actualmente", filterType);
                throw new NotSupportedException("GetItem no soportado para el tipo de filtro especificado");
        }
    }

    public Task<IEnumerable<ChatModularMicroservice.Entities.Models.AppRegistro>> GetLstItem(AppRegistroFilter filter, AppRegistroFilterListType filterType, Utils.Pagination pagination)
    {
        // La implementación actual no soporta listados; devolvemos colección vacía por compatibilidad
        _logger.LogWarning("GetLstItem no está soportado actualmente. Devuelve lista vacía.");
        return Task.FromResult<IEnumerable<ChatModularMicroservice.Entities.Models.AppRegistro>>(Enumerable.Empty<ChatModularMicroservice.Entities.Models.AppRegistro>());
    }

    public async Task<int> Insert(ChatModularMicroservice.Entities.Models.AppRegistro item)
    {
        var domainItem = MapToDomain(item);
        var created = await _appRegistroRepository.CreateAsync(domainItem);
        return created.Id;
    }

    public async Task<bool> Update(ChatModularMicroservice.Entities.Models.AppRegistro item)
    {
        var domainItem = MapToDomain(item);
        var updated = await _appRegistroRepository.UpdateAsync(domainItem);
        return updated != null;
    }

    public Task<bool> DeleteEntero(int id)
    {
        // La implementación de Infrastructure elimina por código de aplicación, no por ID
        _logger.LogWarning("DeleteEntero no está soportado; la eliminación requiere el código de la aplicación");
        return Task.FromResult(false);
    }

    public Task<bool> UserHasAccessToAppAsync(Guid userId, string appCode)
    {
        // No hay verificación de usuario en la implementación actual; se deja como stub
        _logger.LogWarning("UserHasAccessToAppAsync no está implementado en AppRegistroRepository de Infrastructure");
        return Task.FromResult(true);
    }

    // Métodos auxiliares de mapeo entre Domain y Entities
    private static ChatModularMicroservice.Entities.Models.AppRegistro MapToEntities(ChatModularMicroservice.Domain.AppRegistro src)
    {
        return new ChatModularMicroservice.Entities.Models.AppRegistro
        {
            nAppRegistroId = src.nAppRegistroId,
            cAppRegistroCodigoApp = src.cAppRegistroCodigoApp,
            cAppRegistroNombreApp = src.cAppRegistroNombreApp,
            cAppRegistroTokenAcceso = src.cAppRegistroTokenAcceso,
            cAppRegistroSecretoApp = src.cAppRegistroSecretoApp,
            bAppRegistroEsActivo = src.bAppRegistroEsActivo,
            dAppRegistroFechaCreacion = src.dAppRegistroFechaCreacion,
            dAppRegistroFechaExpiracion = src.dAppRegistroFechaExpiracion,
            cAppRegistroConfiguracionesAdicionales = src.cAppRegistroConfiguracionesAdicionales,
            Id = src.Id,
            cAppCodigo = src.cAppCodigo,
            cAccessToken = src.cAccessToken
        };
    }

    private static ChatModularMicroservice.Domain.AppRegistro MapToDomain(ChatModularMicroservice.Entities.Models.AppRegistro src)
    {
        return new ChatModularMicroservice.Domain.AppRegistro
        {
            nAppRegistroId = src.nAppRegistroId,
            cAppRegistroCodigoApp = src.cAppRegistroCodigoApp,
            cAppRegistroNombreApp = src.cAppRegistroNombreApp,
            cAppRegistroTokenAcceso = src.cAppRegistroTokenAcceso,
            cAppRegistroSecretoApp = src.cAppRegistroSecretoApp,
            bAppRegistroEsActivo = src.bAppRegistroEsActivo,
            dAppRegistroFechaCreacion = src.dAppRegistroFechaCreacion,
            dAppRegistroFechaExpiracion = src.dAppRegistroFechaExpiracion,
            cAppRegistroConfiguracionesAdicionales = src.cAppRegistroConfiguracionesAdicionales,
            Id = src.Id,
            cAppCodigo = src.cAppCodigo,
            cAccessToken = src.cAccessToken
        };
    }
}