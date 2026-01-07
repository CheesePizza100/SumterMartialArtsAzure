using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;

public class GetStudentAttendanceEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id}/attendance",
                async (int id, int programId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetStudentAttendanceQuery(id, programId));
                    return result != null 
                        ? Results.Ok(result) 
                        : Results.NotFound();
                })
            .RequireAuthorization()
            .WithName("GetStudentAttendance")
            .WithTags("Students");
    }
}