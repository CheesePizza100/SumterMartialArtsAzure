using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public class GetInstructorsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/instructors",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetInstructorsQuery());

                    return Results.Ok(result);
                })
            .WithName("GetInstructors")
            .WithTags("Instructors");
    }
}