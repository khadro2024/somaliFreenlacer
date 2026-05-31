using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;
using SomaliFreelanceMarketplace.Models.Enums;
using SomaliFreelanceMarketplace.Services.Interfaces;

namespace SomaliFreelanceMarketplace.Services;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _db;

    public PaymentService(ApplicationDbContext db) => _db = db;

    public async Task<PaymentResponseDto> FundAsync(int projectId, int clientId, FundPaymentDto dto)
    {
        var project = await _db.Projects.Include(p => p.Payment)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.ClientId == clientId)
            ?? throw new InvalidOperationException("Project not found.");

        if (project.Payment == null)
            throw new InvalidOperationException("Payment record not found.");

        if (project.Payment.Status != PaymentStatus.Pending)
            throw new InvalidOperationException("Payment already funded.");

        project.Payment.Amount = dto.Amount;
        project.Payment.Status = PaymentStatus.Held;

        await _db.SaveChangesAsync();
        return Map(project.Payment);
    }

    public async Task<PaymentResponseDto?> ReleaseAsync(int projectId, int clientId)
    {
        var project = await _db.Projects
            .Include(p => p.Payment)
            .Include(p => p.Freelancer)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId && p.ClientId == clientId);

        if (project?.Payment == null) return null;

        if (project.Status != ProjectStatus.Completed)
            throw new InvalidOperationException("Project must be completed before payment release.");

        if (project.Payment.Status != PaymentStatus.Held)
            throw new InvalidOperationException("Payment is not in held status.");

        if (!project.Freelancer.IsVerified)
            throw new InvalidOperationException("Freelancer must be verified before payment release.");

        project.Payment.Status = PaymentStatus.Released;
        project.Payment.ReleasedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
        return Map(project.Payment);
    }

    public async Task<PaymentResponseDto?> GetByProjectAsync(int projectId, int userId)
    {
        var project = await _db.Projects.Include(p => p.Payment)
            .FirstOrDefaultAsync(p => p.ProjectId == projectId);

        if (project == null || (project.ClientId != userId && project.FreelancerId != userId))
            return null;

        return project.Payment == null ? null : Map(project.Payment);
    }

    public async Task<IEnumerable<PaymentResponseDto>> GetAllAsync()
    {
        var payments = await _db.Payments.OrderByDescending(p => p.CreatedAt).ToListAsync();
        return payments.Select(Map);
    }

    private static PaymentResponseDto Map(Models.Entities.Payment payment) => new()
    {
        PaymentId = payment.PaymentId,
        ProjectId = payment.ProjectId,
        Amount = payment.Amount,
        Status = payment.Status.ToString(),
        CreatedAt = payment.CreatedAt,
        ReleasedAt = payment.ReleasedAt
    };
}
