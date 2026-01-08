using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.AddTestResult;

public static class AddTestResultEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("{id}/test-results",
                async (int id, AddTestResultRequest request, IMediator mediator) =>
                {
                    var command = new AddTestResultCommand(
                        id,
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
            .WithName("AddTestResult")
            .WithTags("Admin", "Students");
    }
}