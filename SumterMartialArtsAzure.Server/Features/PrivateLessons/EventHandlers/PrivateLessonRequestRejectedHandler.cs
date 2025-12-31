using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestRejectedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestRejected>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestRejectedHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<PrivateLessonRequestRejected> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var request = await _dbContext.PrivateLessonRequests
            .Include(r => r.Instructor)
            .Include(privateLessonRequest => privateLessonRequest.RequestedLessonTime)
            .FirstOrDefaultAsync(r => r.Id == domainEvent.RequestId, cancellationToken);

        if (request == null)
        {
            return;
        }

        await _emailService.SendPrivateLessonRejectedAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            request.Instructor.Name,
            request.RequestedLessonTime.Start,
            domainEvent.Reason ?? "The requested time slot is not available"
        );
    }
}