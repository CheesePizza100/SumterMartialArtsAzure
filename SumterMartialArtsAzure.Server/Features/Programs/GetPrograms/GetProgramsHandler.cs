using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;

public class GetProgramsHandler
    : IRequestHandler<GetProgramsQuery, List<GetProgramsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetProgramsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetProgramsResponse>> Handle(
        GetProgramsQuery request,
        CancellationToken cancellationToken)
    {
        var programs = await _dbContext.Programs
            .Include(p => p.Instructors)
            .Select(p => new GetProgramsResponse(
                p.Id,
                p.Name,
                p.Description,
                p.AgeGroup,
                p.ImageUrl,
                p.Duration,
                p.Schedule,
                p.Instructors.Select(i => i.Id).ToList()
            )).ToListAsync(cancellationToken: cancellationToken);

        return programs;
    }
}