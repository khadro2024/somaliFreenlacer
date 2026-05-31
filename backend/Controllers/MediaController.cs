using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MediaController : ControllerBase
{
    private readonly IMediaService _media;

    public MediaController(IMediaService media) => _media = media;

    [HttpGet("config")]
    public IActionResult GetConfig() =>
        Ok(new { cloudinaryConfigured = _media.IsCloudinaryConfigured });

    [Authorize]
    [HttpPost("upload")]
    [RequestSizeLimit(6_000_000)]
    public async Task<IActionResult> Upload(
        IFormFile file,
        [FromForm] string type,
        [FromForm] int? jobId = null,
        [FromForm] int? projectId = null,
        [FromForm] string? caption = null)
    {
        if (!Enum.TryParse<ImageType>(type, true, out var imageType))
            return BadRequest(new { message = "Invalid type. Use: profile, portfolio, jobcover, projectwork" });

        try
        {
            var result = await _media.UploadAsync(GetUserId(), file, imageType, jobId, projectId, caption);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("portfolio/{userId}")]
    public async Task<IActionResult> GetPortfolio(int userId) =>
        Ok(await _media.GetUserPortfolioAsync(userId));

    [HttpGet("project/{projectId}")]
    public async Task<IActionResult> GetProjectImages(int projectId) =>
        Ok(await _media.GetProjectImagesAsync(projectId));

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var ok = await _media.DeleteAsync(id, GetUserId());
        return ok ? NoContent() : NotFound();
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
