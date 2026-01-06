using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

public record UpdateMyContactInfoCommandResponse(
    bool Success,
    string Message,
    int StudentId
) : IAuditableResponse
{
    public string EntityId => StudentId.ToString();

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}