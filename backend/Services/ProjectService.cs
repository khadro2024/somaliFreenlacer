using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _db;

    public ProjectService(ApplicationDbContext db) => _db = db;

    public async Task<ProjectResponseDto?> GetByIdAsync(int projectId, int userId)
    {
        var project = await _db.Projects
            .Include(p => p.Job)
            .Include(p => p.Freelancer)
            .Include(p => p.Client)
            .Include(p => p.Payment)
            .Include(p => p.WorkImages)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null || (project.ClientId != userId && project.FreelancerId != userId)) return null;
        return Map(project);
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetByClientAsync(int clientId)
    {
        var projects = await _db.Projects
            .Include(p => p.Job)
            .Include(p => p.Freelancer)
            .Include(p => p.Client)
            .Include(p => p.Payment)
            .Include(p => p.WorkImages)
            .Where(p => p.ClientId == clientId)
            .OrderByDescending(p => p.StartedAt)
            .ToListAsync();
        return projects.Select(Map);
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetByFreelancerAsync(int freelancerId)
    {
        var projects = await _db.Projects
            .Include(p => p.Job)
            .Include(p => p.Freelancer)
            .Include(p => p.Client)
            .Include(p => p.Payment)
            .Include(p => p.WorkImages)
            .Where(p => p.FreelancerId == freelancerId)
            .OrderByDescending(p => p.StartedAt)
            .ToListAsync();
        return projects.Select(Map);
    }

    public async Task<ProjectResponseDto?> MarkCompleteAsync(int projectId, int userId, bool isClient)
    {
        var project = await _db.Projects
            .Include(p => p.Job)
            .Include(p => p.Freelancer)
            .Include(p => p.Client)
            .Include(p => p.Payment)
            .Include(p => p.WorkImages)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null) return null;

        if (isClient && project.ClientId != userId) return null;
        if (!isClient && project.FreelancerId != userId) return null;

        project.Status = ProjectStatus.Completed;
        project.CompletedAt = DateTime.UtcNow;
        project.Job.Status = JobStatus.Closed;

        await _db.SaveChangesAsync();
        return Map(project);
    }

    private static ProjectResponseDto Map(Models.Entities.Project project) => new()
    {
        ProjectId = project.ProjectId,
        JobId = project.JobId,
        JobTitle = project.Job.Title,
        FreelancerId = project.FreelancerId,
        FreelancerName = project.Freelancer.FullName,
        ClientId = project.ClientId,
        ClientName = project.Client.FullName,
        Status = project.Status.ToString(),
        StartedAt = project.StartedAt,
        CompletedAt = project.CompletedAt,
        PaymentAmount = project.Payment?.Amount,
        PaymentStatus = project.Payment?.Status.ToString(),
        WorkImages = project.WorkImages
            .Where(w => w.ImageType == ImageType.ProjectWork)
            .Select(w => new WorkImageDto
            {
                WorkImageId = w.WorkImageId,
                ImageUrl = w.ImageUrl,
                Caption = w.Caption,
                ImageType = w.ImageType.ToString(),
                ProjectId = w.ProjectId,
                CreatedAt = w.CreatedAt
            }).ToList()
    };
}
