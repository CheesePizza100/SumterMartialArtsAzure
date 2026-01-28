using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record InstructorLoginCreated(
    int InstructorId,
    string InstructorName,
    string InstructorEmail,
    string Username,
    string TemporaryPassword
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
