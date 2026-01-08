using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.DeactivateStudent;

public record DeactivateStudentCommandResponse(
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