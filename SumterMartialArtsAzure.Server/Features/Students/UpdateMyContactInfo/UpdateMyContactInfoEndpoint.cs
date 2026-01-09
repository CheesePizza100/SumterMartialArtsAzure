using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

public static class UpdateMyContactInfoEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("me",
                async ([FromBody] UpdateMyContactInfoRequest request, IMediator mediator) =>
                {
                    var command = new UpdateMyContactInfoCommand(
                        request.Name,
                        request.Email,
                        request.Phone
                    );
                    var result = await mediator.Send(command);
                    return Results.Ok(result);
                })
            .WithName("UpdateMyContactInfo")
            .WithTags("Students");
    }
}
