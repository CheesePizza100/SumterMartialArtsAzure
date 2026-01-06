using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.EventHandlers;

public class InstructorLoginCreatedEventHandler
    : INotificationHandler<DomainEventNotification<InstructorLoginCreated>>
{
    private readonly IEmailService _emailService;

    public InstructorLoginCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task Handle(
        DomainEventNotification<InstructorLoginCreated> notification,
        CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        await _emailService.SendInstructorLoginCredentialsAsync(
            domainEvent.InstructorEmail,
            domainEvent.InstructorName,
            domainEvent.Username,
            domainEvent.TemporaryPassword
        );
    }
}