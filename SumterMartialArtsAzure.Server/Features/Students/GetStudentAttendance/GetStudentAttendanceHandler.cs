using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;

public class GetStudentAttendanceHandler
    : IRequestHandler<GetStudentAttendanceQuery, GetStudentAttendanceResponse>
{
    private readonly AppDbContext _dbContext;

    public GetStudentAttendanceHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetStudentAttendanceResponse> Handle(
        GetStudentAttendanceQuery request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Where(s => s.Id == request.StudentId)
            .Select(s => new GetStudentAttendanceResponse(
                s.Attendance.Last30Days,
                s.Attendance.Total,
                s.Attendance.AttendanceRate
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return student;
    }
}