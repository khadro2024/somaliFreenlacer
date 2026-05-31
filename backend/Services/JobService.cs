using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class JobService : IJobService
{
    private readonly ApplicationDbContext _db;

    public JobService(ApplicationDbContext db) => _db = db;

    public async Task<JobResponseDto> CreateAsync(int clientId, CreateJobDto dto)
    {
        var job = new Job
        {
            ClientId = clientId,
            Title = dto.Title,
            Description = dto.Description,
            Budget = dto.Budget,
            CategoryId = dto.CategoryId,
            Status = JobStatus.Open
        };
        _db.Jobs.Add(job);
        await _db.SaveChangesAsync();
        return (await GetByIdAsync(job.JobId))!;
    }

    public async Task<JobResponseDto?> UpdateAsync(int jobId, int clientId, UpdateJobDto dto)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId && j.ClientId == clientId);
        if (job == null) return null;

        if (dto.Title != null) job.Title = dto.Title;
        if (dto.Description != null) job.Description = dto.Description;
        if (dto.Budget.HasValue) job.Budget = dto.Budget.Value;
        if (dto.CategoryId.HasValue) job.CategoryId = dto.CategoryId;
        if (dto.Status.HasValue) job.Status = dto.Status.Value;

        await _db.SaveChangesAsync();
        return await GetByIdAsync(jobId);
    }

    public async Task<bool> DeleteAsync(int jobId, int clientId)
    {
        var job = await _db.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId && j.ClientId == clientId);
        if (job == null || job.Status != JobStatus.Open) return false;
        _db.Jobs.Remove(job);
        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<JobResponseDto?> GetByIdAsync(int jobId)
    {
        var job = await _db.Jobs
            .Include(j => j.Client)
            .Include(j => j.Category)
            .Include(j => j.Applications)
            .FirstOrDefaultAsync(j => j.JobId == jobId);
        return job == null ? null : Map(job);
    }

    public async Task<IEnumerable<JobResponseDto>> GetAllAsync(string? status = null, int? categoryId = null)
    {
        var query = _db.Jobs.Include(j => j.Client).Include(j => j.Category).Include(j => j.Applications).AsQueryable();

        if (!string.IsNullOrEmpty(status) && Enum.TryParse<JobStatus>(status, true, out var jobStatus))
            query = query.Where(j => j.Status == jobStatus);

        if (categoryId.HasValue)
            query = query.Where(j => j.CategoryId == categoryId);

        var jobs = await query.OrderByDescending(j => j.CreatedAt).ToListAsync();
        return jobs.Select(Map);
    }

    public async Task<IEnumerable<JobResponseDto>> GetByClientAsync(int clientId)
    {
        var jobs = await _db.Jobs
            .Include(j => j.Client)
            .Include(j => j.Category)
            .Include(j => j.Applications)
            .Where(j => j.ClientId == clientId)
            .OrderByDescending(j => j.CreatedAt)
            .ToListAsync();
        return jobs.Select(Map);
    }

    private static JobResponseDto Map(Job job) => new()
    {
        JobId = job.JobId,
        ClientId = job.ClientId,
        ClientName = job.Client.FullName,
        Title = job.Title,
        Description = job.Description,
        Budget = job.Budget,
        CategoryName = job.Category?.Name,
        Status = job.Status.ToString(),
        CreatedAt = job.CreatedAt,
        ApplicationCount = job.Applications.Count,
        CoverImageUrl = job.CoverImageUrl,
        ClientProfileImageUrl = job.Client.ProfileImageUrl
    };
}
