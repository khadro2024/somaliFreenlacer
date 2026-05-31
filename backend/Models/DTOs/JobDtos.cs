using System.ComponentModel.DataAnnotations;
using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.DTOs;

public class CreateJobDto
{
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Range(1, double.MaxValue)]
    public decimal Budget { get; set; }

    public int? CategoryId { get; set; }
}

public class UpdateJobDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? Budget { get; set; }
    public int? CategoryId { get; set; }
    public JobStatus? Status { get; set; }
}

public class JobResponseDto
{
    public int JobId { get; set; }
    public int ClientId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Budget { get; set; }
    public string? CategoryName { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public int ApplicationCount { get; set; }
    public string? CoverImageUrl { get; set; }
    public string? ClientProfileImageUrl { get; set; }
}
