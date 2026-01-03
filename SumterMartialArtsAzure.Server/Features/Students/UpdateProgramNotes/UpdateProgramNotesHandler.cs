using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;

public class UpdateProgramNotesHandler
    : IRequestHandler<UpdateProgramNotesCommand, UpdateProgramNotesResponse>
{
    private readonly AppDbContext _dbContext;

    public UpdateProgramNotesHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateProgramNotesResponse> Handle(UpdateProgramNotesCommand request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new UpdateProgramNotesResponse(false, "Student not found", null);

        var enrollment = student.UpdateProgramNotes(
            programId: request.ProgramId,
            notes: request.Notes
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new UpdateProgramNotesResponse(
            true,
            "Program notes updated successfully",
            enrollment.Id
        );
    }
}