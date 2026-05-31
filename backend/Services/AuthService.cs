using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Helpers;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _db;
    private readonly JwtHelper _jwt;

    public AuthService(ApplicationDbContext db, JwtHelper jwt)
    {
        _db = db;
        _jwt = jwt;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
            throw new InvalidOperationException("Email already registered.");

        if (dto.Role == UserRole.Admin)
            throw new InvalidOperationException("Cannot register as admin.");

        var user = new User
        {
            FullName = dto.FullName,
            Email = dto.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = dto.Role,
            IsVerified = false
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        if (dto.Role == UserRole.Freelancer)
        {
            _db.FreelancerProfiles.Add(new FreelancerProfile { UserId = user.UserId });
            await _db.SaveChangesAsync();
        }

        return MapResponse(user);
    }

    public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
    {
        var email = (dto.Email ?? string.Empty).Trim();
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || user.IsSuspended) return null;
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash)) return null;
        return MapResponse(user);
    }

    private AuthResponseDto MapResponse(User user) => new()
    {
        Token = _jwt.GenerateToken(user),
        UserId = user.UserId,
        FullName = user.FullName,
        Email = user.Email,
        Role = user.Role.ToString(),
        IsVerified = user.IsVerified,
        ProfileImageUrl = user.ProfileImageUrl
    };
}
