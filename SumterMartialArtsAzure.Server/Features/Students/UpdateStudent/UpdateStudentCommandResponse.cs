using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public record UpdateStudentCommandResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    List<TestHistoryDto> TestHistory
) : IAuditableResponse
{
    public string EntityId => Id.ToString();

    public object GetAuditDetails() => new
    {
        Name,
        Email,
        Phone
    };
}