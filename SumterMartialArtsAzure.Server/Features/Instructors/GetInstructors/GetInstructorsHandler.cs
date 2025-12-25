using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public class GetInstructorsHandler
    : IRequestHandler<GetInstructorsQuery, List<GetInstructorsResponse>>
{
    private readonly AppDbContext _db;

    public GetInstructorsHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<GetInstructorsResponse>> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
    {
        // The issue is that AvailabilityRules is not a navigation property(entity relationship)
        // it's a JSON-serialized collection stored as a string. You can't use .Include() on it.
        // AvailabilityRules are automatically loaded because they're stored as JSON in the same
        // table row - no need to .Include() them. You only use .Include() for actual entity relationships (like Programs)
        var instructors = await _db.Instructors.Select(x => new GetInstructorsResponse(x)).ToListAsync();

        return instructors;
    }
}