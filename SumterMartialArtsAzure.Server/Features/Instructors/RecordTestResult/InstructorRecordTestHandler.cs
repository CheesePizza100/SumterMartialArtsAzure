using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordTestResult;

public class InstructorRecordTestHandler
    : IRequestHandler<InstructorRecordTestCommand, InstructorRecordTestResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public InstructorRecordTestHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<InstructorRecordTestResponse> Handle(
        InstructorRecordTestCommand request,
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

        // Get student
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new InstructorRecordTestResponse(false, "Student not found", null);

        // Record test result using aggregate method
        var testResult = student.RecordTestResult(
            programId: request.ProgramId,
            programName: request.ProgramName,
            rankAchieved: request.Rank,
            passed: request.Result.Equals("Pass", StringComparison.OrdinalIgnoreCase),
            notes: request.Notes,
            testDate: request.TestDate
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new InstructorRecordTestResponse(
            true,
            "Test result recorded successfully",
            testResult.Id
        );
    }
}