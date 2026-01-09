using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Logout;

public static class LogoutEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("logout",
                async (IMediator mediator) =>
                {
                    var command = new LogoutCommand();
                    await mediator.Send(command);
                    return Results.Ok(new { message = "Logged out successfully" });
                })
            .RequireAuthorization()
            .WithName("Logout")
            .WithTags("Authorization");
    }
}