using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public class GetStudentsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetStudentsQuery());
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetStudents")
            .WithTags("Students");
    }
}
