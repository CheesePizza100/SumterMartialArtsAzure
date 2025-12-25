using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;

public class GetInstructorAvailabilityEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/instructors/{id:int}/availability",
                async (int id, IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetInstructorAvailabilityQuery(id));

                    return result is null
                        ? Results.NotFound()
                        : Results.Ok(result);
                })
            .WithName("GetInstructorAvailability")
            .WithTags("Instructors");
    }
}