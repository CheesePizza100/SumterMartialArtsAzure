using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Entities;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentAttendance;

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
        var enrollment = await _dbContext.Set<StudentProgramEnrollment>()
            .Where(e => e.StudentId == request.StudentId
                        && e.ProgramId == request.ProgramId
                        && e.IsActive)
            .Select(e => new GetStudentAttendanceResponse(
                e.Attendance.Last30Days,
                e.Attendance.Total,
                e.Attendance.AttendanceRate
            ))
            .FirstOrDefaultAsync(cancellationToken);

        return enrollment;
    }
}