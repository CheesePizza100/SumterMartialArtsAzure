using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;

public static class GetProgramsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/programs",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetProgramsQuery());

                    return Results.Ok(result);
                })
            .WithName("GetPrograms")
            .WithTags("Programs");
    }
}