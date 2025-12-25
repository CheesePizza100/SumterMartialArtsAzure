using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;

public static class SubmitPrivateLessonRequestEndpoint
{
    public static void MapEndpoint(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/PrivateLessons", async (
                [FromBody] SubmitPrivateLessonCommand request,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var response = await mediator.Send(request, cancellationToken);
                return Results.Ok(response);
            })
            .WithName("SubmitPrivateLessonCommand")
            .WithTags("PrivateLessons");
    }
}