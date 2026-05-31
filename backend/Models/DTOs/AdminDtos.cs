namespace SomaliFreelanceMarketplace.Models.DTOs;

public class UserAdminDto
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public bool IsVerified { get; set; }
    public bool IsSuspended { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class DashboardStatsDto
{
    public int TotalUsers { get; set; }
    public int TotalJobs { get; set; }
    public int ActiveProjects { get; set; }
    public int PendingPayments { get; set; }
}
