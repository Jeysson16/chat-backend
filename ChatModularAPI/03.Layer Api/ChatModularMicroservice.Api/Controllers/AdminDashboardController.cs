using Microsoft.AspNetCore.Mvc;
using ChatModularMicroservice.Domain;
using ChatModularMicroservice.Entities.DTOs;

namespace ChatModularMicroservice.Api.Controllers
{
    [ApiController]
    [Route("api/v1/admin/dashboard")]
    public class AdminDashboardController : BaseController
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardController(IDashboardService dashboardService, ILogger<AdminDashboardController> logger)
            : base(logger)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                var stats = await _dashboardService.ObtenerEstadisticasAsync();
                return Ok(CreateSuccessResponse(stats, GetClientName(), GetUserName()));
            }
            catch (Exception ex)
            {
                return HandleException(ex, GetClientName(), GetUserName());
            }
        }
    }
}
