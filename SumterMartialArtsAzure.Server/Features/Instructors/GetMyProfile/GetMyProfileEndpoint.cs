using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;

public static class GetInstructorProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("me",
                async (IMediator mediator) =>
                {
                    var query = new GetInstructorProfileQuery();
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                })
            .WithName("GetInstructorProfile")
            .WithTags("Instructors");
    }
}