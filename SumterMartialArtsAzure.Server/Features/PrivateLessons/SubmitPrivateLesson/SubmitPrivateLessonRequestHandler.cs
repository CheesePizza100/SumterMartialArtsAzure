using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;

public class SubmitPrivateLessonRequestHandler
    : IRequestHandler<SubmitPrivateLessonCommand, SubmitPrivateLessonResponse>
{
    private readonly AppDbContext _db;
    // TODO: Add IEmailSender when ready

    public SubmitPrivateLessonRequestHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<SubmitPrivateLessonResponse> Handle(SubmitPrivateLessonCommand command, CancellationToken cancellationToken)
    {
        // Create lesson time value object
        var lessonTime = new LessonTime(command.RequestedStart, command.RequestedEnd);

        // Load instructor with availability rules
        var instructor = await _db.Instructors
            .Include(i => i.PrivateLessonRequests)
            .FirstOrDefaultAsync(i => i.Id == command.InstructorId, cancellationToken);

        if (instructor == null)
            throw new InvalidOperationException($"Instructor {command.InstructorId} not found");

        // Check availability
        if (!instructor.IsAvailable(lessonTime))
            throw new InvalidOperationException("Requested time slot is not available");

        // Create command using domain factory
        var privateLessonRequest = PrivateLessonRequest.Create(
            command.InstructorId,
            command.StudentName,
            command.StudentEmail,
            command.StudentPhone,
            lessonTime,
            command.Notes
        );

        _db.PrivateLessonRequests.Add(privateLessonRequest);
        await _db.SaveChangesAsync(cancellationToken);

        // TODO: Send emails
        // await _emailService.SendAsync("admin@...", "New Request", ...);
        // await _emailService.SendAsync(command.StudentEmail, "Confirmation", ...);

        return new SubmitPrivateLessonResponse(
            privateLessonRequest.Id,
            privateLessonRequest.Status.Name,
            "Your private lesson command has been submitted successfully!"
        );
    }
}