using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.ChangePassword;

public static class ChangePasswordEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("change-password",
                async ([FromBody] ChangePasswordRequest request, IMediator mediator) =>
                {
                    var command = new ChangePasswordCommand(
                        request.CurrentPassword,
                        request.NewPassword
                    );
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("ChangePassword")
            .WithTags("Auth");
    }
}