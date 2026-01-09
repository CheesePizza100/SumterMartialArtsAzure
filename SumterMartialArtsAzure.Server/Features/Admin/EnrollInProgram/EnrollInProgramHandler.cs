using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.EnrollInProgram;

public class EnrollInProgramHandler
    : IRequestHandler<EnrollInProgramCommand, EnrollInProgramCommandResponse>
{
    private readonly AppDbContext _dbContext;

    public EnrollInProgramHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<EnrollInProgramCommandResponse> Handle(
        EnrollInProgramCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new EnrollInProgramCommandResponse(false, "Student not found", null);

        var enrollment = student.EnrollInProgram(
            programId: request.ProgramId,
            programName: request.ProgramName,
            initialRank: request.InitialRank
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new EnrollInProgramCommandResponse(
            true,
            $"Student successfully enrolled in {request.ProgramName}",
            enrollment.Id
        );
    }
}