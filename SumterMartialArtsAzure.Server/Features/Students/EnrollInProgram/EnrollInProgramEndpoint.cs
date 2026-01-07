using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EnrollInProgram;

public static class EnrollInProgramEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("{id}/enroll",
                async (int id, EnrollInProgramRequest request, IMediator mediator) =>
                {
                    var command = new EnrollInProgramCommand(
                        id,
                        request.ProgramId,
                        request.ProgramName,
                        request.InitialRank
                    );

                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.BadRequest(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("EnrollInProgram")
            .WithTags("Students");
    }
}