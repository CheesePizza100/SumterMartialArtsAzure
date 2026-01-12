using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentCreated>>
{
    private readonly EmailOrchestrator _emailOrchestrator;

    public StudentCreatedEventHandler(EmailOrchestrator emailOrchestrator)
    {
        _emailOrchestrator = emailOrchestrator;
    }

    public Task Handle(DomainEventNotification<StudentCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _emailOrchestrator.SendAsync(
            domainEvent.Email,
            domainEvent.Name,
            new SimpleEmailContentBuilder(EmailTemplateKeys.SchoolWelcome)
                .WithVariable("StudentName", domainEvent.Name)
        );
    }
}

public class StudentTestRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentTestRecorded>>
{
    public StudentTestRecordedEventHandler()
    {
    }

    public Task Handle(DomainEventNotification<StudentTestRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}

public class StudentAttendanceRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentAttendanceRecorded>>
{

    public StudentAttendanceRecordedEventHandler()
    {
    }

    public Task Handle(DomainEventNotification<StudentAttendanceRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return Task.CompletedTask;
    }
}