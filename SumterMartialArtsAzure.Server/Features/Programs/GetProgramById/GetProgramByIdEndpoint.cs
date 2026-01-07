using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;

public class GetProgramByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id:int}",
                async (int id, IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetProgramByIdQuery(id));

                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetProgramById")
            .WithTags("Programs");
    }
}