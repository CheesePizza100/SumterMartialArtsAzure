using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public class GetStudentsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetStudentsQuery());

                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetStudents")
            .WithTags("Students");
    }
}
public record GetStudentsQuery
    : IRequest<List<GetStudentsResponse>>;
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
public record GetStudentsResponse(
    int Id,
    string Name,
    string Description,
    string AgeGroup,
    string ImageUrl,
    string Duration,
    string Schedule,
    List<int> InstructorIds
);



public record StudentDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Phone { get; init; } = string.Empty;
    public List<ProgramDto> Programs { get; init; } = new();
    public AttendanceDto Attendance { get; init; } = new();
    public List<TestHistoryDto> TestHistory { get; init; } = new();
}

public record ProgramDto
{
    public string Name { get; init; } = string.Empty;
    public string Rank { get; init; } = string.Empty;
    public DateTime EnrolledDate { get; init; }
    public DateTime? LastTest { get; init; }
    public string? TestNotes { get; init; }
}

public record TestHistoryDto
{
    public DateTime Date { get; init; }
    public string Program { get; init; } = string.Empty;
    public string Rank { get; init; } = string.Empty;
    public string Result { get; init; } = string.Empty; // "Pass" or "Fail"
    public string Notes { get; init; } = string.Empty;
}

public record AttendanceDto
{
    public int Last30Days { get; init; }
    public int Total { get; init; }
    public int AttendanceRate { get; init; }
}

public record UpdateStudentRequest
{
    public string? Name { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
}

public record AddTestResultRequest
{
    public string ProgramName { get; init; } = string.Empty;
    public string Rank { get; init; } = string.Empty;
    public string Result { get; init; } = string.Empty; // "Pass" or "Fail"
    public string Notes { get; init; } = string.Empty;
    public DateTime TestDate { get; init; }
}

public record UpdateNotesRequest
{
    public string Notes { get; init; } = string.Empty;
}