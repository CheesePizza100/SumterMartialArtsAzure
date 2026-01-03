using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public record UpdateStudentRequest(
    string? Name,
    string? Email,
    string? Phone
);

public record UpdateStudentCommand(
    int Id,
    string? Name,
    string? Email,
    string? Phone
) : IRequest<UpdateStudentCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentUpdated;
    public string EntityType => "Student";
}