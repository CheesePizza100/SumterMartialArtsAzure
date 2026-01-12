using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services.Email;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders;
using SumterMartialArtsAzure.Server.Services.Email.ContentBuilders.Constants;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentEnrolledInProgramEventHandler
    : INotificationHandler<DomainEventNotification<StudentEnrolledInProgram>>
{
    private readonly EmailOrchestrator _emailOrchestrator;
    private readonly AppDbContext _dbContext;

    public StudentEnrolledInProgramEventHandler(EmailOrchestrator emailOrchestrator, AppDbContext dbContext)
    {
        _emailOrchestrator = emailOrchestrator;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentEnrolledInProgram> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        var student = await _dbContext.Students
            .FirstOrDefaultAsync(s => s.Id == domainEvent.StudentId, cancellationToken);

        // Send enrollment confirmation for THIS program
        await _emailOrchestrator.SendAsync(
            student.Email,
            student.Name,
            new SimpleEmailContentBuilder(EmailTemplateKeys.ProgramEnrollment)
                .WithVariable("StudentName", student.Name)
                .WithVariable("ProgramName", domainEvent.ProgramName)
                .WithVariable("InitialRank", domainEvent.InitialRank)
        );
    }
}