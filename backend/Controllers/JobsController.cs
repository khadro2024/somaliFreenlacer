using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class JobsController : ControllerBase
{
    private readonly IJobService _jobs;

    public JobsController(IJobService jobs) => _jobs = jobs;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JobResponseDto>>> GetAll(
        [FromQuery] string? status,
        [FromQuery] int? categoryId) =>
        Ok(await _jobs.GetAllAsync(status, categoryId));

    [HttpGet("{id}")]
    public async Task<ActionResult<JobResponseDto>> GetById(int id)
    {
        var job = await _jobs.GetByIdAsync(id);
        return job == null ? NotFound() : Ok(job);
    }

    [Authorize(Roles = "Client")]
    [HttpGet("my")]
    public async Task<ActionResult<IEnumerable<JobResponseDto>>> GetMyJobs() =>
        Ok(await _jobs.GetByClientAsync(GetUserId()));

    [Authorize(Roles = "Client")]
    [HttpPost]
    public async Task<ActionResult<JobResponseDto>> Create([FromBody] CreateJobDto dto) =>
        Ok(await _jobs.CreateAsync(GetUserId(), dto));

    [Authorize(Roles = "Client")]
    [HttpPut("{id}")]
    public async Task<ActionResult<JobResponseDto>> Update(int id, [FromBody] UpdateJobDto dto)
    {
        var job = await _jobs.UpdateAsync(id, GetUserId(), dto);
        return job == null ? NotFound() : Ok(job);
    }

    [Authorize(Roles = "Client")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _jobs.DeleteAsync(id, GetUserId());
        return deleted ? NoContent() : NotFound();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
