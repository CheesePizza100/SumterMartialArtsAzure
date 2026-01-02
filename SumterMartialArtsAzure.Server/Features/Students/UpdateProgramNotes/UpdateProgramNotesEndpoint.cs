using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;

public static class UpdateProgramNotesEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/students/{id}/programs/{programId}/notes",
                async (int id, int programId, [FromBody] UpdateProgramNotesRequest request, IMediator mediator) =>
                {
                    var command = new UpdateProgramNotesCommand(
                        id,
                        programId,
                        request.Notes
                    );
                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.NotFound(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("UpdateProgramNotes")
            .WithTags("Students");
    }
}