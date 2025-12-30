using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentCreated>>
{
    private readonly IEmailService _emailService;

    public StudentCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(DomainEventNotification<StudentCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _emailService.SendSchoolWelcomeEmailAsync(
            domainEvent.Email,
            domainEvent.Name
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