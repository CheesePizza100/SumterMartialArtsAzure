using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record PrivateLessonRequestCreated : IDomainEvent
{
    public int RequestId { get; init; }
    public int InstructorId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public string StudentEmail { get; init; } = string.Empty;
    public DateTime RequestedStart { get; init; }
    public DateTime RequestedEnd { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}