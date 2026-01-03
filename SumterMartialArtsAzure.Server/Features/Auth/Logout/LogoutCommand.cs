using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Logout;

public record LogoutCommand() : IRequest<LogoutCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.UserLoggedOut;
    public string EntityType => "User";
};
public record LogoutCommandResponse(Guid UserId) : IAuditableResponse
{
    public string EntityId => UserId.ToString();
    public object GetAuditDetails() => new { };
}