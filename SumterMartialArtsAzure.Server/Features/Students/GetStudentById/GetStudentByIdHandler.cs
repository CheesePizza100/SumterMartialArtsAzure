using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

public class GetStudentByIdHandler
    : IRequestHandler<GetStudentByIdQuery, GetStudentByIdResponse>
{
    private readonly AppDbContext _dbContext;

    public GetStudentByIdHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetStudentByIdResponse> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .Include(s => s.TestHistory)
            .Where(s => s.Id == request.Id)
            .Select(s => new GetStudentByIdResponse(
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
            .FirstOrDefaultAsync(cancellationToken);

        return student;
    }
}
