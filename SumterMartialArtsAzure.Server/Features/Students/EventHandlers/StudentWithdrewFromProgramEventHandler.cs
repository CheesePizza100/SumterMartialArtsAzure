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
    private readonly ILogger<StudentWithdrewFromProgramEventHandler> _logger;

    public StudentWithdrewFromProgramEventHandler(
        IEmailService emailService,
        AppDbContext dbContext,
        ILogger<StudentWithdrewFromProgramEventHandler> logger)
    {
        _logger = logger;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentWithdrewFromProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student withdrawn from program. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found for withdrawal email", domainEvent.StudentId);
            return;
        }

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

        _logger.LogInformation(
            "Withdrawal email sent to {StudentName} ({Email}) for {ProgramName}",
            student.Name,
            student.Email,
            domainEvent.ProgramName
        );

    }
}