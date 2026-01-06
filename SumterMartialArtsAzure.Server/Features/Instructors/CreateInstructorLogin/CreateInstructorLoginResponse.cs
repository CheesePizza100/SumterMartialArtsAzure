using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.CreateInstructorLogin;

public record CreateInstructorLoginResponse(
    bool Success,
    string Message,
    string Username,
    string TemporaryPassword,
    string UserId
) : IAuditableResponse
{
    public string EntityId => UserId;

    public object GetAuditDetails() => new
    {
        Username
    };
}