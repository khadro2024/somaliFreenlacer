using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class Application
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public int FreelancerId { get; set; }
    public string Proposal { get; set; } = string.Empty;
    public decimal BidAmount { get; set; }
    public ApplicationStatus Status { get; set; } = ApplicationStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Job Job { get; set; } = null!;
    public User Freelancer { get; set; } = null!;
}
