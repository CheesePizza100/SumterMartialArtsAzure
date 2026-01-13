using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestRejectedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestRejected>>
{
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestRejectedHandler(EmailOrchestrator emailOrchestrator, AppDbContext dbContext)
    {
        _emailOrchestrator = emailOrchestrator;
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

        await _emailOrchestrator.SendAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            new SimpleEmailContentBuilder(EmailTemplateKeys.PrivateLessonRejected)
                .WithVariable("StudentName", domainEvent.StudentName)
                .WithVariable("InstructorName", request.Instructor.Name)
                .WithVariable("RequestedDate", request.RequestedLessonTime.Start.ToString("MMMM dd, yyyy 'at' h:mm tt"))
                .WithVariable("Reason", domainEvent.Reason ?? "The requested time slot if not available")
        );
    }
}