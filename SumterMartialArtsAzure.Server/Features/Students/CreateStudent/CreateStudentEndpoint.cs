using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudent;

public static class CreateStudentEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/students",
                async (CreateStudentRequest request, IMediator mediator) =>
                {
                    var command = new CreateStudentCommand(
                        request.Name,
                        request.Email,
                        request.Phone
                    );

                    var result = await mediator.Send(command);
                    return Results.Created($"/api/students/{result.Id}", result);
                })
            .RequireAuthorization()
            .WithName("CreateStudent")
            .WithTags("Students");
    }
}