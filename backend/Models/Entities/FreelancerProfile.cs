namespace SomaliFreelanceMarketplace.Models.Entities;

public class FreelancerProfile
{
    public int ProfileId { get; set; }
    public int UserId { get; set; }
    public string Skills { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public decimal HourlyRate { get; set; }
    public string Portfolio { get; set; } = string.Empty;
    public double Rating { get; set; }
    public int RatingCount { get; set; }

    public User User { get; set; } = null!;
}
