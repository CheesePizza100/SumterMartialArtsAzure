using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.DeactivateStudent;

public static class DeactivateStudentEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("/api/students/{id}",
                async (int id, IMediator mediator) =>
                {
                    var command = new DeactivateStudentCommand(id);
                    var result = await mediator.Send(command);

                    return result.Success
                        ? Results.Ok(new { success = true, message = result.Message })
                        : Results.BadRequest(new { success = false, message = result.Message });
                })
            .WithName("DeactivateStudent")
            .WithTags("Students");
    }
}
public record DeactivateStudentCommand(
    int StudentId
) : IRequest<DeactivateStudentCommandResponse>;

public record DeactivateStudentCommandResponse(
    bool Success,
    string Message
);
public class DeactivateStudentHandler
    : IRequestHandler<DeactivateStudentCommand, DeactivateStudentCommandResponse>
{
    private readonly AppDbContext _dbContext;

    public DeactivateStudentHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DeactivateStudentCommandResponse> Handle(
        DeactivateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new DeactivateStudentCommandResponse(false, "Student not found");

        try
        {
            // Use aggregate business method for soft delete
            student.Deactivate();

            await _dbContext.SaveChangesAsync(cancellationToken);

            return new DeactivateStudentCommandResponse(
                true,
                $"Student {student.Name} has been deactivated"
            );
        }
        catch (InvalidOperationException ex)
        {
            return new DeactivateStudentCommandResponse(false, ex.Message);
        }
    }
}
