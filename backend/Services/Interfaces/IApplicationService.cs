using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationResponseDto> ApplyAsync(int jobId, int freelancerId, CreateApplicationDto dto);
    Task<ApplicationResponseDto?> AcceptAsync(int applicationId, int clientId);
    Task<ApplicationResponseDto?> RejectAsync(int applicationId, int clientId);
    Task<IEnumerable<ApplicationResponseDto>> GetByJobAsync(int jobId, int clientId);
    Task<IEnumerable<ApplicationResponseDto>> GetByFreelancerAsync(int freelancerId);
}
