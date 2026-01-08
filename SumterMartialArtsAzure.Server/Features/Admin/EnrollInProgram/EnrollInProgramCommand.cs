using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.EnrollInProgram;

public record EnrollInProgramRequest(int ProgramId, string ProgramName, string InitialRank);

public record EnrollInProgramCommand(
    int StudentId,
    int ProgramId,
    string ProgramName,
    string InitialRank
) : IRequest<EnrollInProgramCommandResponse>, IAuditableCommand
{
    public string Action => AuditActions.StudentEnrolled;
    public string EntityType => "Enrollment";
}