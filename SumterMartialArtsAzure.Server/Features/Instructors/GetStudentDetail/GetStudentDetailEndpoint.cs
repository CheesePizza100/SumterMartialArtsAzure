using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetStudentDetail;

public static class GetStudentDetailEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("me/students/{studentId}",
                async (int studentId, IMediator mediator) =>
                {
                    var query = new GetStudentDetailQuery(studentId);
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetStudentDetail")
            .WithTags("Instructors");
    }
}