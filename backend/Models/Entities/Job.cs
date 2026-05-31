using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class Job
{
    public int JobId { get; set; }
    public int ClientId { get; set; }
    public int? CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public JobStatus Status { get; set; } = JobStatus.Open;
    public string? CoverImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Client { get; set; } = null!;
    public ICollection<WorkImage> WorkImages { get; set; } = new List<WorkImage>();
    public Category? Category { get; set; }
    public ICollection<Application> Applications { get; set; } = new List<Application>();
    public Project? Project { get; set; }
}
