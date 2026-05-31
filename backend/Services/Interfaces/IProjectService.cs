using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto?> GetByIdAsync(int projectId, int userId);
    Task<IEnumerable<ProjectResponseDto>> GetByClientAsync(int clientId);
    Task<IEnumerable<ProjectResponseDto>> GetByFreelancerAsync(int freelancerId);
    Task<ProjectResponseDto?> MarkCompleteAsync(int projectId, int userId, bool isClient);
}
