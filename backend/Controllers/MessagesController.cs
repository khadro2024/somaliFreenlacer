using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messages;

    public MessagesController(IMessageService messages) => _messages = messages;

    [HttpPost]
    public async Task<ActionResult<MessageResponseDto>> Send([FromBody] SendMessageDto dto)
    {
        try
        {
            return Ok(await _messages.SendAsync(GetUserId(), dto));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("inbox")]
    public async Task<ActionResult<IEnumerable<MessageResponseDto>>> Inbox() =>
        Ok(await _messages.GetInboxAsync(GetUserId()));

    [HttpGet("conversation/{otherUserId}")]
    public async Task<ActionResult<IEnumerable<MessageResponseDto>>> Conversation(int otherUserId) =>
        Ok(await _messages.GetConversationAsync(GetUserId(), otherUserId));

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
