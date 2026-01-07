using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;

public class StudentSearchEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("search",
                async (string q, IMediator mediator) =>
                {
                    var result = await mediator.Send(new StudentSearchQuery(q));
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("StudentSearch")
            .WithTags("Students");
    }
}