using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class ProfileService : IProfileService
{
    private readonly ApplicationDbContext _db;

    public ProfileService(ApplicationDbContext db) => _db = db;

    public async Task<FreelancerProfileResponseDto?> GetProfileAsync(int userId)
    {
        var profile = await _db.FreelancerProfiles
            .Include(p => p.User)
            .FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null) return null;
        var dto = Map(profile);
        dto.PortfolioImages = await LoadPortfolioAsync(userId);
        return dto;
    }

    public async Task<FreelancerProfileResponseDto> UpdateProfileAsync(int userId, UpdateFreelancerProfileDto dto)
    {
        var profile = await _db.FreelancerProfiles.FirstOrDefaultAsync(p => p.UserId == userId);
        if (profile == null)
        {
            profile = new FreelancerProfile { UserId = userId };
            _db.FreelancerProfiles.Add(profile);
        }

        if (dto.Skills != null) profile.Skills = dto.Skills;
        if (dto.Bio != null) profile.Bio = dto.Bio;
        if (dto.HourlyRate.HasValue) profile.HourlyRate = dto.HourlyRate.Value;
        if (dto.Portfolio != null) profile.Portfolio = dto.Portfolio;

        await _db.SaveChangesAsync();
        await _db.Entry(profile).Reference(p => p.User).LoadAsync();
        return Map(profile);
    }

    public async Task<IEnumerable<FreelancerProfileResponseDto>> GetAllFreelancersAsync()
    {
        var profiles = await _db.FreelancerProfiles
            .Include(p => p.User)
            .Where(p => !p.User.IsSuspended)
            .OrderByDescending(p => p.Rating)
            .ToListAsync();
        var list = new List<FreelancerProfileResponseDto>();
        foreach (var p in profiles)
        {
            var dto = Map(p);
            dto.PortfolioImages = await LoadPortfolioAsync(p.UserId);
            list.Add(dto);
        }
        return list;
    }

    public async Task<RatingResponseDto> CreateRatingAsync(int projectId, int raterId, CreateRatingDto dto)
    {
        var project = await _db.Projects
            .Include(p => p.Freelancer)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId)
            ?? throw new InvalidOperationException("Project not found.");

        if (project.Status != ProjectStatus.Completed)
            throw new InvalidOperationException("Can only rate completed projects.");

        if (project.ClientId != raterId && project.FreelancerId != raterId)
            throw new InvalidOperationException("Not authorized to rate this project.");

        var ratedUserId = project.ClientId == raterId ? project.FreelancerId : project.ClientId;

        if (await _db.Ratings.AnyAsync(r => r.ProjectId == projectId && r.RaterId == raterId))
            throw new InvalidOperationException("Already rated this project.");

        var rater = await _db.Users.FindAsync(raterId);
        var rating = new Rating
        {
            ProjectId = projectId,
            RaterId = raterId,
            RatedUserId = ratedUserId,
            Stars = dto.Stars,
            ReviewText = dto.ReviewText
        };

        _db.Ratings.Add(rating);

        if (ratedUserId == project.FreelancerId)
        {
            var profile = await _db.FreelancerProfiles.FirstOrDefaultAsync(p => p.UserId == ratedUserId);
            if (profile != null)
            {
                var total = profile.Rating * profile.RatingCount + dto.Stars;
                profile.RatingCount++;
                profile.Rating = total / profile.RatingCount;
            }
        }

        await _db.SaveChangesAsync();

        return new RatingResponseDto
        {
            RatingId = rating.RatingId,
            RaterName = rater!.FullName,
            Stars = rating.Stars,
            ReviewText = rating.ReviewText,
            CreatedAt = rating.CreatedAt
        };
    }

    private async Task<List<WorkImageDto>> LoadPortfolioAsync(int userId)
    {
        var images = await _db.WorkImages
            .Include(w => w.Job)
            .Where(w => w.UserId == userId && w.ImageType == ImageType.Portfolio)
            .OrderByDescending(w => w.CreatedAt)
            .ToListAsync();
        return images.Select(w => new WorkImageDto
        {
            WorkImageId = w.WorkImageId,
            ImageUrl = w.ImageUrl,
            Caption = w.Caption,
            ImageType = w.ImageType.ToString(),
            JobId = w.JobId,
            ProjectId = w.ProjectId,
            JobTitle = w.Job?.Title,
            CreatedAt = w.CreatedAt
        }).ToList();
    }

    private static FreelancerProfileResponseDto Map(FreelancerProfile profile) => new()
    {
        ProfileId = profile.ProfileId,
        UserId = profile.UserId,
        FullName = profile.User.FullName,
        Email = profile.User.Email,
        Skills = profile.Skills,
        Bio = profile.Bio,
        HourlyRate = profile.HourlyRate,
        Portfolio = profile.Portfolio,
        Rating = profile.Rating,
        RatingCount = profile.RatingCount,
        ProfileImageUrl = profile.User.ProfileImageUrl
    };
}
