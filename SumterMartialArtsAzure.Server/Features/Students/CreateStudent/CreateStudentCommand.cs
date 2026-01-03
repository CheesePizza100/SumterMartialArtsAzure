using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudent;

public record CreateStudentRequest(
    string Name,
    string Email,
    string Phone
);

public record CreateStudentCommand(string Name, string Email, string Phone)
    : IRequest<GetStudentByIdResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentCreated;
    public string EntityType => "Student";
}