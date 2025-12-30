using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentContactInfoUpdatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentContactInfoUpdated>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentContactInfoUpdatedEventHandler> _logger;

    public StudentContactInfoUpdatedEventHandler(
        IEmailService emailService,
        AppDbContext dbContext,
        ILogger<StudentContactInfoUpdatedEventHandler> logger)
    {
        _logger = logger;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentContactInfoUpdated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student contact info updated. StudentId: {StudentId}, Student: {Name}",
            domainEvent.StudentId,
            domainEvent.Name);

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found for contact info update email", domainEvent.StudentId);
            return;
        }

        await _emailService.SendContactInfoUpdatedEmailAsync(student.Email, student.Name);

        _logger.LogInformation(
            "Contact info update confirmation email sent to {StudentName} ({Email})",
            student.Name,
            student.Email
        );
    }
}