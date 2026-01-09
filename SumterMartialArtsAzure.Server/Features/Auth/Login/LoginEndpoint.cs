using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Login;

public static class LoginEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("login",
                async ([FromBody] LoginRequest request, IMediator mediator) =>
                {
                    var command = new LoginCommand(request.UserName, request.Password);
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .WithName("Login")
            .WithTags("Authorization");
    }
}