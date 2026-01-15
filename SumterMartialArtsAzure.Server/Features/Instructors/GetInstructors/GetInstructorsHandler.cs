using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public class GetInstructorsHandler
    : IRequestHandler<GetInstructorsQuery, List<GetInstructorsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetInstructorsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetInstructorsResponse>> Handle(GetInstructorsQuery request, CancellationToken cancellationToken)
    {
        // The issue is that AvailabilityRules is not a navigation property(entity relationship)
        // it's a JSON-serialized collection stored as a string. You can't use .Include() on it.
        // AvailabilityRules are automatically loaded because they're stored as JSON in the same
        // table row - no need to .Include() them. You only use .Include() for actual entity relationships (like Programs)
        //var instructors = await _db.Instructors.Select(x => new GetInstructorsResponse(x)).ToListAsync();
        var instructorIdsWithLogins = await _dbContext.Users
            .AsNoTracking()
            .Where(u => u.InstructorId.HasValue)
            .Select(u => u.InstructorId.Value)
            .ToListAsync(cancellationToken);

        var hasLoginLookup = instructorIdsWithLogins.ToHashSet();

        var instructors = await _dbContext.Instructors
            .AsNoTracking()
            .Select(i => new
            {
                i.Id,
                i.Name,
                i.Email,
                i.Rank,
                i.Bio,
                i.PhotoUrl,
                ProgramIds = i.Programs.Select(p => p.Id).ToList()
            }).ToListAsync(cancellationToken);

        return instructors.Select(i => new GetInstructorsResponse(
            i.Id,
            i.Name,
            i.Email,
            i.Rank,
            i.Bio,
            i.PhotoUrl,
            hasLoginLookup.Contains(i.Id),
            i.ProgramIds
        )).ToList();
    }
}