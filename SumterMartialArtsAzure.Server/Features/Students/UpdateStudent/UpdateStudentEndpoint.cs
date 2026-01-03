using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public static class UpdateStudentEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut("/api/students/{id}",
                async (int id, UpdateStudentRequest request, IMediator mediator) =>
                {
                    var command = new UpdateStudentCommand(
                        id,
                        request.Name,
                        request.Email,
                        request.Phone
                    );

                    var result = await mediator.Send(command);
                    return result != null 
                        ? Results.Ok(result) 
                        : Results.NotFound();
                })
            .RequireAuthorization()
            .WithName("UpdateStudent")
            .WithTags("Students");
    }
}