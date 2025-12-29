using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentEventStream;

public class GetStudentEventStreamEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/{studentId}/programs/{programId}/events",
                async (int studentId, int programId, IMediator mediator) =>
                {
                    var result = await mediator.Send(new GetStudentEventStreamQuery(studentId, programId));
                    return Results.Ok(result);
                })
            .WithName("GetStudentEventStream")
            .WithTags("Students - Event Sourcing");
    }
}