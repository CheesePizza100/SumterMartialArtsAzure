using MediatR;
using System.Security.Claims;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;

public static class GetMyPrivateLessonRequestsEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("me/private-lessons",
                async (IMediator mediator, ClaimsPrincipal user) =>
                {
                    var studentIdClaim = user.FindFirst("StudentId")?.Value;
                    if (studentIdClaim == null)
                    {
                        return Results.BadRequest("Student ID not found");
                    }

                    var studentId = int.Parse(studentIdClaim);
                    var result = await mediator.Send(new GetMyPrivateLessonRequestsQuery(studentId));
                    return Results.Ok(result);
                })
            .RequireAuthorization("StudentOnly")
            .WithName("GetMyPrivateLessonRequests")
            .WithTags("Students");
    }
}