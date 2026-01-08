using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.StudentSearch;

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
            .WithName("StudentSearch")
            .WithTags("Admin");
    }
}