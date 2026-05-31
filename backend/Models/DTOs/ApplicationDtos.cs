using System.ComponentModel.DataAnnotations;

namespace SomaliFreelanceMarketplace.Models.DTOs;

public class CreateApplicationDto
{
    [Required]
    public string Proposal { get; set; } = string.Empty;

    [Range(1, double.MaxValue)]
    public decimal BidAmount { get; set; }
}

public class ApplicationResponseDto
{
    public int ApplicationId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public int FreelancerId { get; set; }
    public string FreelancerName { get; set; } = string.Empty;
    public string Proposal { get; set; } = string.Empty;
    public decimal BidAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public double? FreelancerRating { get; set; }
}
