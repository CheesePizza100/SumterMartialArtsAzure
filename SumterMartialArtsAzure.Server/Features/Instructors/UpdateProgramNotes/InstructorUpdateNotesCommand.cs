using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.UpdateProgramNotes;

public record InstructorUpdateNotesRequest(string Notes);

public record InstructorUpdateNotesCommand(
    int StudentId,
    int ProgramId,
    string Notes
) : IRequest<InstructorUpdateNotesResponse>, IAuditableCommand
{
    public string Action => AuditActions.InstructorUpdatedNotes;
    public string EntityType => "Enrollment";
}