using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;

public class UpdatePrivateLessonRequestStatusHandler
    : IRequestHandler<UpdatePrivateLessonStatusCommand, UpdatePrivateLessonStatusResponse>
{
    private readonly AppDbContext _db;

    public UpdatePrivateLessonRequestStatusHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<UpdatePrivateLessonStatusResponse> Handle(UpdatePrivateLessonStatusCommand command, CancellationToken cancellationToken)
    {
        var lessonRequest = await _db.PrivateLessonRequests
            .Include(r => r.Instructor)
            .ThenInclude(i => i.PrivateLessonRequests)
            .FirstOrDefaultAsync(r => r.Id == command.Id, cancellationToken);

        if (lessonRequest == null)
            return new UpdatePrivateLessonStatusResponse(false, "Request not found");

        var newStatus = RequestStatus.FromName(command.Status);

        // If approving, check for conflicts
        if (newStatus.IsApproved)
        {
            if (!lessonRequest.Instructor.IsAvailableForUpdate(lessonRequest.RequestedLessonTime, lessonRequest.Id))
            {
                return new UpdatePrivateLessonStatusResponse(
                    false,
                    "This time slot is no longer available");
            }

            lessonRequest.Approve();
        }
        else if (newStatus.IsRejected)
        {
            if (string.IsNullOrWhiteSpace(command.RejectionReason))
            {
                return new UpdatePrivateLessonStatusResponse(
                    false,
                    "Rejection reason is required");
            }

            lessonRequest.Reject(command.RejectionReason);
        }

        await _db.SaveChangesAsync(cancellationToken);

        return new UpdatePrivateLessonStatusResponse(
                true,
                $"Request has been {newStatus.Name.ToLower()}"
        );
    }
}