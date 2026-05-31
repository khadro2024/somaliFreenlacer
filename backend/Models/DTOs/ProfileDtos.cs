using System.ComponentModel.DataAnnotations;

namespace SomaliFreelanceMarketplace.Models.DTOs;

public class UpdateFreelancerProfileDto
{
    public string? Skills { get; set; }
    public string? Bio { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? HourlyRate { get; set; }

    public string? Portfolio { get; set; }
}

public class FreelancerProfileResponseDto
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Skills { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Portfolio { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int RatingCount { get; set; }
    public string? ProfileImageUrl { get; set; }
    public List<WorkImageDto> PortfolioImages { get; set; } = new();
}

public class CreateRatingDto
{
    [Range(1, 5)]
    public int Stars { get; set; }

    [MaxLength(1000)]
    public string ReviewText { get; set; } = string.Empty;
}

public class RatingResponseDto
{
    public int RatingId { get; set; }
    public string RaterName { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
