using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentLoginCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentLoginCreated>>
{
    private readonly EmailOrchestrator _emailOrchestrator;

    public StudentLoginCreatedEventHandler(EmailOrchestrator emailOrchestrator)
    {
        _emailOrchestrator = emailOrchestrator;
    }

    public async Task Handle(DomainEventNotification<StudentLoginCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _emailOrchestrator.SendAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            new SimpleEmailContentBuilder(EmailTemplateKeys.StudentLoginCredentials)
                .WithVariable("StudentName", domainEvent.StudentName)
                .WithVariable("UserName", domainEvent.UserName)
                .WithVariable("TemporaryPassword", domainEvent.TemporaryPassword)
        );
    }
}