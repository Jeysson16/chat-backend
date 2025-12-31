using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;
using ChatModularMicroservice.Shared.Configs;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace ChatModularMicroservice.Infrastructure.Repositories
{
    public class DashboardSupabaseRepository : SupabaseRepository, IDashboardRepository
    {
        public DashboardSupabaseRepository(Supabase.Client supabaseClient, ILogger<DashboardSupabaseRepository> logger, SupabaseConfig config)
            : base(supabaseClient, logger, config) { }

        public async Task<DashboardStatsDto?> ObtenerEstadisticasAsync()
        {
            var res = await _supabaseClient.Rpc("usp_dashboard_stats", null);
            var content = res?.Content ?? "{}";
            try
            {
                var dto = JsonSerializer.Deserialize<DashboardStatsDto>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return dto;
            }
            catch
            {
                return null;
            }
        }
    }
}
