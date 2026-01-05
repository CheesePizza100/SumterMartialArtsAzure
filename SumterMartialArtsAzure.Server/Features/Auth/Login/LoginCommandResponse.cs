using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Login;

public record LoginCommandResponse(string Token, string Username, Guid UserId, string Role, bool MustChangePassword) : IAuditableResponse
{
    public string EntityId => UserId.ToString();

    public object GetAuditDetails() => new
    {
        Username
    };
}