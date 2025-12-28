using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;

public record UpdateProgramNotesRequest(
    string Notes
);

public record UpdateProgramNotesCommand(
    int StudentId,
    int ProgramId,
    string Notes
) : IRequest<bool>;