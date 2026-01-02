using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;

public record UpdateProgramNotesRequest(
    string Notes
);

public record UpdateProgramNotesCommand(
    int StudentId,
    int ProgramId,
    string Notes
) : IRequest<UpdateProgramNotesResponse>, IAuditableCommand
{
    public string Action => AuditActions.ProgramNotesUpdated;
    public string EntityType => "Enrollment";
}
public record UpdateProgramNotesResponse(
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