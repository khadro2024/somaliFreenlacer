namespace SomaliFreelanceMarketplace.Models.DTOs;

public class WorkImageDto
{
    public int WorkImageId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Caption { get; set; } = string.Empty;
    public string ImageType { get; set; } = string.Empty;
    public int? JobId { get; set; }
    public int? ProjectId { get; set; }
    public string? JobTitle { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class UploadImageResponseDto
{
    public string ImageUrl { get; set; } = string.Empty;
    public int? WorkImageId { get; set; }
    public string Message { get; set; } = string.Empty;
}
