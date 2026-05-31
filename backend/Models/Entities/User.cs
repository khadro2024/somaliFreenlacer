using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class User
{
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; }
    public bool IsVerified { get; set; }
    public bool IsSuspended { get; set; }
    public string? ProfileImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public FreelancerProfile? FreelancerProfile { get; set; }
    public ICollection<WorkImage> WorkImages { get; set; } = new List<WorkImage>();
    public ICollection<Job> PostedJobs { get; set; } = new List<Job>();
    public ICollection<Application> Applications { get; set; } = new List<Application>();
    public ICollection<Project> ClientProjects { get; set; } = new List<Project>();
    public ICollection<Project> FreelancerProjects { get; set; } = new List<Project>();
    public ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
    public ICollection<Rating> RatingsGiven { get; set; } = new List<Rating>();
    public ICollection<Rating> RatingsReceived { get; set; } = new List<Rating>();
}
