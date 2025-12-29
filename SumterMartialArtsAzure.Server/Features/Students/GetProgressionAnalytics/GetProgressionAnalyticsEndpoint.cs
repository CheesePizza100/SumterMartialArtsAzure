using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetProgressionAnalytics;

public static class GetProgressionAnalyticsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/analytics/progression",
                async (IMediator mediator, int? programId = null) =>
                {
                    var result = await mediator.Send(new GetProgressionAnalyticsQuery(programId));
                    return Results.Ok(result);
                })
            .WithName("GetProgressionAnalytics")
            .WithTags("Students - Event Sourcing");
    }
}
