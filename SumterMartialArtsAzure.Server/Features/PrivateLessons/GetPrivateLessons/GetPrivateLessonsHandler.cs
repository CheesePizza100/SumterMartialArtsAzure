using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;

public class GetPrivateLessonsHandler 
    : IRequestHandler<GetPrivateLessonsQuery, List<GetPrivateLessonsResponse>>
{
    private readonly AppDbContext _db;

    public GetPrivateLessonsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<GetPrivateLessonsResponse>> Handle(GetPrivateLessonsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.PrivateLessonRequests
            .Include(r => r.Instructor)
            .AsQueryable();

        query = request.Filter switch
        {
            "Pending" => query.Where(r => r.Status == RequestStatus.Pending),
            "Recent" => query.Where(r => r.CreatedAt >= DateTime.UtcNow.AddDays(-30)),
            "All" => query,
            _ => query.Where(r => r.Status == RequestStatus.Pending) // Default to pending
        };

        var requests = await query
            .OrderByDescending(r => r.CreatedAt)
            .Select(r => new GetPrivateLessonsResponse(
                r.Id,
                r.InstructorId,
                r.Instructor.Name,
                r.StudentName,
                r.StudentEmail,
                r.StudentPhone,
                r.RequestedLessonTime.Start,
                r.RequestedLessonTime.End,
                r.Status.Name,
                r.Notes,
                r.RejectionReason,
                r.CreatedAt
            )).ToListAsync(cancellationToken);

        return requests;
    }
}