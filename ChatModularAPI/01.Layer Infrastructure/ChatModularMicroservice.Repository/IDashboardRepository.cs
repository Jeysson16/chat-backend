using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Repository
{
    public interface IDashboardRepository
    {
        Task<DashboardStatsDto?> ObtenerEstadisticasAsync();
    }
}
