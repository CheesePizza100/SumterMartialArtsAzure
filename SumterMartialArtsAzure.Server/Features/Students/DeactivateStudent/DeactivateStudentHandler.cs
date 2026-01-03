using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.DeactivateStudent;

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
            return new DeactivateStudentCommandResponse(false, "Student not found", null);

        student.Deactivate();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DeactivateStudentCommandResponse(
            true,
            $"Student {student.Name} has been deactivated",
            student.Id
        );
    }
}