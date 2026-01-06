using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.CreateInstructorLogin;

public record CreateInstructorLoginRequest(string Username, string? Password);

public record CreateInstructorLoginCommand(
    int InstructorId,
    string Username,
    string? Password
) : IRequest<CreateInstructorLoginResponse>, IAuditableCommand
{
    public string Action => AuditActions.InstructorLoginCreated;
    public string EntityType => "User";
}