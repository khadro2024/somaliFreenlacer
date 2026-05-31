namespace SomaliFreelanceMarketplace.Models.Entities;

public class Message
{
    public int MessageId { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
    public string Text { get; set; } = string.Empty;
    public DateTime SentAt { get; set; } = DateTime.UtcNow;
    public bool IsRead { get; set; }

    public User Sender { get; set; } = null!;
    public User Receiver { get; set; } = null!;
}
