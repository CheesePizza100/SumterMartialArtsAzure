using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EventHandlers;

public class StudentCreatedEventHandler
    : INotificationHandler<DomainEventNotification<StudentCreated>>
{
    private readonly IEmailService _emailService;
    private readonly AppDbContext _dbContext;
    private readonly ILogger<StudentCreatedEventHandler> _logger;

    public StudentCreatedEventHandler(
        IEmailService emailService,
        AppDbContext dbContext,
        ILogger<StudentCreatedEventHandler> logger)
    {
        _logger = logger;
        _emailService = emailService;
        _dbContext = dbContext;
    }

    public async Task Handle(DomainEventNotification<StudentCreated> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "New student created. StudentId: {StudentId}, Student: {StudentName}",
            domainEvent.StudentId,
            domainEvent.Name);


        await _emailService.SendSchoolWelcomeEmailAsync(
            domainEvent.Email,
            domainEvent.Name
        );

        _logger.LogInformation(
            "School welcome email sent to new student {StudentName} ({Email})",
            domainEvent.Email,
            domainEvent.Name
        );
    }
}

public class StudentTestRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentTestRecorded>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentTestRecordedEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentTestRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student test recorded. StudentId: {StudentId}, Student: {StudentName}, ProgramId: {ProgramId}, Program: {ProgramName}",
            domainEvent.StudentId,
            domainEvent.StudentName,
            domainEvent.ProgramId,
            domainEvent.ProgramName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}

public class StudentAttendanceRecordedEventHandler
    : INotificationHandler<DomainEventNotification<StudentAttendanceRecorded>>
{
    private readonly ILogger<StudentEnrolledInProgramEventHandler> _logger;

    public StudentAttendanceRecordedEventHandler(ILogger<StudentEnrolledInProgramEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(DomainEventNotification<StudentAttendanceRecorded> notification, CancellationToken cancellationToken)
    {
        var domainEvent = notification.DomainEvent;

        _logger.LogInformation(
            "Student attendance recorded. StudentId: {StudentId}, Student: {StudentName}",
            domainEvent.StudentId,
            domainEvent.StudentName);

        // TODO: Send email notification to admin
        // TODO: Send confirmation email to student

        return Task.CompletedTask;
    }
}