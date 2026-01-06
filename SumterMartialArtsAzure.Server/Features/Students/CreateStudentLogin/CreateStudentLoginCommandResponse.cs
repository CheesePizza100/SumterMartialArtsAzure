using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudentLogin;

public record CreateStudentLoginCommandResponse(
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
        Username,
        StudentId = UserId
    };
}