using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons.Filters;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;

public class GetPrivateLessonsHandler 
    : IRequestHandler<GetPrivateLessonsQuery, List<GetPrivateLessonsResponse>>
{
    private readonly IEnumerable<IPrivateLessonFilter> _filters;
    private readonly AppDbContext _db;

    public GetPrivateLessonsHandler(IEnumerable<IPrivateLessonFilter> filters, AppDbContext db)
    {
        _filters = filters;
        _db = db;
    }

    public async Task<List<GetPrivateLessonsResponse>> Handle(GetPrivateLessonsQuery request, CancellationToken cancellationToken)
    {
        var query = _db.PrivateLessonRequests
            .Include(r => r.Instructor)
            .AsQueryable();

        var filter = _filters.FirstOrDefault(x => x.FilterName == request.Filter, new PendingLessonsFilter());

        query = filter.Apply(query);

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