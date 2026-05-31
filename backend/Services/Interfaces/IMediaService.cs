using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IMediaService
{
    bool IsCloudinaryConfigured { get; }
    Task<UploadImageResponseDto> UploadAsync(int userId, IFormFile file, ImageType type, int? jobId, int? projectId, string? caption);
    Task<IEnumerable<WorkImageDto>> GetUserPortfolioAsync(int userId);
    Task<IEnumerable<WorkImageDto>> GetProjectImagesAsync(int projectId);
    Task<bool> DeleteAsync(int workImageId, int userId);
}
