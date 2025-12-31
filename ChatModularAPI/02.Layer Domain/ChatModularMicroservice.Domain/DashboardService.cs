using ChatModularMicroservice.Entities.DTOs;
using ChatModularMicroservice.Repository;

namespace ChatModularMicroservice.Domain
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _repo;

        public DashboardService(IDashboardRepository repo)
        {
            _repo = repo;
        }

        public async Task<DashboardStatsDto> ObtenerEstadisticasAsync()
        {
            var stats = await _repo.ObtenerEstadisticasAsync();
            return stats ?? new DashboardStatsDto();
        }
    }
}
