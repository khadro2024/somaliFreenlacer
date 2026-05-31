using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _db;

    public MessageService(ApplicationDbContext db) => _db = db;

    public async Task<MessageResponseDto> SendAsync(int senderId, SendMessageDto dto)
    {
        var receiver = await _db.Users.FindAsync(dto.ReceiverId)
            ?? throw new InvalidOperationException("Receiver not found.");

        var message = new Message
        {
            SenderId = senderId,
            ReceiverId = dto.ReceiverId,
            Text = dto.Text
        };

        _db.Messages.Add(message);
        await _db.SaveChangesAsync();

        var sender = await _db.Users.FindAsync(senderId);
        return new MessageResponseDto
        {
            MessageId = message.MessageId,
            SenderId = senderId,
            SenderName = sender!.FullName,
            ReceiverId = dto.ReceiverId,
            ReceiverName = receiver.FullName,
            Text = message.Text,
            SentAt = message.SentAt,
            IsRead = false
        };
    }

    public async Task<IEnumerable<MessageResponseDto>> GetConversationAsync(int userId, int otherUserId)
    {
        var messages = await _db.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => (m.SenderId == userId && m.ReceiverId == otherUserId) ||
                        (m.SenderId == otherUserId && m.ReceiverId == userId))
            .OrderBy(m => m.SentAt)
            .ToListAsync();

        var unread = messages.Where(m => m.ReceiverId == userId && !m.IsRead).ToList();
        foreach (var m in unread) m.IsRead = true;
        if (unread.Count > 0) await _db.SaveChangesAsync();

        return messages.Select(Map);
    }

    public async Task<IEnumerable<MessageResponseDto>> GetInboxAsync(int userId)
    {
        var messages = await _db.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.SenderId == userId || m.ReceiverId == userId)
            .OrderByDescending(m => m.SentAt)
            .Take(100)
            .ToListAsync();

        return messages.Select(Map);
    }

    private static MessageResponseDto Map(Message m) => new()
    {
        MessageId = m.MessageId,
        SenderId = m.SenderId,
        SenderName = m.Sender.FullName,
        ReceiverId = m.ReceiverId,
        ReceiverName = m.Receiver.FullName,
        Text = m.Text,
        SentAt = m.SentAt,
        IsRead = m.IsRead
    };
}
