using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;

public class StudentSearchHandler
    : IRequestHandler<StudentSearchQuery, List<GetStudentSearchResponse>>
{
    private readonly AppDbContext _dbContext;

    public StudentSearchHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetStudentSearchResponse>> Handle(StudentSearchQuery request, CancellationToken cancellationToken)
    {
        var searchTerm = request.SearchTerm.ToLower();

        var students = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .Where(s =>
                s.Name.ToLower().Contains(searchTerm) ||
                s.Email.ToLower().Contains(searchTerm) ||
                s.ProgramEnrollments.Any(e => e.ProgramName.ToLower().Contains(searchTerm))
            )
            .Select(s => new GetStudentSearchResponse(
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.ProgramEnrollments
                    .Where(e => e.IsActive)
                    .Select(e => new ProgramEnrollmentDto(
                        e.ProgramName,
                        e.CurrentRank,
                        e.EnrolledDate,
                        e.LastTestDate,
                        e.InstructorNotes
                    )).ToList(),
                new AttendanceDto(
                    s.Attendance.Last30Days,
                    s.Attendance.Total,
                    s.Attendance.AttendanceRate
                ),
                s.TestHistory
                    .OrderByDescending(t => t.TestDate)
                    .Select(t => new TestHistoryDto(
                        t.TestDate,
                        t.ProgramName,
                        t.RankAchieved,
                        t.Result,
                        t.Notes
                    )).ToList()
            ))
            .ToListAsync(cancellationToken);

        return students;
    }
}