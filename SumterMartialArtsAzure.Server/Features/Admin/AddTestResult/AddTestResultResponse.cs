using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.AddTestResult;

public record AddTestResultResponse(
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