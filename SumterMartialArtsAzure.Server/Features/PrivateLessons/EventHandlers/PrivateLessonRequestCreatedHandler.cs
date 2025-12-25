using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestCreatedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestCreated>>
{
    private readonly ILogger<PrivateLessonRequestCreatedHandler> _logger;

    public PrivateLessonRequestCreatedHandler(
        ILogger<PrivateLessonRequestCreatedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<PrivateLessonRequestCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "New private lesson request created. RequestId: {RequestId}, Student: {StudentName}, Instructor: {InstructorId}",
            domainEvent.RequestId,
            domainEvent.StudentName,
            domainEvent.InstructorId);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}
public class PrivateLessonRequestApprovedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestApproved>>
{
    private readonly ILogger<PrivateLessonRequestApprovedHandler> _logger;

    public PrivateLessonRequestApprovedHandler(ILogger<PrivateLessonRequestApprovedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<PrivateLessonRequestApproved> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation(
            "Private lesson request approved. RequestId: {RequestId}, Student: {StudentName}",
            domainEvent.RequestId,
            domainEvent.StudentName);

        // TODO: Send approval email to student with calendar invite
        // TODO: Notify instructor
        // TODO: Create calendar event

        return Task.CompletedTask;
    }
}

public class PrivateLessonRequestRejectedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestRejected>>
{
    private readonly ILogger<PrivateLessonRequestRejectedHandler> _logger;

    public PrivateLessonRequestRejectedHandler(ILogger<PrivateLessonRequestRejectedHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<PrivateLessonRequestRejected> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;
        _logger.LogInformation(
            "Private lesson request rejected. RequestId: {RequestId}, Student: {StudentName}, Reason: {Reason}",
            domainEvent.RequestId,
            domainEvent.StudentName,
            domainEvent.Reason ?? "Not specified");

        // TODO: Send rejection email to student with reason
        // TODO: Log to audit trail

        return Task.CompletedTask;
    }
}