using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Domain.Events;

public record StudentCreated : IDomainEvent
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentEnrolledInProgram : IDomainEvent
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string InitialRank { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentWithdrewFromProgram : IDomainEvent
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public DateTime WithdrawnAt { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
public record StudentPromoted : IDomainEvent
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string PreviousRank { get; set; } = string.Empty;
    public string NewRank { get; set; } = string.Empty;
    public DateTime PromotedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentTestRecorded : IDomainEvent
{
    public int StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ProgramId { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public string RankTested { get; set; } = string.Empty;
    public bool Passed { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime TestDate { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}
public record StudentAttendanceRecorded : IDomainEvent
{
    public int StudentId { get; set; }
    public int ProgramId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public int ClassesAttended { get; set; }
    public int NewTotal { get; set; }
    public int NewAttendanceRate { get; set; }
    public DateTime RecordedAt { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}

public record StudentContactInfoUpdated : IDomainEvent
{
    public int StudentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public DateTime OccurredOn { get; init; } = DateTime.UtcNow;
}