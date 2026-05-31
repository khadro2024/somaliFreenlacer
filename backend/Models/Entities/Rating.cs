namespace SomaliFreelanceMarketplace.Models.Entities;

public class Rating
{
    public int RatingId { get; set; }
    public int ProjectId { get; set; }
    public int RaterId { get; set; }
    public int RatedUserId { get; set; }
    public int Stars { get; set; }
    public string ReviewText { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Project Project { get; set; } = null!;
    public User Rater { get; set; } = null!;
    public User RatedUser { get; set; } = null!;
}
