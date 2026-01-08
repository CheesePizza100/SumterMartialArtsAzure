using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.RecordAttendance;

public class RecordAttendanceHandler
    : IRequestHandler<RecordAttendanceCommand, RecordAttendanceCommandResponse>
{
    private readonly AppDbContext _dbContext;

    public RecordAttendanceHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RecordAttendanceCommandResponse> Handle(
        RecordAttendanceCommand request,
        CancellationToken cancellationToken)
    {
        var student = await _dbContext.Students
            .Include(s => s.ProgramEnrollments)
            .FirstOrDefaultAsync(s => s.Id == request.StudentId, cancellationToken);

        if (student == null)
            return new RecordAttendanceCommandResponse(false, "Student not found", null);

        student.RecordAttendance(request.ProgramId, request.ClassesAttended);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new RecordAttendanceCommandResponse(
            true,
            $"Recorded {request.ClassesAttended} class(es) for student",
            student.Id
        );
    }
}