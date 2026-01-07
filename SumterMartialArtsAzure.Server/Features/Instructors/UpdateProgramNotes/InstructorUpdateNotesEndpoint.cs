using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.UpdateProgramNotes;

public static class InstructorUpdateNotesEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("me/students/{studentId}/programs/{programId}/notes",
                async (int studentId, int programId, [FromBody] InstructorUpdateNotesRequest request, IMediator mediator) =>
                {
                    var command = new InstructorUpdateNotesCommand(studentId, programId, request.Notes);
                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.NotFound(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("InstructorUpdateNotes")
            .WithTags("Instructors");
    }
}
