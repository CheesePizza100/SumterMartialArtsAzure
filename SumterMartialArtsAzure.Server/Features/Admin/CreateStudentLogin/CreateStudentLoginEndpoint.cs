using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.CreateStudentLogin;

public static class CreateStudentLoginEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("{studentId}/create-login",
                async (int studentId, [FromBody] CreateStudentLoginRequest request, IMediator mediator) =>
                {
                    var command = new CreateStudentLoginCommand(
                        studentId,
                        request.Username,
                        request.Password
                    );
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .WithName("CreateStudentLogin")
            .WithTags("Admin", "Students");
    }
}