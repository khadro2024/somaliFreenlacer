using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Models.Entities;

public class Payment
{
    public int PaymentId { get; set; }
    public int ProjectId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ReleasedAt { get; set; }

    public Project Project { get; set; } = null!;
}
