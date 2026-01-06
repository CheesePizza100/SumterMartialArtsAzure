using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentLoginCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentLoginCreated>>
{
    private readonly IEmailService _emailService;

    public StudentLoginCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public Task Handle(DomainEventNotification<StudentLoginCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        return _emailService.SendStudentLoginCredentialsAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            domainEvent.Username,
            domainEvent.TemporaryPassword
        );
    }
}