using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class ApplicationService : IApplicationService
{
    private readonly ApplicationDbContext _db;

    public ApplicationService(ApplicationDbContext db) => _db = db;

    public async Task<ApplicationResponseDto> ApplyAsync(int jobId, int freelancerId, CreateApplicationDto dto)
    {
        var job = await _db.Jobs.FindAsync(jobId)
            ?? throw new InvalidOperationException("Job not found.");

        if (job.Status != JobStatus.Open)
            throw new InvalidOperationException("Job is not open for applications.");

        if (job.ClientId == freelancerId)
            throw new InvalidOperationException("Cannot apply to your own job.");

        if (await _db.Applications.AnyAsync(a => a.JobId == jobId && a.FreelancerId == freelancerId))
            throw new InvalidOperationException("Already applied to this job.");

        var application = new Application
        {
            JobId = jobId,
            FreelancerId = freelancerId,
            Proposal = dto.Proposal,
            BidAmount = dto.BidAmount,
            Status = ApplicationStatus.Pending
        };

        _db.Applications.Add(application);
        await _db.SaveChangesAsync();

        return (await GetApplicationDto(application.ApplicationId))!;
    }

    public async Task<ApplicationResponseDto?> AcceptAsync(int applicationId, int clientId)
    {
        var app = await _db.Applications
            .Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);

        if (app == null || app.Job.ClientId != clientId) return null;
        if (app.Status != ApplicationStatus.Pending) return null;

        app.Status = ApplicationStatus.Accepted;
        app.Job.Status = JobStatus.InProgress;

        var otherApps = await _db.Applications
            .Where(a => a.JobId == app.JobId && a.ApplicationId != applicationId && a.Status == ApplicationStatus.Pending)
            .ToListAsync();
        foreach (var other in otherApps)
            other.Status = ApplicationStatus.Rejected;

        var project = new Project
        {
            JobId = app.JobId,
            FreelancerId = app.FreelancerId,
            ClientId = clientId,
            Status = ProjectStatus.Working
        };
        _db.Projects.Add(project);

        var payment = new Payment
        {
            Project = project,
            Amount = app.BidAmount,
            Status = PaymentStatus.Pending
        };
        _db.Payments.Add(payment);

        await _db.SaveChangesAsync();
        return await GetApplicationDto(applicationId);
    }

    public async Task<ApplicationResponseDto?> RejectAsync(int applicationId, int clientId)
    {
        var app = await _db.Applications.Include(a => a.Job)
            .FirstOrDefaultAsync(a => a.ApplicationId == applicationId);
        if (app == null || app.Job.ClientId != clientId) return null;

        app.Status = ApplicationStatus.Rejected;
        await _db.SaveChangesAsync();
        return await GetApplicationDto(applicationId);
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetByJobAsync(int jobId, int clientId)
    {
        var job = await _db.Jobs.FindAsync(jobId);
        if (job == null || job.ClientId != clientId) return Enumerable.Empty<ApplicationResponseDto>();

        var apps = await _db.Applications
            .Include(a => a.Job)
            .Include(a => a.Freelancer).ThenInclude(f => f.FreelancerProfile)
            .Where(a => a.JobId == jobId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return apps.Select(Map);
    }

    public async Task<IEnumerable<ApplicationResponseDto>> GetByFreelancerAsync(int freelancerId)
    {
        var apps = await _db.Applications
            .Include(a => a.Job)
            .Include(a => a.Freelancer).ThenInclude(f => f.FreelancerProfile)
            .Where(a => a.FreelancerId == freelancerId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return apps.Select(Map);
    }

    private async Task<ApplicationResponseDto?> GetApplicationDto(int id)
    {
        var app = await _db.Applications
            .Include(a => a.Job)
            .Include(a => a.Freelancer).ThenInclude(f => f.FreelancerProfile)
            .FirstOrDefaultAsync(a => a.ApplicationId == id);
        return app == null ? null : Map(app);
    }

    private static ApplicationResponseDto Map(Application app) => new()
    {
        ApplicationId = app.ApplicationId,
        JobId = app.JobId,
        JobTitle = app.Job.Title,
        FreelancerId = app.FreelancerId,
        FreelancerName = app.Freelancer.FullName,
        Proposal = app.Proposal,
        BidAmount = app.BidAmount,
        Status = app.Status.ToString(),
        CreatedAt = app.CreatedAt,
        FreelancerRating = app.Freelancer.FreelancerProfile?.Rating
    };
}
