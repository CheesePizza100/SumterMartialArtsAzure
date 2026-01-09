using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;

public class GetInstructorByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id:int}",
                async (int id, IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetInstructorByIdQuery(id));

                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetInstructorById")
            .WithTags("Instructors");
    }
}