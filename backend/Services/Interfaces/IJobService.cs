using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IJobService
{
    Task<JobResponseDto> CreateAsync(int clientId, CreateJobDto dto);
    Task<JobResponseDto?> UpdateAsync(int jobId, int clientId, UpdateJobDto dto);
    Task<bool> DeleteAsync(int jobId, int clientId);
    Task<JobResponseDto?> GetByIdAsync(int jobId);
    Task<IEnumerable<JobResponseDto>> GetAllAsync(string? status = null, int? categoryId = null);
    Task<IEnumerable<JobResponseDto>> GetByClientAsync(int clientId);
}
