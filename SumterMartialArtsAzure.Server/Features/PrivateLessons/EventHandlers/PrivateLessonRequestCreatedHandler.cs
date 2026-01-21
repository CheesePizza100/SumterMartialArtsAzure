using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestCreatedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestCreated>>
{
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestCreatedHandler(EmailOrchestrator emailOrchestrator, AppDbContext dbContext)
    {
        _emailOrchestrator = emailOrchestrator;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<PrivateLessonRequestCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var instructor = await _dbContext.Instructors
            .FirstOrDefaultAsync(i => i.Id == domainEvent.InstructorId, cancellationToken);

        if (instructor == null)
        {
            return;
        }

        await _emailOrchestrator.SendAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            new SimpleEmailContentBuilder(EmailTemplateKeys.PrivateLessonRequestConfirmation)
                .WithVariable("StudentName", domainEvent.StudentName)
                .WithVariable("InstructorName", instructor.Name)
                .WithVariable("RequestedDate", domainEvent.RequestedStart.ToString("MMMM dd, yyyy 'at' h:mm tt"))
        );

        var admin = await _dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Username == "admin" && u.Role == UserRole.Admin, cancellationToken);

        if (admin != null)
        {
            await _emailOrchestrator.SendAsync(
                admin.Email,
                admin.Username,
                new SimpleEmailContentBuilder(EmailTemplateKeys.PrivateLessonAdminNotification)
                    .WithVariable("StudentName", domainEvent.StudentName)
                    .WithVariable("InstructorName", instructor.Name)
                    .WithVariable("RequestedDate", domainEvent.RequestedStart.ToString("MMMM dd, yyyy 'at' h:mm tt"))
            );
        }
    }
}