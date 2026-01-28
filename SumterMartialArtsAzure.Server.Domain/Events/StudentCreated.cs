using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record StudentCreated(
    int StudentId,
    string Name,
    string Email,
    string Phone,
    DateTime CreatedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentEnrolledInProgram(
    int StudentId,
    string StudentName,
    int ProgramId,
    string ProgramName,
    string InitialRank,
    DateTime EnrolledAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}


public record StudentWithdrewFromProgram(
    int StudentId,
    string StudentName,
    int ProgramId,
    string ProgramName,
    DateTime WithdrawnAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentPromoted(
    int StudentId,
    string StudentName,
    int ProgramId,
    string ProgramName,
    string PreviousRank,
    string NewRank,
    DateTime PromotedAt,
    string Notes
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentTestRecorded(
    int StudentId,
    string StudentName,
    int ProgramId,
    string ProgramName,
    string RankTested,
    bool Passed,
    string Notes,
    DateTime TestDate
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}


public record StudentAttendanceRecorded(
    int StudentId,
    int ProgramId,
    string StudentName,
    int ClassesAttended,
    int NewTotal,
    int NewAttendanceRate,
    DateTime RecordedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentContactInfoUpdated(
    int StudentId,
    string Name,
    string Email,
    DateTime UpdatedAt
) : IDomainEvent
{
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
