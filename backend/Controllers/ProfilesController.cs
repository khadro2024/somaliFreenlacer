using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProfilesController : ControllerBase
{
    private readonly IProfileService _profiles;

    public ProfilesController(IProfileService profiles) => _profiles = profiles;

    [HttpGet("freelancers")]
    public async Task<ActionResult<IEnumerable<FreelancerProfileResponseDto>>> GetFreelancers() =>
        Ok(await _profiles.GetAllFreelancersAsync());

    [HttpGet("{userId}")]
    public async Task<ActionResult<FreelancerProfileResponseDto>> GetProfile(int userId)
    {
        var profile = await _profiles.GetProfileAsync(userId);
        return profile == null ? NotFound() : Ok(profile);
    }

    [Authorize(Roles = "Freelancer")]
    [HttpPut("me")]
    public async Task<ActionResult<FreelancerProfileResponseDto>> UpdateMyProfile([FromBody] UpdateFreelancerProfileDto dto) =>
        Ok(await _profiles.UpdateProfileAsync(GetUserId(), dto));

    [Authorize]
    [HttpPost("projects/{projectId}/rate")]
    public async Task<ActionResult<RatingResponseDto>> Rate(int projectId, [FromBody] CreateRatingDto dto)
    {
        try
        {
            return Ok(await _profiles.CreateRatingAsync(projectId, GetUserId(), dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
