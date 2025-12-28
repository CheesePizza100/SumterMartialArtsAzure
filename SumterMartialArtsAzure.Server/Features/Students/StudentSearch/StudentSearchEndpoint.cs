using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;

public class StudentSearchEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/admin/students/search",
                async (string q, IMediator mediator) =>
                {
                    var result = await mediator.Send(new StudentSearchQuery(q));
                    return Results.Ok(result);
                })
            .WithName("StudentSearch")
            .WithTags("Students");
    }
}