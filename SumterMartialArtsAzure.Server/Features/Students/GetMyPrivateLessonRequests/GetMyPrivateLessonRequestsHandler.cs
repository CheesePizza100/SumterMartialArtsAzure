using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;

public class GetMyPrivateLessonRequestsHandler
    : IRequestHandler<GetMyPrivateLessonRequestsQuery, List<PrivateLessonRequestResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetMyPrivateLessonRequestsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<PrivateLessonRequestResponse>> Handle(
        GetMyPrivateLessonRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var studentEmail = await _dbContext.Students
            .Where(s => s.Id == request.StudentId)
            .Select(s => s.Email)
            .FirstOrDefaultAsync(cancellationToken);

        if (studentEmail == null)
        {
            return new List<PrivateLessonRequestResponse>();
        }

        var privateLessonRequests = await _dbContext.PrivateLessonRequests
            .AsNoTracking()
            .Where(plr => plr.StudentEmail == studentEmail)
            .OrderByDescending(plr => plr.CreatedAt)
            .Select(plr => new PrivateLessonRequestResponse(
                plr.Id,
                plr.InstructorId,
                plr.Instructor.Name,
                plr.RequestedLessonTime.Start,
                plr.RequestedLessonTime.End,
                plr.Status.Name,
                plr.Notes,
                plr.RejectionReason,
                plr.CreatedAt
            )).ToListAsync(cancellationToken);

        return privateLessonRequests ?? new List<PrivateLessonRequestResponse>();
    }
}