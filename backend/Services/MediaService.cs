using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class MediaService : IMediaService
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".webp", ".gif"];
    private const long MaxBytes = 5 * 1024 * 1024;

    private readonly ApplicationDbContext _db;
    private readonly CloudinaryStorageService _cloudinary;

    public MediaService(ApplicationDbContext db, CloudinaryStorageService cloudinary)
    {
        _db = db;
        _cloudinary = cloudinary;
    }

    public bool IsCloudinaryConfigured => _cloudinary.IsConfigured;

    public async Task<UploadImageResponseDto> UploadAsync(
        int userId, IFormFile file, ImageType type, int? jobId, int? projectId, string? caption)
    {
        ValidateFile(file);
        if (!_cloudinary.IsConfigured)
            throw new InvalidOperationException("Cloudinary ma configured. Ku dar CloudName, ApiKey, ApiSecret appsettings.json.");

        await ValidateOwnershipAsync(userId, type, jobId, projectId);

        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        var subFolder = type switch
        {
            ImageType.Profile => "profiles",
            ImageType.Portfolio => "portfolio",
            ImageType.JobCover => "jobs",
            ImageType.ProjectWork => "projects",
            _ => "misc"
        };

        await using var stream = file.OpenReadStream();
        var (url, publicId) = await _cloudinary.UploadImageAsync(stream, $"{userId}_{Guid.NewGuid():N}{ext}", subFolder);

        if (type == ImageType.Profile)
        {
            var user = await _db.Users.FindAsync(userId) ?? throw new InvalidOperationException("User not found.");
            user.ProfileImageUrl = url;
            await _db.SaveChangesAsync();
            return new UploadImageResponseDto { ImageUrl = url, Message = "Profile image updated." };
        }

        if (type == ImageType.JobCover)
        {
            var job = await _db.Jobs.FirstOrDefaultAsync(j => j.JobId == jobId && j.ClientId == userId)
                ?? throw new InvalidOperationException("Job not found.");
            job.CoverImageUrl = url;
            await _db.SaveChangesAsync();
            return new UploadImageResponseDto { ImageUrl = url, Message = "Job cover updated." };
        }

        var workImage = new WorkImage
        {
            UserId = userId,
            ImageUrl = url,
            PublicId = publicId,
            Caption = caption ?? "",
            ImageType = type,
            JobId = jobId,
            ProjectId = projectId
        };
        _db.WorkImages.Add(workImage);
        await _db.SaveChangesAsync();

        return new UploadImageResponseDto
        {
            ImageUrl = url,
            WorkImageId = workImage.WorkImageId,
            Message = type == ImageType.Portfolio ? "Portfolio image added." : "Project work image added."
        };
    }

    public async Task<IEnumerable<WorkImageDto>> GetUserPortfolioAsync(int userId)
    {
        var images = await _db.WorkImages
            .Include(w => w.Job)
            .Where(w => w.UserId == userId && w.ImageType == ImageType.Portfolio)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
        return images.Select(Map);
    }

    public async Task<IEnumerable<WorkImageDto>> GetProjectImagesAsync(int projectId)
    {
        var images = await _db.WorkImages
            .Include(w => w.Job)
            .Where(w => w.ProjectId == projectId && w.ImageType == ImageType.ProjectWork)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
        return images.Select(Map);
    }

    public async Task<bool> DeleteAsync(int workImageId, int userId)
    {
        var image = await _db.WorkImages.FirstOrDefaultAsync(w => w.WorkImageId == workImageId && w.UserId == userId);
        if (image == null) return false;

        if (!string.IsNullOrEmpty(image.PublicId))
            await _cloudinary.DeleteAsync(image.PublicId);

        _db.WorkImages.Remove(image);
        await _db.SaveChangesAsync();
        return true;
    }

    private static void ValidateFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new InvalidOperationException("No file uploaded.");
        if (file.Length > MaxBytes)
            throw new InvalidOperationException("File max size is 5MB.");
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(ext))
            throw new InvalidOperationException("Allowed: JPG, PNG, WEBP, GIF.");
    }

    private async Task ValidateOwnershipAsync(int userId, ImageType type, int? jobId, int? projectId)
    {
        if (type == ImageType.JobCover && jobId == null)
            throw new InvalidOperationException("jobId required for job cover.");

        if (type == ImageType.JobCover)
        {
            if (!await _db.Jobs.AnyAsync(j => j.JobId == jobId && j.ClientId == userId))
                throw new InvalidOperationException("Not your job.");
        }

        if (type == ImageType.ProjectWork)
        {
            if (projectId == null)
                throw new InvalidOperationException("projectId required.");
            var project = await _db.Projects.FindAsync(projectId)
                ?? throw new InvalidOperationException("Project not found.");
            if (project.FreelancerId != userId && project.ClientId != userId)
                throw new InvalidOperationException("Not authorized.");
        }
    }

    private static WorkImageDto Map(WorkImage w) => new()
    {
        WorkImageId = w.WorkImageId,
        ImageUrl = w.ImageUrl,
        Caption = w.Caption,
        ImageType = w.ImageType.ToString(),
        JobId = w.JobId,
        ProjectId = w.ProjectId,
        JobTitle = w.Job?.Title,
        CreatedAt = w.CreatedAt
    };
}
