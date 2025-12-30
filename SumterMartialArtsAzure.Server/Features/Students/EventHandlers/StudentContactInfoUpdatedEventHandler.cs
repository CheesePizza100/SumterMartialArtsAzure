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

    public StudentContactInfoUpdatedEventHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentContactInfoUpdated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        await _emailService.SendContactInfoUpdatedEmailAsync(student.Email, student.Name);
    }
}