using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.EmailTemplates.UpdateEmailTemplate;

public static class UpdateEmailTemplateEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("{id}",
                async (int id, [FromBody] UpdateEmailTemplateRequest request, IMediator mediator) =>
                {
                    var command = new UpdateEmailTemplateCommand(
                        id,
                        request.Name,
                        request.Subject,
                        request.Body,
                        request.Description
                    );
                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.BadRequest(new { success = false, message = result.Message });
                })
            .WithName("UpdateEmailTemplate")
            .WithTags("Admin");
    }
}