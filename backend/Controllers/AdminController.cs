using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IAdminService _admin;

    public AdminController(IAdminService admin) => _admin = admin;

    [HttpGet("stats")]
    public async Task<ActionResult<DashboardStatsDto>> GetStats() =>
        Ok(await _admin.GetStatsAsync());

    [HttpGet("users")]
    public async Task<ActionResult<IEnumerable<UserAdminDto>>> GetUsers() =>
        Ok(await _admin.GetUsersAsync());

    [HttpPost("users/{id}/suspend")]
    public async Task<IActionResult> Suspend(int id, [FromQuery] bool suspend = true)
    {
        var ok = await _admin.SuspendUserAsync(id, suspend);
        return ok ? Ok() : NotFound();
    }

    [HttpPost("users/{id}/verify")]
    public async Task<IActionResult> Verify(int id)
    {
        var ok = await _admin.VerifyUserAsync(id);
        return ok ? Ok() : NotFound();
    }

    [HttpGet("jobs")]
    public async Task<ActionResult<IEnumerable<JobResponseDto>>> GetJobs() =>
        Ok(await _admin.GetAllJobsAsync());
}
