using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.GetEmailTemplateById;

public static class GetEmailTemplateByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id}",
                async (int id, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetEmailTemplateByIdQuery(id));
                    return result != null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .WithName("GetEmailTemplateById")
            .WithTags("Admin");
    }
}