using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IProfileService
{
    Task<FreelancerProfileResponseDto?> GetProfileAsync(int userId);
    Task<FreelancerProfileResponseDto> UpdateProfileAsync(int userId, UpdateFreelancerProfileDto dto);
    Task<IEnumerable<FreelancerProfileResponseDto>> GetAllFreelancersAsync();
    Task<RatingResponseDto> CreateRatingAsync(int projectId, int raterId, CreateRatingDto dto);
}
