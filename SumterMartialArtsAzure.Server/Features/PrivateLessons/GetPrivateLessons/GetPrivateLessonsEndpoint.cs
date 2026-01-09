using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;

public static class GetPrivateLessonsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("", async (
                [FromQuery] string filter,
                IMediator mediator,
                CancellationToken cancellationToken) =>
            {
                var query = new GetPrivateLessonsQuery(filter ?? "Pending");
                var response = await mediator.Send(query, cancellationToken);
                return Results.Ok(response);
            })
            .WithName("GetPrivateLessons")
            .WithTags("PrivateLessons");
    }
}