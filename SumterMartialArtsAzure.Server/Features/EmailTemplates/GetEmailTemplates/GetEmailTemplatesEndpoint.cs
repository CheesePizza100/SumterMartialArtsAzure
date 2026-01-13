using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplates;

public static class GetEmailTemplatesEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("",
                async (IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetEmailTemplatesQuery());
                    return Results.Ok(result);
                })
            .WithName("GetEmailTemplates")
            .WithTags("Admin");
    }
}