using ChatModularMicroservice.Entities.Models;
using ChatModularMicroservice.Entities;
using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Infrastructure.Repositories;
using ChatModularMicroservice.Shared.Configs;
using Microsoft.Extensions.Logging;
using Supabase;
using Supabase.Postgrest;
using static Supabase.Postgrest.Constants;
using Utils = ChatModularMicroservice.Shared.Utils;

namespace ChatModularMicroservice.Infrastructure
{
    public class ApplicationSupabaseRepository : SupabaseRepository, IApplicationRepository
    {
        public ApplicationSupabaseRepository(Supabase.Client supabaseClient, ILogger<ApplicationSupabaseRepository> logger, SupabaseConfig config)
            : base(supabaseClient, logger, config) { }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            try
            {
                var result = await ExecuteStoredProcedureListAsync<Application>("usp_applications_select", null, "ChatModularMicroservice");
                if (result.isSuccess && result.lstItem != null)
                {
                    return result.lstItem.Cast<Application>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al ejecutar usp_application_getall, usando consulta directa");
            }

            // Fallback: usar consulta directa si el procedimiento almacenado falla
            var q = _supabaseClient
                .From<Application>()
                .Where(a => a.bAplicacionesEsActiva == true)
                .Order("cAplicacionesNombre", Ordering.Ascending);
            var res = await q.Get();
            return res.Models ?? new List<Application>();
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            try
            {
                var parameters = new { nAplicacionesId = id };
                var result = await ExecuteStoredProcedureListAsync<Application>("usp_application_getbyid", parameters, "ChatModularMicroservice");
                if (result.isSuccess && result.lstItem != null)
                {
                    return result.lstItem.Cast<Application>().FirstOrDefault();
                }
            }
            catch { }

            var res = await _supabaseClient
                .From<Application>()
                .Filter("nAplicacionesId", Constants.Operator.Equals, id)
                .Get();
            return res.Models?.FirstOrDefault();
        }

        public async Task<Application?> GetApplicationByCodeAsync(string code)
        {
            try
            {
                var parameters = new { cAplicacionesCodigo = code };
                var result = await ExecuteStoredProcedureListAsync<Application>("USP_Application_GetByCode", parameters, "ChatModularMicroservice");
                if (result.isSuccess && result.lstItem != null)
                {
                    return result.lstItem.Cast<Application>().FirstOrDefault();
                }
            }
            catch { }

            var res = await _supabaseClient
                .From<Application>()
                .Filter("cAplicacionesCodigo", Constants.Operator.Equals, code)
                .Get();
            return res.Models?.FirstOrDefault();
        }

        public async Task<Application> CreateApplicationAsync(Application application)
        {
            var parameters = new
            {
                p_nombre = application.cAplicacionesNombre,
                p_descripcion = application.cAplicacionesDescripcion ?? "",
                p_codigo = application.cAplicacionesCodigo,
                p_url_logo = application.cAplicacionesUrlLogo,
                p_version = application.cAplicacionesVersion
            };

            var result = await ExecuteStoredProcedureListAsync<CreateApplicationResult>(
                "sp_aplicaciones_crear",
                parameters,
                "ChatModularMicroservice");

            if (!(result.isSuccess && result.lstItem != null && result.lstItem.Any()))
            {
                var err = result.lstError?.Any() == true ? string.Join(", ", result.lstError) : string.Empty;
                var legacyParams = new
                {
                    cAplicacionesNombre = application.cAplicacionesNombre,
                    cAplicacionesCodigo = application.cAplicacionesCodigo,
                    cAplicacionesDescripcion = application.cAplicacionesDescripcion ?? ""
                };

                var legacyResult = await ExecuteStoredProcedureListAsync<CreateApplicationResult>(
                    "sp_aplicaciones_crear",
                    legacyParams,
                    "ChatModularMicroservice");

                if (legacyResult.isSuccess && legacyResult.lstItem != null && legacyResult.lstItem.Any())
                {
                    result = legacyResult;
                }
                else
                {
                    var pcapParams = new
                    {
                        p_caplicacionesnombre = application.cAplicacionesNombre,
                        p_caplicacionescodigo = application.cAplicacionesCodigo,
                        p_caplicacionesdescripcion = application.cAplicacionesDescripcion ?? ""
                    };

                    var pcapResult = await ExecuteStoredProcedureListAsync<CreateApplicationResult>(
                        "sp_aplicaciones_crear",
                        pcapParams,
                        "ChatModularMicroservice");

                    if (pcapResult.isSuccess && pcapResult.lstItem != null && pcapResult.lstItem.Any())
                    {
                        result = pcapResult;
                    }
                    else
                    {
                        var errorMessage = pcapResult.lstError?.Any() == true ? string.Join(", ", pcapResult.lstError) : (legacyResult.lstError?.Any() == true ? string.Join(", ", legacyResult.lstError) : err);
                        throw new Exception($"Error al crear aplicación: {errorMessage}");
                    }
                }
            }

            var createdResult = result.lstItem.Cast<CreateApplicationResult>().First();
            var createdApp = new Application
            {
                nAplicacionesId = createdResult.naplicacionesid,
                cAplicacionesNombre = createdResult.caplicacionesnombre,
                cAplicacionesCodigo = createdResult.caplicacionescodigo,
                cAplicacionesDescripcion = application.cAplicacionesDescripcion,
                bAplicacionesEsActiva = true,
                dAplicacionesFechaCreacion = createdResult.daplicacionesfechacreacion
            };

            _logger.LogInformation(
                "Aplicación creada exitosamente con ID: {Id}, Código: {Codigo}, Token: {Token}",
                createdApp.nAplicacionesId,
                createdApp.cAplicacionesCodigo,
                createdResult.cappregistrostokenacceso);

            return createdApp;
        }

        public Task<Application> UpdateApplicationAsync(Application application)
        {
            throw new NotSupportedException();
        }

        public Task<bool> DeleteApplicationAsync(int id)
        {
            throw new NotSupportedException();
        }

        public async Task<bool> ApplicationExistsAsync(int id)
        {
            var a = await GetApplicationByIdAsync(id);
            return a != null;
        }

        public async Task<bool> ApplicationExistsByNameAsync(string name, int? excludeId = null)
        {
            var parameters = new { cAplicacionesNombre = name, nExcludeId = excludeId };
            var result = await ExecuteStoredProcedureListAsync<int>("USP_Application_ExistsByName", parameters, "ChatModularMicroservice");
            var value = result.isSuccess && result.lstItem != null ? result.lstItem.Cast<int>().FirstOrDefault() : 0;
            return value > 0;
        }

        public Task<ConfiguracionAplicacion?> GetApplicationConfigurationAsync(int applicationId)
        {
            throw new NotSupportedException();
        }

        public Task<ConfiguracionAplicacion> CreateApplicationConfigurationAsync(ConfiguracionAplicacion configuration)
        {
            throw new NotSupportedException();
        }

        public Task<ConfiguracionAplicacion> UpdateApplicationConfigurationAsync(ConfiguracionAplicacion configuration)
        {
            throw new NotSupportedException();
        }

        public Task<bool> DeleteApplicationConfigurationAsync(int applicationId)
        {
            throw new NotSupportedException();
        }

        public Task<int> Insert(Application item)
        {
            throw new NotSupportedException();
        }

        public Task<bool> Update(Application item)
        {
            throw new NotSupportedException();
        }

        public Task<bool> DeleteEntero(int id)
        {
            throw new NotSupportedException();
        }

        public Task<Application> GetItem(ApplicationFilter filter, ApplicationFilterItemType filterType)
        {
            throw new NotSupportedException();
        }

        public Task<IEnumerable<Application>> GetLstItem(ApplicationFilter filter, ApplicationFilterListType filterType, Utils.Pagination pagination)
        {
            throw new NotSupportedException();
        }
    }
}
