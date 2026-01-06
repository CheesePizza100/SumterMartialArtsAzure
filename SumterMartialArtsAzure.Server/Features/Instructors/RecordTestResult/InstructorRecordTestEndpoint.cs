using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordTestResult;

public static class InstructorRecordTestEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/instructors/me/students/{studentId}/test-results",
                async (int studentId, [FromBody] InstructorRecordTestRequest request, IMediator mediator) =>
                {
                    var command = new InstructorRecordTestCommand(
                        studentId,
                        request.ProgramId,
                        request.ProgramName,
                        request.Rank,
                        request.Result,
                        request.Notes,
                        request.TestDate
                    );
                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.NotFound(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("InstructorRecordTest")
            .WithTags("Instructors");
    }
}
