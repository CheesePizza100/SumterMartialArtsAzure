using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudents;

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
        //var students = await _dbContext.Students
        //    .AsNoTracking()
        //    .AsSplitQuery()
        //    .Select(s => new GetStudentsResponse(
        //        s.Id,
        //        s.Name,
        //        s.Email,
        //        s.Phone,
        //        _dbContext.Users.Any(u => u.StudentId == s.Id),
        //        s.ProgramEnrollments
        //            .Where(e => e.IsActive)
        //            .Select(e => new ProgramEnrollmentDto(
        //                e.ProgramId,
        //                e.ProgramName,
        //                e.CurrentRank,
        //                e.EnrolledDate,
        //                e.LastTestDate,
        //                e.InstructorNotes,
        //                new AttendanceDto(
        //                    e.Attendance.Last30Days,
        //                    e.Attendance.Total,
        //                    e.Attendance.AttendanceRate
        //                )
        //            ))
        //            .ToList(),
        //        s.TestHistory
        //            .OrderByDescending(t => t.TestDate)
        //            .Select(t => new TestHistoryDto(
        //                t.TestDate,
        //                t.ProgramName,
        //                t.RankAchieved,
        //                t.Result,
        //                t.Notes
        //            ))
        //            .ToList()
        //    )).ToListAsync(cancellationToken);

        //return students;

        // First, get all student IDs that have user accounts (single query)
        var studentIdsWithLogins = await _dbContext.Users
            .Where(u => u.StudentId.HasValue)
            .Select(u => u.StudentId.Value)
            .ToListAsync(cancellationToken);

        // Convert to HashSet for O(1) lookup
        var hasLoginLookup = studentIdsWithLogins.ToHashSet();

        var students = await _dbContext.Students
            .AsNoTracking()
            .AsSplitQuery()
            .Select(s => new
            {
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                Programs = s.ProgramEnrollments
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
                            e.Attendance.AttendanceRate
                        )
                    ))
                    .ToList(),
                TestHistory = s.TestHistory
                    .OrderByDescending(t => t.TestDate)
                    .Select(t => new TestHistoryDto(
                        t.TestDate,
                        t.ProgramName,
                        t.RankAchieved,
                        t.Result,
                        t.Notes
                    ))
                    .ToList()
            }).ToListAsync(cancellationToken);

        // Map to response with hasLogin (in-memory operation)
        return students.Select(s => new GetStudentsResponse(
            s.Id,
            s.Name,
            s.Email,
            s.Phone,
            hasLoginLookup.Contains(s.Id),  // O(1) lookup
            s.Programs,
            s.TestHistory
        )).ToList();
    }
}