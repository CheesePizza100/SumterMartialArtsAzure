using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestApprovedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestApproved>>
{
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestApprovedHandler(EmailOrchestrator emailOrchestrator, AppDbContext dbContext)
    {
        _emailOrchestrator = emailOrchestrator;
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

        await _emailOrchestrator.SendAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            new SimpleEmailContentBuilder(EmailTemplateKeys.PrivateLessonApproved)
                .WithVariable("StudentName", domainEvent.StudentName)
                .WithVariable("InstructorName", request.Instructor.Name)
                .WithVariable("ScheduledDate", domainEvent.RequestedStart.ToString("MMMM dd, yyyy 'at' h:mm tt"))
        );

        // TODO: Could also send notification to instructor
        // TODO: Could create calendar invite attachment
    }
}