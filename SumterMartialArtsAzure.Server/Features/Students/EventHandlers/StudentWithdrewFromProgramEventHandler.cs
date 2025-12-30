using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentWithdrewFromProgramEventHandler
    : INotificationHandler<DomainEventNotification<StudentWithdrewFromProgram>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;

    public StudentWithdrewFromProgramEventHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentWithdrewFromProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        // Get remaining active programs
        var remainingPrograms = student.ProgramEnrollments
            .Where(e => e.IsActive)
            .Select(e => e.ProgramName)
            .ToList();

        await _emailService.SendProgramWithdrawalEmailAsync(
            student.Email,
            student.Name,
            domainEvent.ProgramName,
            remainingPrograms
        );
    }
}