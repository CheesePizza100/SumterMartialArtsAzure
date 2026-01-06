using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordAttendance;

public record InstructorRecordAttendanceRequest(int ProgramId, int ClassesAttended);

public record InstructorRecordAttendanceCommand(
    int StudentId,
    int ProgramId,
    int ClassesAttended
) : IRequest<InstructorRecordAttendanceResponse>, IAuditableCommand
{
    public string Action => AuditActions.InstructorRecordedAttendance;
    public string EntityType => "Attendance";
}