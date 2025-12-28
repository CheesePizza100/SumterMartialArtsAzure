using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public class GetStudentsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetStudentsQuery());
                    return Results.Ok(result);
                })
            .WithName("GetStudents")
            .WithTags("Students");
    }
}
