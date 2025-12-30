using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentPromotedEventHandler
    : INotificationHandler<DomainEventNotification<StudentPromoted>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentPromotedEventHandler> _logger;

    public StudentPromotedEventHandler(
        IEmailService emailService,
        AppDbContext dbContext,
        ILogger<StudentPromotedEventHandler> logger)
    {
        _logger = logger;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentPromoted> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student promoted. StudentId: {StudentId}, Student: {StudentName}, From: {FromRank}, To: {ToRank}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.PreviousRank,
            domainEvent.NewRank);

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found for promotion email", domainEvent.StudentId);
            return;
        }

        await _emailService.SendBeltPromotionEmailAsync(
            student.Email,
            student.Name,
            domainEvent.ProgramName,
            domainEvent.PreviousRank,
            domainEvent.NewRank,
            domainEvent.PromotedAt,
            domainEvent.Notes
        );

        _logger.LogInformation(
            "Promotion email sent to {StudentName} ({Email}) - {FromRank} to {ToRank}",
            student.Name,
            student.Email,
            domainEvent.PreviousRank,
            domainEvent.NewRank
        );
    }
}