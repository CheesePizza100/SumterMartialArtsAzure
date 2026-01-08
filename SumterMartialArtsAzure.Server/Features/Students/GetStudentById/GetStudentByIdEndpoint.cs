using MediatR;
using System.Security.Claims;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

public static class GetStudentByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("{id}",
                async (int id, IMediator mediator, ClaimsPrincipal user) =>
                {
                    if (!user.IsInRole("Admin"))
                    {
                        var userStudentIdClaim = user.FindFirst("StudentId")?.Value;

                        if (userStudentIdClaim == null || int.Parse(userStudentIdClaim) != id)
                        {
                            return Results.Forbid();
                        }
                    }

                    var result = await mediator.Send(new GetStudentByIdQuery(id));
                    return result is not null
                        ? Results.Ok(result)
                        : Results.NotFound();
                })
            .RequireAuthorization()
            .WithName("GetStudentById")
            .WithTags("Students");
    }
}