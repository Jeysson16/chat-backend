using Supabase;
using Supabase.Postgrest;
using Supabase.Postgrest.Models;
using ChatModularMicroservice.Entities.DTOs;
using Utils = ChatModularMicroservice.Shared.Utils;
using ChatModularMicroservice.Shared.Configs;
using Npgsql;
using System.Data;
using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace ChatModularMicroservice.Infrastructure.Repositories
{
    public class SupabaseRepository
    {
        protected readonly Supabase.Client _supabaseClient;
        protected readonly ILogger<SupabaseRepository> _logger;
        protected readonly SupabaseConfig _config;

        public SupabaseRepository(Supabase.Client supabaseClient, ILogger<SupabaseRepository> logger, SupabaseConfig config)
        {
            _supabaseClient = supabaseClient ?? throw new ArgumentNullException(nameof(supabaseClient));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected async Task<Utils.ItemResponseDT> ExecuteStoredProcedureListAsync<T>(
            string procedureName, 
            object? parameters = null,
            string clientName = "ChatModularMicroservice")
        {
            try
            {
                _logger.LogInformation("Ejecutando stored procedure (lista): {ProcedureName}", procedureName);

                var rpc = _supabaseClient.Rpc(procedureName, parameters);
                var result = await rpc;

                if (result?.Content != null)
                {
                    var jsonContent = result.Content;
                    _logger.LogDebug("Resultado del stored procedure: {Content}", jsonContent);

                    var dataList = JsonSerializer.Deserialize<List<T>>(jsonContent, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });

                    return new Utils.ItemResponseDT
                    {
                        clientName = clientName,
                        isSuccess = true,
                        lstItem = dataList?.Cast<object>().ToList() ?? new List<object>(),
                        ticket = Guid.NewGuid().ToString()
                    };
                }

                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { "No se recibió respuesta del stored procedure" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error ejecutando stored procedure {ProcedureName}: {Error}", procedureName, ex.Message);
                
                return new Utils.ItemResponseDT
                {
                    clientName = clientName,
                    isSuccess = false,
                    lstError = new List<string> { $"Error ejecutando {procedureName}: {ex.Message}" },
                    ticket = Guid.NewGuid().ToString()
                };
            }
        }
    }
}
