using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;

public class GetStudentAttendanceEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/{id}/attendance",
                async (int id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetStudentAttendanceQuery(id));
                    return result != null 
                        ? Results.Ok(result) 
                        : Results.NotFound();
                })
            .WithName("GetStudentAttendance")
            .WithTags("Students");
    }
}