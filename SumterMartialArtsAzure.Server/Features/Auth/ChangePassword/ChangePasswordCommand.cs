using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.ChangePassword;

public record ChangePasswordRequest(string CurrentPassword, string NewPassword);

public record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<ChangePasswordCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.PasswordChanged;
    public string EntityType => "User";
}