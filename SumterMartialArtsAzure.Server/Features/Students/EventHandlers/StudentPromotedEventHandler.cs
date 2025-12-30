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

    public StudentPromotedEventHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentPromoted> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        await _emailService.SendBeltPromotionEmailAsync(
            student.Email,
            student.Name,
            domainEvent.ProgramName,
            domainEvent.PreviousRank,
            domainEvent.NewRank,
            domainEvent.PromotedAt,
            domainEvent.Notes
        );
    }
}