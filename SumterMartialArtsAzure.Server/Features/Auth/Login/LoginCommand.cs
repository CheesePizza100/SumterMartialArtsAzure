using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Login;

public record LoginRequest(string UserName, string Password);

public record LoginCommand(string Username, string Password) : IRequest<LoginCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.UserLoggedIn;
    public string EntityType => "User";
}