namespace SomaliFreelanceMarketplace.Models.DTOs;

public class ProjectResponseDto
{
    public int ProjectId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public int FreelancerId { get; set; }
    public string FreelancerName { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public decimal? PaymentAmount { get; set; }
    public string? PaymentStatus { get; set; }
    public List<WorkImageDto> WorkImages { get; set; } = new();
}
