using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Services.Interfaces;

public interface IPaymentService
{
    Task<PaymentResponseDto> FundAsync(int projectId, int clientId, FundPaymentDto dto);
    Task<PaymentResponseDto?> ReleaseAsync(int projectId, int clientId);
    Task<PaymentResponseDto?> GetByProjectAsync(int projectId, int userId);
    Task<IEnumerable<PaymentResponseDto>> GetAllAsync();
}
