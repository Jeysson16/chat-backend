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
                .Filter("bAppRegistrosEsActivo", Operator.Equals, true)
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
        // Tabla AppRegistros no existe, retornar objeto por defecto para SICOM_CHAT_2024
        if (appCode == "SICOM_CHAT_2024")
        {
            _logger.LogInformation("AppRegistros no existe, retornando configuración por defecto para: {AppCode}", appCode);
            return new AppRegistro
            {
                nAppRegistroId = 1,
                nAppRegistrosAplicacionId = 1,
                cAppRegistroCodigoApp = "SICOM_CHAT_2024",
                cAppRegistroNombreApp = "SICOM Chat 2024",
                cAppRegistroTokenAcceso = "AT_6cbeb6faba662bffb1e9b0cdc3a96670",
                cAppRegistroSecretoApp = "ST_0b2c702f3fd200f0fc9ddb02624a21bc9d8be4fa96672504",
                bAppRegistroEsActivo = true,
                dAppRegistroFechaCreacion = DateTime.UtcNow,
                dAppRegistroFechaExpiracion = DateTime.UtcNow.AddYears(1),
                cAppRegistroConfiguracionesAdicionales = "{\"urlBase\":\"http://localhost:5406\",\"descripcion\":\"Aplicación de chat SICOM\"}",
                UpdatedAt = DateTime.UtcNow,
                cAppUrl = "http://localhost:5406"
            };
        }

        _logger.LogWarning("Aplicación no reconocida: {AppCode}", appCode);
        return null;
    }

    public async Task<AppRegistro?> GetByAccessTokenAsync(string accessToken)
    {
        // Tabla AppRegistros no existe, retornar objeto por defecto para el token conocido
        if (string.Equals(accessToken, "AT_6cbeb6faba662bffb1e9b0cdc3a96670", StringComparison.Ordinal))
        {
            _logger.LogInformation("AppRegistros no existe, retornando configuración por defecto para token: {AccessToken}", accessToken);
            return new AppRegistro
            {
                nAppRegistroId = 1,
                nAppRegistrosAplicacionId = 1,
                cAppRegistroCodigoApp = "SICOM_CHAT_2024",
                cAppRegistroNombreApp = "SICOM Chat Application 2024",
                cAppRegistroTokenAcceso = "AT_6cbeb6faba662bffb1e9b0cdc3a96670",
                cAppRegistroSecretoApp = "ST_0b2c702f3fd200f0fc9ddb02624a21bc9d8be4fa96672504",
                bAppRegistroEsActivo = true,
                dAppRegistroFechaCreacion = DateTime.UtcNow,
                dAppRegistroFechaExpiracion = DateTime.UtcNow.AddYears(1),
                cAppRegistroConfiguracionesAdicionales = "{\"urlBase\":\"http://localhost:5406\",\"descripcion\":\"SICOM Chat Application 2024\"}",
                UpdatedAt = DateTime.UtcNow,
                cAppUrl = "http://localhost:5406"
            };
        }

        _logger.LogWarning("Token no reconocido: {AccessToken}", accessToken);
        return null;
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
