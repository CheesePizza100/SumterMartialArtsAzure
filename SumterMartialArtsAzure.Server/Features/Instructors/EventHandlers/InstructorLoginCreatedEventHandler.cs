using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.EventHandlers;

public class InstructorLoginCreatedEventHandler
    : INotificationHandler<DomainEventNotification<InstructorLoginCreated>>
{
    private readonly EmailOrchestrator _emailOrchestrator;

    public InstructorLoginCreatedEventHandler(EmailOrchestrator emailOrchestrator)
    {
        _emailOrchestrator = emailOrchestrator;
    }

    public async Task Handle(
        DomainEventNotification<InstructorLoginCreated> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _emailOrchestrator.SendAsync(
            domainEvent.InstructorEmail,
            domainEvent.InstructorName,
            new SimpleEmailContentBuilder(EmailTemplateKeys.InstructorLoginCredentials)
                .WithVariable("InstructorName", domainEvent.InstructorName)
                .WithVariable("UserName", domainEvent.Username)
                .WithVariable("TemporaryPassword", domainEvent.TemporaryPassword)
        );
    }
}