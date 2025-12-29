using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public class StudentDeactivated : IDomainEvent
{
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public DateTime DeactivatedAt { get; init; }

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public class StudentReactivated : IDomainEvent
{
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public DateTime ReactivatedAt { get; init; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}