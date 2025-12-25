using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;

public class GetProgramByIdHandler
    : IRequestHandler<GetProgramByIdQuery, GetProgramByIdResponse?>
{
    private readonly AppDbContext _db;

    public GetProgramByIdHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<GetProgramByIdResponse?> Handle(GetProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var program = await _db.Programs
            .Include(p => p.Instructors)
            .FirstOrDefaultAsync(p => p.Id == request.Id, cancellationToken);

        if (program == null) return null;

        var instructorDtos = program.Instructors
            .Select(i => new InstructorDto(i.Id, i.Name, i.Rank))
            .ToList();

        var result = new GetProgramByIdResponse(
            program.Id,
            program.Name,
            program.Description,
            program.Details,
            program.AgeGroup,
            program.ImageUrl,
            program.Duration,
            program.Schedule,
            instructorDtos
        );
        return result;
    }
}