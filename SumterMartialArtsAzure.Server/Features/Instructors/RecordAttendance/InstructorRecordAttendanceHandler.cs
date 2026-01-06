using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordAttendance;

public class InstructorRecordAttendanceHandler
    : IRequestHandler<InstructorRecordAttendanceCommand, InstructorRecordAttendanceResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public InstructorRecordAttendanceHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<InstructorRecordAttendanceResponse> Handle(
        InstructorRecordAttendanceCommand request,
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
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new InstructorRecordAttendanceResponse(false, "Student not found", null);

        // Record attendance using aggregate method
        student.RecordAttendance(request.ProgramId, request.ClassesAttended);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new InstructorRecordAttendanceResponse(
            true,
            $"Recorded {request.ClassesAttended} class(es) for student",
            student.Id
        );
    }
}