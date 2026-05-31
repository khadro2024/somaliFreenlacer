using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class WorkImage
{
    public int WorkImageId { get; set; }
    public int UserId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string PublicId { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public ImageType ImageType { get; set; }
    public int? JobId { get; set; }
    public int? ProjectId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Job? Job { get; set; }
    public Project? Project { get; set; }
}
