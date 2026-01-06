using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;

public class GetMyStudentsHandler
    : IRequestHandler<GetMyStudentsQuery, List<InstructorStudentDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetMyStudentsHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<List<InstructorStudentDto>> Handle(
        GetMyStudentsQuery request,
        CancellationToken cancellationToken)
    {
        var instructorId = _currentUserService.GetInstructorId();

        if (instructorId == null)
            throw new UnauthorizedAccessException("User is not associated with an instructor");

        // Get programs this instructor teaches
        var instructorProgramIds = await _dbContext.Instructors
            .Where(i => i.Id == instructorId.Value)
            .SelectMany(i => i.Programs.Select(p => p.Id))
            .ToListAsync(cancellationToken);

        if (!instructorProgramIds.Any())
            return new List<InstructorStudentDto>(); // Instructor doesn't teach any programs

        // Get students enrolled in those programs
        var students = await _dbContext.Students
            .Where(s => s.ProgramEnrollments.Any(e =>
                e.IsActive && instructorProgramIds.Contains(e.ProgramId)))
            .Select(s => new InstructorStudentDto(
                s.Id,
                s.Name,
                s.Email,
                s.Phone,
                s.ProgramEnrollments
                    .Where(e => e.IsActive && instructorProgramIds.Contains(e.ProgramId))
                    .Select(e => new StudentProgramDto(
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
                s.TestHistory
                    .Where(t => instructorProgramIds.Contains(t.ProgramId))
                    .OrderByDescending(t => t.TestDate)
                    .Select(t => new TestHistoryDto(
                        t.TestDate,
                        t.ProgramName,
                        t.RankAchieved,
                        t.Result,
                        t.Notes
                    )).ToList()
            )).ToListAsync(cancellationToken);

        return students;
    }
}