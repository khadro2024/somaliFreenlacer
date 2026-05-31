using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
}
