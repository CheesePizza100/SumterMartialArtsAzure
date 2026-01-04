using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudentLogin;

public record CreateStudentLoginRequest(string Username, string? Password);

public record CreateStudentLoginCommand(
    int StudentId,
    string Username,
    string Password
) : IRequest<CreateStudentLoginCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentLoginCreated;
    public string EntityType => "User";
}