using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ApplicationsController : ControllerBase
{
    private readonly IApplicationService _applications;

    public ApplicationsController(IApplicationService applications) => _applications = applications;

    [Authorize(Roles = "Freelancer")]
    [HttpPost("jobs/{jobId}")]
    public async Task<ActionResult<ApplicationResponseDto>> Apply(int jobId, [FromBody] CreateApplicationDto dto)
    {
        try
        {
            return Ok(await _applications.ApplyAsync(jobId, GetUserId(), dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Freelancer")]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetMyApplications() =>
        Ok(await _applications.GetByFreelancerAsync(GetUserId()));

    [Authorize(Roles = "Client")]
    [HttpGet("jobs/{jobId}")]
    public async Task<ActionResult<IEnumerable<ApplicationResponseDto>>> GetByJob(int jobId) =>
        Ok(await _applications.GetByJobAsync(jobId, GetUserId()));

    [Authorize(Roles = "Client")]
    [HttpPost("{id}/accept")]
    public async Task<ActionResult<ApplicationResponseDto>> Accept(int id)
    {
        var result = await _applications.AcceptAsync(id, GetUserId());
        return result == null ? NotFound() : Ok(result);
    }

    [Authorize(Roles = "Client")]
    [HttpPost("{id}/reject")]
    public async Task<ActionResult<ApplicationResponseDto>> Reject(int id)
    {
        var result = await _applications.RejectAsync(id, GetUserId());
        return result == null ? NotFound() : Ok(result);
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
