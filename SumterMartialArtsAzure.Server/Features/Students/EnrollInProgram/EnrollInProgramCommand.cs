using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.EnrollInProgram;

public record EnrollInProgramRequest(int ProgramId, string ProgramName, string InitialRank);

public record EnrollInProgramCommand(
    int StudentId,
    int ProgramId,
    string ProgramName,
    string InitialRank
) : IRequest<EnrollInProgramCommandResponse>;