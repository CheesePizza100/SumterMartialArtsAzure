using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;

public static class GetMyProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/me",
                async (IMediator mediator) =>
                {
                    var query = new GetMyProfileQuery();
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetMyProfile")
            .WithTags("Students");
    }
}