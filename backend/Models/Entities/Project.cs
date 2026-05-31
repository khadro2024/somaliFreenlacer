using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class Project
{
    public int ProjectId { get; set; }
    public int JobId { get; set; }
    public int FreelancerId { get; set; }
    public int ClientId { get; set; }
    public ProjectStatus Status { get; set; } = ProjectStatus.Working;
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? CompletedAt { get; set; }

    public Job Job { get; set; } = null!;
    public User Freelancer { get; set; } = null!;
    public User Client { get; set; } = null!;
    public Payment? Payment { get; set; }
    public ICollection<Rating> Ratings { get; set; } = new List<Rating>();
    public ICollection<WorkImage> WorkImages { get; set; } = new List<WorkImage>();
}
