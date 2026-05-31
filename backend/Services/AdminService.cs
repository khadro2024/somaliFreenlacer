using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class AdminService : IAdminService
{
    private readonly ApplicationDbContext _db;
    private readonly IJobService _jobService;

    public AdminService(ApplicationDbContext db, IJobService jobService)
    {
        _db = db;
        _jobService = jobService;
    }

    public async Task<DashboardStatsDto> GetStatsAsync() => new()
    {
        TotalUsers = await _db.Users.CountAsync(),
        TotalJobs = await _db.Jobs.CountAsync(),
        ActiveProjects = await _db.Projects.CountAsync(p => p.Status == ProjectStatus.Working),
        PendingPayments = await _db.Payments.CountAsync(p => p.Status == PaymentStatus.Held)
    };

    public async Task<IEnumerable<UserAdminDto>> GetUsersAsync()
    {
        var users = await _db.Users.OrderByDescending(u => u.CreatedAt).ToListAsync();
        return users.Select(u => new UserAdminDto
        {
            UserId = u.UserId,
            FullName = u.FullName,
            Email = u.Email,
            Role = u.Role.ToString(),
            IsVerified = u.IsVerified,
            IsSuspended = u.IsSuspended,
            CreatedAt = u.CreatedAt
        });
    }

    public async Task<bool> SuspendUserAsync(int userId, bool suspend)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null || user.Role == UserRole.Admin) return false;
        user.IsSuspended = suspend;
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> VerifyUserAsync(int userId)
    {
        var user = await _db.Users.FindAsync(userId);
        if (user == null) return false;
        user.IsVerified = true;
        await _db.SaveChangesAsync();
        return true;
    }

    public Task<IEnumerable<JobResponseDto>> GetAllJobsAsync() => _jobService.GetAllAsync();
}
