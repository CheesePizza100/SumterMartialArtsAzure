using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.DeactivateStudent;

public static class DeactivateStudentEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/students/{id}",
                async (int id, IMediator mediator) =>
                {
                    var command = new DeactivateStudentCommand(id);
                    var result = await mediator.Send(command);

                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.BadRequest(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("DeactivateStudent")
            .WithTags("Admin");
    }
}