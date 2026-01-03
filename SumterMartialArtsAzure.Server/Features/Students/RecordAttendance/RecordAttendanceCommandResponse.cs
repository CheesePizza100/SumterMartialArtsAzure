using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.RecordAttendance;

public record RecordAttendanceCommandResponse(
    bool Success,
    string Message,
    int? StudentId
) : IAuditableResponse
{
    public string EntityId => StudentId?.ToString() ?? "unknown";

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}