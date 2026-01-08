using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.StudentSearch;

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
            .AsNoTracking() // No need to track for read-only
            .Where(s =>
                EF.Functions.Like(s.Name.ToLower(), $"%{searchTerm}%") ||
                EF.Functions.Like(s.Email.ToLower(), $"%{searchTerm}%") ||
                s.ProgramEnrollments.Any(e => EF.Functions.Like(e.ProgramName.ToLower(), $"%{searchTerm}%"))
            )
            .Take(50) // Limit results - add pagination later
            .Select(s => new GetStudentSearchResponse(
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.ProgramEnrollments
                    .Where(e => e.IsActive)
                    .Select(e => new ProgramEnrollmentDto(
                        e.ProgramId,
                        e.ProgramName,
                        e.CurrentRank,
                        e.EnrolledDate,
                        e.LastTestDate,
                        e.InstructorNotes,
                        new AttendanceDto(
                            e.Attendance.Last30Days,
                            e.Attendance.Total,
                            e.Attendance.AttendanceRate)
                    )).ToList(),
                s.TestHistory
                    .OrderByDescending(t => t.TestDate)
                    .Take(5) // Only get last 5 tests per student
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