using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordAttendance;

public static class InstructorRecordAttendanceEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/instructors/me/students/{studentId}/attendance",
                async (int studentId, [FromBody] InstructorRecordAttendanceRequest request, IMediator mediator) =>
                {
                    var command = new InstructorRecordAttendanceCommand(
                        studentId,
                        request.ProgramId,
                        request.ClassesAttended
                    );
                    var result = await mediator.Send(command);
                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.NotFound(new { success = false, message = result.Message });
                })
            .RequireAuthorization()
            .WithName("InstructorRecordAttendance")
            .WithTags("Instructors");
    }
}
