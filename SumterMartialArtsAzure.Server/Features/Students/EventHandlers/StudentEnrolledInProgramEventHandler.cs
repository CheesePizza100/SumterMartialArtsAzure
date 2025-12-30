using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentEnrolledInProgramEventHandler
    : INotificationHandler<DomainEventNotification<StudentEnrolledInProgram>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentEnrolledInProgramEventHandler(
        IEmailService emailService,
        AppDbContext dbContext,
        ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _emailService = emailService;
        _dbContext = dbContext;
        _logger = logger;
    }

    public async Task Handle(DomainEventNotification<StudentEnrolledInProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student enrolled in program. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        if (student == null)
        {
            _logger.LogWarning("Student {StudentId} not found for enrollment email", domainEvent.StudentId);
            return;
        }

        // Send enrollment confirmation for THIS program
        await _emailService.SendProgramEnrollmentEmailAsync(
            student.Email,
            student.Name,
            domainEvent.ProgramName,
            domainEvent.InitialRank
        );

        _logger.LogInformation(
            "Enrollment email sent to {StudentName} ({Email}) for {ProgramName}",
            student.Name,
            student.Email,
            domainEvent.ProgramName
        );
    }
}