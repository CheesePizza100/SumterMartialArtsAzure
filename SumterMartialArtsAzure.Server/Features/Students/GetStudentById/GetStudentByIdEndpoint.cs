using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

public static class GetStudentByIdEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/students/{id}",
                async (int id, IMediator mediator) =>
                {
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