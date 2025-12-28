using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public class GetStudentsHandler
    : IRequestHandler<GetStudentsQuery, List<GetStudentsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetStudentsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetStudentsResponse>> Handle(
        GetStudentsQuery request,
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

        return null;
    }
}