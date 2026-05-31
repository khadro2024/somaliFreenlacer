using System.ComponentModel.DataAnnotations;

namespace SomaliFreelanceMarketplace.Models.DTOs;

public class SendMessageDto
{
    [Required]
    public int ReceiverId { get; set; }

    [Required, MaxLength(2000)]
    public string Text { get; set; } = string.Empty;
}

public class MessageResponseDto
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public int ReceiverId { get; set; }
    public string ReceiverName { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; }
    public bool IsRead { get; set; }
}
