using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record StudentDeactivated(
    int StudentId,
    string StudentName,
    DateTime DeactivatedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}


public record StudentReactivated(
    int StudentId,
    string StudentName,
    DateTime ReactivatedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
