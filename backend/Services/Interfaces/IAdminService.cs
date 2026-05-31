using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IAdminService
{
    Task<DashboardStatsDto> GetStatsAsync();
    Task<IEnumerable<UserAdminDto>> GetUsersAsync();
    Task<bool> SuspendUserAsync(int userId, bool suspend);
    Task<bool> VerifyUserAsync(int userId);
    Task<IEnumerable<JobResponseDto>> GetAllJobsAsync();
}
