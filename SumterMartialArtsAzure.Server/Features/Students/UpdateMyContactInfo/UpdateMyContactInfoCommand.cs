using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

public record UpdateMyContactInfoRequest(string? Name, string? Email, string? Phone);

public record UpdateMyContactInfoCommand(
    string? Name,
    string? Email,
    string? Phone
) : IRequest<UpdateMyContactInfoCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentUpdatedOwnProfile;
    public string EntityType => "Student";
}