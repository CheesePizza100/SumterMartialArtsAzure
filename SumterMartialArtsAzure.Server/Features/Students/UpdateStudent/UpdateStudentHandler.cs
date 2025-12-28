using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public class UpdateStudentHandler
    : IRequestHandler<UpdateStudentCommand, UpdateStudentCommandResponse>
{
    private readonly AppDbContext _dbContext;

    public UpdateStudentHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<UpdateStudentCommandResponse> Handle(
        UpdateStudentCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .FirstOrDefaultAsync(s => s.Id == request.Id, cancellationToken);

        if (student == null)
            return null;

        // Use aggregate method to maintain business rules
        student.UpdateContactInfo(
            name: request.Name,
            email: request.Email,
            phone: request.Phone
        );

        await _dbContext.SaveChangesAsync(cancellationToken);

        // Return updated student
        return new UpdateStudentCommandResponse(
            student.Id,
            student.Name,
            student.Email,
            student.Phone,
            student.ProgramEnrollments
                .Where(e => e.IsActive)
                .Select(e => new ProgramEnrollmentDto(
                    e.ProgramName,
                    e.CurrentRank,
                    e.EnrolledDate,
                    e.LastTestDate,
                    e.InstructorNotes
                )).ToList(),
            new AttendanceDto(
                student.Attendance.Last30Days,
                student.Attendance.Total,
                student.Attendance.AttendanceRate
            ),
            student.TestHistory
                .OrderByDescending(t => t.TestDate)
                .Select(t => new TestHistoryDto(
                    t.TestDate,
                    t.ProgramName,
                    t.RankAchieved,
                    t.Result,
                    t.Notes
                )).ToList()
        );
    }
}