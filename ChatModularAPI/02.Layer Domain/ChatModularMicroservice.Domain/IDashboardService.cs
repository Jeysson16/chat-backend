using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Domain
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> ObtenerEstadisticasAsync();
    }
}
