using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.DeactivateStudent;

public record DeactivateStudentCommand(
    int StudentId
) : IRequest<DeactivateStudentCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentDeactivated;
    public string EntityType => "Student";
}