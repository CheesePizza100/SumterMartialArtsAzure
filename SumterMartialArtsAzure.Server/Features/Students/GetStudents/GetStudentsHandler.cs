using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public class GetStudentsHandler
    : IRequestHandler<GetStudentsQuery, List<GetStudentsResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetStudentsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetStudentsResponse>> Handle(
        GetStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var students = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .Select(s => new GetStudentsResponse(
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