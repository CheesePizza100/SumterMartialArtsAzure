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

    public StudentEnrolledInProgramEventHandler(IEmailService emailService, AppDbContext dbContext)
    {
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentEnrolledInProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        // Send enrollment confirmation for THIS program
        await _emailService.SendProgramEnrollmentEmailAsync(
            student.Email,
            student.Name,
            domainEvent.ProgramName,
            domainEvent.InitialRank
        );
    }
}