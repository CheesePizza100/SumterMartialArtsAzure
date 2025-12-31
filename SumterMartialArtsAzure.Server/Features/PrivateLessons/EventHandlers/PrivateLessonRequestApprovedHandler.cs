using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestApprovedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestApproved>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestApprovedHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<PrivateLessonRequestApproved> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var request = await _dbContext.PrivateLessonRequests
            .Include(r => r.Instructor)
            .FirstOrDefaultAsync(r => r.Id == domainEvent.RequestId, cancellationToken);

        if (request == null)
        {
            return;
        }

        await _emailService.SendPrivateLessonApprovedAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            request.Instructor.Name,
            domainEvent.RequestedStart
        );

        // TODO: Could also send notification to instructor
        // TODO: Could create calendar invite attachment
    }
}