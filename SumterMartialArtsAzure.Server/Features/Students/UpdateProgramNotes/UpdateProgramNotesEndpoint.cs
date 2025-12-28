using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;

public static class UpdateProgramNotesEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/admin/students/{id}/programs/{programId}/notes",
                async (int id, int programId, UpdateProgramNotesRequest request, IMediator mediator) =>
                {
                    var command = new UpdateProgramNotesCommand(
                        id,
                        programId,
                        request.Notes
                    );

                    var result = await mediator.Send(command);
                    return result
                        ? Results.Ok(new { success = true, message = "Notes updated successfully" })
                        : Results.NotFound(new { success = false, message = "Student or program enrollment not found" });
                })
            .WithName("UpdateProgramNotes")
            .WithTags("Students");
    }
}