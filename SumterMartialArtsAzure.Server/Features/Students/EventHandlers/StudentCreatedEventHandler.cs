using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentCreated>>
{
    private readonly ILogger<StudentCreatedEventHandler> _logger;

    public StudentCreatedEventHandler(ILogger<StudentCreatedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "New student created. StudentId: {RequestId}, Student: {StudentName}",
            domainEvent.StudentId,
            domainEvent.Name);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}
public class StudentEnrolledInProgramEventHandler
    : INotificationHandler<DomainEventNotification<StudentEnrolledInProgram>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentEnrolledInProgramEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentEnrolledInProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student enrolled in program. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}

public class StudentWithdrewFromProgramEventHandler
    : INotificationHandler<DomainEventNotification<StudentWithdrewFromProgram>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentWithdrewFromProgramEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentWithdrewFromProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student withdrawn from program. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}
public class StudentTestRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentTestRecorded>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentTestRecordedEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentTestRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student test recorded. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}

public class StudentAttendanceRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentAttendanceRecorded>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentAttendanceRecordedEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentAttendanceRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student attendance recorded. StudentId: {StudentId}, Student: {StudentName}",
            domainEvent.StudentId,
            domainEvent.StudentName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}
public class StudentContactInfoUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentContactInfoUpdated>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentContactInfoUpdatedEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentContactInfoUpdated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student contact info updated. StudentId: {StudentId}, Student: {Name}",
            domainEvent.StudentId,
            domainEvent.Name);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}
