using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public class GetInstructorsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("",
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