namespace SumterMartialArtsAzure.Server.Domain.Events;

public record StudentProgressionEventRecord
{
    public Guid EventId { get; set; }
    public int StudentId { get; set; }
    public int ProgramId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string EventData { get; set; } = string.Empty; // JSON
    public DateTime OccurredAt { get; set; }
    public int Version { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
public class EnrollmentEventData
{
    public string InitialRank { get; set; } = string.Empty;
    public int InstructorId { get; set; }
}

public class TestAttemptEventData
{
    public string RankTested { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public int TestingInstructorId { get; set; }
    public string InstructorNotes { get; set; } = string.Empty;
    public Dictionary<string, int> TechniqueScores { get; set; } = new();
}

public class PromotionEventData
{
    public string FromRank { get; set; } = string.Empty;
    public string ToRank { get; set; } = string.Empty;
    public int PromotingInstructorId { get; set; }
    public string Reason { get; set; } = string.Empty;
}