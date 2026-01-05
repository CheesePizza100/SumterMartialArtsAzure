using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.ChangePassword;

public record ChangePasswordCommandResponse(
    bool Success,
    string Message,
    string UserId
) : IAuditableResponse
{
    public string EntityId => UserId;

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}