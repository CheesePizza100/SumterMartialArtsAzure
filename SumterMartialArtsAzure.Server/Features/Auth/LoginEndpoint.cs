using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth;

public static class LoginEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {

        app.MapGet("/api/auth/login",
                async (LoginRequest request, IMediator mediator) =>
                {
                    var command = new LoginCommand(request.UserName, request.Email);
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .WithName("Login")
            .WithTags("Authorization");
    }
}