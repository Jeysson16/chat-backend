using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Repository;
using Supabase;
using Microsoft.Extensions.Logging;
using static Supabase.Postgrest.Constants;

namespace ChatModularMicroservice.Infrastructure.Repositories;

public class AppRegistroRepository
{
    private readonly Supabase.Client _supabaseClient;
    private readonly ILogger<AppRegistroRepository> _logger;

    public AppRegistroRepository(Supabase.Client supabaseClient, ILogger<AppRegistroRepository> logger)
    {
        _supabaseClient = supabaseClient;
        _logger = logger;
    }

    public async Task<AppRegistro?> GetByApplicationIdAsync(int applicationId)
    {
        try
        {
            var result = await _supabaseClient
                .From<AppRegistro>()
                .Filter("nAppRegistrosAplicacionId", Operator.Equals, applicationId)
                .Filter("bAppRegistrosEsActivo", Operator.Equals, "true")
                .Get();
            
            return result.Models?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener AppRegistro por aplicación ID: {ApplicationId}", applicationId);
            return null;
        }
    }

    public async Task<AppRegistro?> GetByCodeAsync(string appCode)
    {
        try
        {
            var result = await _supabaseClient
                .From<AppRegistro>()
                .Filter("cAppRegistrosCodigoApp", Operator.Equals, appCode)
                .Filter("bAppRegistrosEsActivo", Operator.Equals, "true")
                .Get();

            return result.Models?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener AppRegistro por código: {AppCode}", appCode);
            return null;
        }
    }

    public async Task<AppRegistro?> GetByAccessTokenAsync(string accessToken)
    {
        try
        {
            var result = await _supabaseClient
                .From<AppRegistro>()
                .Filter("cAppRegistrosTokenAcceso", Operator.Equals, accessToken)
                .Filter("bAppRegistrosEsActivo", Operator.Equals, "true")
                .Get();

            return result.Models?.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener AppRegistro por token de acceso");
            return null;
        }
    }

    public async Task<bool> ValidateAccessTokenAsync(string appCode, string accessToken)
    {
        try
        {
            var app = await GetByCodeAsync(appCode);
            return app != null && app.cAccessToken == accessToken && app.bAppActivo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating access token for app: {AppCode}", appCode);
            return false;
        }
    }

    public async Task<AppRegistro> CreateAsync(AppRegistro appRegistro)
    {
        try
        {
            appRegistro.CreatedAt = DateTime.UtcNow;
            appRegistro.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseClient
                .From<AppRegistro>()
                .Insert(appRegistro);

            return response.Model!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating app registration: {AppCode}", appRegistro.cAppCodigo);
            throw;
        }
    }

    public async Task<AppRegistro> UpdateAsync(AppRegistro appRegistro)
    {
        try
        {
            appRegistro.UpdatedAt = DateTime.UtcNow;

            var response = await _supabaseClient
                .From<AppRegistro>()
                .Where(x => x.Id == appRegistro.Id)
                .Update(appRegistro);

            return response.Model!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating app registration: {AppCode}", appRegistro.cAppCodigo);
            throw;
        }
    }

    public async Task<bool> DeleteAsync(string appCode)
    {
        try
        {
            await _supabaseClient
                .From<AppRegistro>()
                .Where(x => x.cAppCodigo == appCode)
                .Delete();

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting app registration: {AppCode}", appCode);
            return false;
        }
    }
}
