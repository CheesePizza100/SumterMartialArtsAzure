using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.EnrollInProgram;

public record EnrollInProgramCommandResponse(
    bool Success,
    string Message,
    int? EnrollmentId
) : IAuditableResponse
{
    public string EntityId => EnrollmentId?.ToString() ?? "unknown";

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}