using SumterMartialArtsAzure.Server.Api.Auditing;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.UpdateProgramNotes;

public record InstructorUpdateNotesResponse(
    bool Success,
    string Message,
    int? EnrollmentId
) : IAuditableResponse
{
    public string EntityId => EnrollmentId?.ToString() ?? "unknown";

    public object GetAuditDetails() => new
    {
        Success,
        Message
    };
}