using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public class InstructorLoginCreated : IDomainEvent
{
    public int InstructorId { get; set; }
    public string InstructorName { get; set; } = string.Empty;
    public string InstructorEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}