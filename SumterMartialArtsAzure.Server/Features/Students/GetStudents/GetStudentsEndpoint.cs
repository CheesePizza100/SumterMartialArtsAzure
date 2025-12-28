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

                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetStudents")
            .WithTags("Students");
    }
}
