using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Services.Telemetry.Enrichers;

public class StudentPromotedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentPromoted>
{
    public void Enrich(StudentPromoted domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["ProgramId"] = domainEvent.ProgramId.ToString();
        properties["FromRank"] = domainEvent.PreviousRank;
        properties["ToRank"] = domainEvent.NewRank;
    }
}
public class StudentCreatedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentCreated>
{
    public void Enrich(StudentCreated domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
    }
}
public class StudentContactInfoUpdatedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentContactInfoUpdated>
{
    public void Enrich(StudentContactInfoUpdated domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
    }
}
public class StudentEnrolledInProgramTelemetryEnricher : IDomainEventTelemetryEnricher<StudentEnrolledInProgram>
{
    public void Enrich(StudentEnrolledInProgram domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["ProgramId"] = domainEvent.ProgramId.ToString();
        properties["InitialRank"] = domainEvent.InitialRank;
    }
}
public class StudentAttendanceRecordedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentAttendanceRecorded>
{
    public void Enrich(StudentAttendanceRecorded domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["ProgramId"] = domainEvent.ProgramId.ToString();
        properties["ClassesAttended"] = domainEvent.ClassesAttended.ToString();
        properties["NewTotal"] = domainEvent.NewTotal.ToString();
        properties["NewAttendanceRate"] = domainEvent.NewAttendanceRate.ToString();
    }
}
public class StudentTestRecordedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentTestRecorded>
{
    public void Enrich(StudentTestRecorded domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["ProgramId"] = domainEvent.ProgramId.ToString();
        properties["RankTested"] = domainEvent.RankTested;
        properties["Passed"] = domainEvent.Passed.ToString();
    }
}
public class StudentWithdrewFromProgramTelemetryEnricher : IDomainEventTelemetryEnricher<StudentWithdrewFromProgram>
{
    public void Enrich(StudentWithdrewFromProgram domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["ProgramId"] = domainEvent.ProgramId.ToString();
    }
}
public class InstructorLoginCreatedTelemetryEnricher : IDomainEventTelemetryEnricher<InstructorLoginCreated>
{
    public void Enrich(InstructorLoginCreated domainEvent, IDictionary<string, string> properties)
    {
        properties["InstructorId"] = domainEvent.InstructorId.ToString();
        properties["Username"] = domainEvent.Username;
    }
}
public class PrivateLessonRequestCreatedTelemetryEnricher : IDomainEventTelemetryEnricher<PrivateLessonRequestCreated>
{
    public void Enrich(PrivateLessonRequestCreated domainEvent, IDictionary<string, string> properties)
    {
        properties["RequestId"] = domainEvent.RequestId.ToString();
        properties["InstructorId"] = domainEvent.InstructorId.ToString();
        properties["RequestedStart"] = domainEvent.RequestedStart.ToString("O");
        properties["RequestedEnd"] = domainEvent.RequestedEnd.ToString("O");
    }
}
public class PrivateLessonRequestApprovedTelemetryEnricher : IDomainEventTelemetryEnricher<PrivateLessonRequestApproved>
{
    public void Enrich(PrivateLessonRequestApproved domainEvent, IDictionary<string, string> properties)
    {
        properties["RequestId"] = domainEvent.RequestId.ToString();
        properties["InstructorId"] = domainEvent.InstructorId.ToString();
    }
}
public class PrivateLessonRequestRejectedTelemetryEnricher : IDomainEventTelemetryEnricher<PrivateLessonRequestRejected>
{
    public void Enrich(PrivateLessonRequestRejected domainEvent, IDictionary<string, string> properties)
    {
        properties["RequestId"] = domainEvent.RequestId.ToString();
        properties["InstructorId"] = domainEvent.InstructorId.ToString();
        if (!string.IsNullOrEmpty(domainEvent.Reason))
            properties["Reason"] = domainEvent.Reason;
    }
}
public class StudentDeactivatedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentDeactivated>
{
    public void Enrich(StudentDeactivated domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
    }
}
public class StudentReactivatedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentReactivated>
{
    public void Enrich(StudentReactivated domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
    }
}
public class StudentLoginCreatedTelemetryEnricher : IDomainEventTelemetryEnricher<StudentLoginCreated>
{
    public void Enrich(StudentLoginCreated domainEvent, IDictionary<string, string> properties)
    {
        properties["StudentId"] = domainEvent.StudentId.ToString();
        properties["UserName"] = domainEvent.UserName;
    }
}






