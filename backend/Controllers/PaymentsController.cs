using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _payments;

    public PaymentsController(IPaymentService payments) => _payments = payments;

    [Authorize(Roles = "Client")]
    [HttpPost("projects/{projectId}/fund")]
    public async Task<ActionResult<PaymentResponseDto>> Fund(int projectId, [FromBody] FundPaymentDto dto)
    {
        try
        {
            return Ok(await _payments.FundAsync(projectId, GetUserId(), dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize(Roles = "Client")]
    [HttpPost("projects/{projectId}/release")]
    public async Task<ActionResult<PaymentResponseDto>> Release(int projectId)
    {
        try
        {
            var result = await _payments.ReleaseAsync(projectId, GetUserId());
            return result == null ? NotFound() : Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("projects/{projectId}")]
    public async Task<ActionResult<PaymentResponseDto>> GetByProject(int projectId)
    {
        var payment = await _payments.GetByProjectAsync(projectId, GetUserId());
        return payment == null ? NotFound() : Ok(payment);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<PaymentResponseDto>>> GetAll() =>
        Ok(await _payments.GetAllAsync());

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
