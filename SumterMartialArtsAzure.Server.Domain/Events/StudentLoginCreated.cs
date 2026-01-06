using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public class StudentLoginCreated : IDomainEvent
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string StudentEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string TemporaryPassword { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}