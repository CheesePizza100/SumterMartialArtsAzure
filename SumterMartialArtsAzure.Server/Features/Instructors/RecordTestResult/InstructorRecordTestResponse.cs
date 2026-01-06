using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordTestResult;

public record InstructorRecordTestResponse(
    bool Success,
    string Message,
    int? TestResultId
) : IAuditableResponse
{
    public string EntityId => TestResultId?.ToString() ?? "unknown";

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}