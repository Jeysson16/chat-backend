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

            return createdApp;
        }
    }
}
