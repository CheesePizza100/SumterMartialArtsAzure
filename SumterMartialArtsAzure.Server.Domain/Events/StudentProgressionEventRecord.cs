using System.Text.Json;

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

// Helper records for cleaner handling
public record EnrollmentEvent(int StudentId, int ProgramId, DateTime OccurredAt, EnrollmentEventData Data);
public record PromotionEvent(int StudentId, int ProgramId, DateTime OccurredAt, PromotionEventData Data);
public record TestAttemptEvent(int StudentId, int ProgramId, DateTime OccurredAt, TestAttemptEventData Data);

public record StudentProgressionState
{
    public string CurrentRank { get; init; } = "Not Enrolled";
    public DateTime? EnrolledDate { get; init; }
    public DateTime? LastTestDate { get; init; }
    public string? LastTestNotes { get; init; }
}

public interface IEventProjector
{
    string EventType { get; }
    StudentProgressionState Project(StudentProgressionEventRecord evt, StudentProgressionState currentState);
}
public abstract class EventProjectorBase<TEventData> : IEventProjector
{
    public abstract string EventType { get; }

    public StudentProgressionState Project(StudentProgressionEventRecord evt, StudentProgressionState currentState)
    {
        var data = JsonSerializer.Deserialize<TEventData>(evt.EventData);
        return data == null
            ? currentState
            : ProjectEvent(data, evt, currentState);
    }

    protected abstract StudentProgressionState ProjectEvent(
        TEventData data,
        StudentProgressionEventRecord evt,
        StudentProgressionState currentState);
}
public class EnrollmentEventProjector : EventProjectorBase<EnrollmentEventData>
{
    public override string EventType => nameof(EnrollmentEventData);

    protected override StudentProgressionState ProjectEvent(
        EnrollmentEventData data,
        StudentProgressionEventRecord evt,
        StudentProgressionState currentState)
    {
        return currentState with
        {
            CurrentRank = data.InitialRank ?? "Unknown",
            EnrolledDate = evt.OccurredAt
        };
    }
}

public class PromotionEventProjector : EventProjectorBase<PromotionEventData>
{
    public override string EventType => nameof(PromotionEventData);

    protected override StudentProgressionState ProjectEvent(
        PromotionEventData data,
        StudentProgressionEventRecord evt,
        StudentProgressionState currentState)
    {
        return currentState with
        {
            CurrentRank = data.ToRank,
            LastTestDate = evt.OccurredAt,
            LastTestNotes = data.Reason
        };
    }
}

public class TestAttemptEventProjector : EventProjectorBase<TestAttemptEventData>
{
    public override string EventType => nameof(TestAttemptEventData);

    protected override StudentProgressionState ProjectEvent(
        TestAttemptEventData data,
        StudentProgressionEventRecord evt,
        StudentProgressionState currentState)
    {
        return currentState with
        {
            LastTestDate = evt.OccurredAt,
            LastTestNotes = data.InstructorNotes
        };
    }
}
