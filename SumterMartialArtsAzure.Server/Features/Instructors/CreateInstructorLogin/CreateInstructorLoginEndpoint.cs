using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.CreateInstructorLogin;

public static class CreateInstructorLoginEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("{instructorId}/create-login",
                async (int instructorId, [FromBody] CreateInstructorLoginRequest request, IMediator mediator) =>
                {
                    var command = new CreateInstructorLoginCommand(
                        instructorId,
                        request.Username,
                        request.Password
                    );
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("CreateInstructorLogin")
            .WithTags("Instructors");
    }
}
