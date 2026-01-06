using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;

public static class GetMyStudentsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/instructors/me/students",
                async (IMediator mediator) =>
                {
                    var query = new GetMyStudentsQuery();
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetMyStudents")
            .WithTags("Instructors");
    }
}