using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IMessageService
{
    Task<MessageResponseDto> SendAsync(int senderId, SendMessageDto dto);
    Task<IEnumerable<MessageResponseDto>> GetConversationAsync(int userId, int otherUserId);
    Task<IEnumerable<MessageResponseDto>> GetInboxAsync(int userId);
}
