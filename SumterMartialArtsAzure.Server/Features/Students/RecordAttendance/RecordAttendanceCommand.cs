using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.RecordAttendance;

public record RecordAttendanceRequest(int ProgramId, int ClassesAttended);

public record RecordAttendanceCommand(
    int StudentId,
    int ProgramId,
    int ClassesAttended
) : IRequest<RecordAttendanceCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.AttendanceRecorded;
    public string EntityType => "Attendance";
}