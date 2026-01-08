using System.Security.Claims;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentById;

public record GetStudentByIdQuery(int Id)
    : IRequest<GetStudentByIdResponse>;
public record GetStudentByIdResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    List<TestHistoryDto> TestHistory
) : IAuditableResponse
{
    public string EntityId => Id.ToString();

    public object GetAuditDetails() => new
    {
        Name,
        Email,
        Phone,
        Programs,
        TestHistory
    };
}
public record ProgramEnrollmentDto(
    int ProgramId,
    string Name,
    string Rank,
    DateTime EnrolledDate,
    DateTime? LastTest,
    string? TestNotes,
    AttendanceDto Attendance
);
public record TestHistoryDto(
    DateTime Date,
    string Program,
    string Rank,
    string Result,
    string Notes
);
public record AttendanceDto(
    int Last30Days,
    int Total,
    int AttendanceRate
);
public class GetStudentByIdHandler
    : IRequestHandler<GetStudentByIdQuery, GetStudentByIdResponse>
{
    private readonly AppDbContext _dbContext;

    public GetStudentByIdHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetStudentByIdResponse> Handle(
        GetStudentByIdQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .AsNoTracking()
            .AsSplitQuery()
            .Where(s => s.Id == request.Id)
            .Select(s => new GetStudentByIdResponse(
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.ProgramEnrollments
                    .Where(e => e.IsActive)
                    .Select(e => new ProgramEnrollmentDto(
                        e.ProgramId,
                        e.ProgramName,
                        e.CurrentRank,
                        e.EnrolledDate,
                        e.LastTestDate,
                        e.InstructorNotes,
                        new AttendanceDto(
                            e.Attendance.Last30Days,
                            e.Attendance.Total,
                            e.Attendance.AttendanceRate
                        )
                    ))
                    .ToList(),
                s.TestHistory
                    .OrderByDescending(t => t.TestDate)
                    .Select(t => new TestHistoryDto(
                        t.TestDate,
                        t.ProgramName,
                        t.RankAchieved,
                        t.Result,
                        t.Notes
                    ))
                    .ToList()
            )).FirstOrDefaultAsync(cancellationToken);

        return student;
    }
}

public class GetStudentByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id}",
                async (int id, IMediator mediator, ClaimsPrincipal user) =>
                {
                    if (!user.IsInRole("Admin"))
                    {
                        var userStudentIdClaim = user.FindFirst("StudentId")?.Value;

                        if (userStudentIdClaim == null || int.Parse(userStudentIdClaim) != id)
                        {
                            return Results.Forbid();
                        }
                    }

                    var result = await mediator.Send(new GetStudentByIdQuery(id));
                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetStudentById")
            .WithTags("Admin", "Students");
    }
}