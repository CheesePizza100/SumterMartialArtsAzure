using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;

public static class UpdatePrivateLessonRequestStatusEndpoint
{
    public static void MapEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPatch("/api/private-lessons/{id}/status", async (
                int id,
                [FromBody] UpdateStatusRequest request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var command = new UpdatePrivateLessonStatusCommand(id, request.Status, request.RejectionReason);
                var response = await mediator.Send(command, cancellationToken);
                return response.Success
                    ? Results.Ok(response)
                    : Results.BadRequest(response);
            })
            .WithName("UpdatePrivateLessonRequestStatus")
            .WithTags("PrivateLessons");
    }
}