using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.UpdateProgramNotes;

public class InstructorUpdateNotesHandler
    : IRequestHandler<InstructorUpdateNotesCommand, InstructorUpdateNotesResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public InstructorUpdateNotesHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<InstructorUpdateNotesResponse> Handle(
        InstructorUpdateNotesCommand request,
        CancellationToken cancellationToken)
    {
        var instructorId = _currentUserService.GetInstructorId();

        if (instructorId == null)
            throw new UnauthorizedAccessException("User is not associated with an instructor");

        // Verify instructor teaches this program
        var instructorTeachesProgram = await _dbContext.Instructors
            .Where(i => i.Id == instructorId.Value)
            .AnyAsync(i => Enumerable.Any<Domain.Program>(i.Programs, p => p.Id == request.ProgramId), cancellationToken);

        if (!instructorTeachesProgram)
            throw new UnauthorizedAccessException("You do not teach this program");

        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new InstructorUpdateNotesResponse(false, "Student not found", null);

        var enrollment = student.UpdateProgramNotes(request.ProgramId, request.Notes);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new InstructorUpdateNotesResponse(
            true,
            "Program notes updated successfully",
            enrollment.Id
        );
    }
}