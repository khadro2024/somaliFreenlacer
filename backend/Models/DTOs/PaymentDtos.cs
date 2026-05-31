using System.ComponentModel.DataAnnotations;

namespace SomaliFreelanceMarketplace.Models.DTOs;

public class FundPaymentDto
{
    [Range(1, double.MaxValue)]
    public decimal Amount { get; set; }
}

public class PaymentResponseDto
{
    public int PaymentId { get; set; }
    public int ProjectId { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? ReleasedAt { get; set; }
}
