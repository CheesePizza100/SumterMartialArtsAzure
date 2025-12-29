using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentRankAtDate;

public class GetStudentRankAtDateEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/{studentId}/programs/{programId}/rank-at-date",
                async (int studentId, int programId, DateTime asOfDate, IMediator mediator) =>
                {
                    var result = await mediator.Send(
                        new GetStudentRankAtDateQuery(studentId, programId, asOfDate)
                    );

                    return result != null 
                        ? Results.Ok(result) 
                        : Results.NotFound();
                })
            .WithName("GetStudentRankAtDate")
            .WithTags("Students - Event Sourcing");
    }
}