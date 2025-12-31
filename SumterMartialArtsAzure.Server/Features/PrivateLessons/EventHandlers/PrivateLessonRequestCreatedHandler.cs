using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.EventHandlers;

public class PrivateLessonRequestCreatedHandler
    : INotificationHandler<DomainEventNotification<PrivateLessonRequestCreated>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;

    public PrivateLessonRequestCreatedHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
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

        await _emailService.SendPrivateLessonRequestConfirmationAsync(
            domainEvent.StudentEmail,
            domainEvent.StudentName,
            instructor.Name,
            domainEvent.RequestedStart
        );

        // Send notification to admin (you'd configure admin email in settings)
        // For now, you could use a hardcoded admin email or configuration setting
        var adminEmail = "admin@sumtermartialarts.com"; // TODO: Get from configuration
        await _emailService.SendPrivateLessonAdminNotificationAsync(
            adminEmail,
            domainEvent.StudentName,
            instructor.Name,
            domainEvent.RequestedStart
        );
    }
}