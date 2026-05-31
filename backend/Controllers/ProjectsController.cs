using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projects;

    public ProjectsController(IProjectService projects) => _projects = projects;

    [HttpGet("{id}")]
    public async Task<ActionResult<ProjectResponseDto>> GetById(int id)
    {
        var project = await _projects.GetByIdAsync(id, GetUserId());
        return project == null ? NotFound() : Ok(project);
    }

    [Authorize(Roles = "Client")]
    [HttpGet("client")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetClientProjects() =>
        Ok(await _projects.GetByClientAsync(GetUserId()));

    [Authorize(Roles = "Freelancer")]
    [HttpGet("freelancer")]
    public async Task<ActionResult<IEnumerable<ProjectResponseDto>>> GetFreelancerProjects() =>
        Ok(await _projects.GetByFreelancerAsync(GetUserId()));

    [Authorize(Roles = "Client")]
    [HttpPost("{id}/complete")]
    public async Task<ActionResult<ProjectResponseDto>> ClientComplete(int id)
    {
        var project = await _projects.MarkCompleteAsync(id, GetUserId(), true);
        return project == null ? NotFound() : Ok(project);
    }

    [Authorize(Roles = "Freelancer")]
    [HttpPost("{id}/submit")]
    public async Task<ActionResult<ProjectResponseDto>> FreelancerSubmit(int id)
    {
        var project = await _projects.MarkCompleteAsync(id, GetUserId(), false);
        return project == null ? NotFound() : Ok(project);
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
