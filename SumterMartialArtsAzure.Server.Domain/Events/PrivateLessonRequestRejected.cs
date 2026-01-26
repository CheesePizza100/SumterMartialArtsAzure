using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record PrivateLessonRequestRejected(
    int RequestId,
    int InstructorId,
    string StudentName,
    string StudentEmail,
    string? Reason
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
