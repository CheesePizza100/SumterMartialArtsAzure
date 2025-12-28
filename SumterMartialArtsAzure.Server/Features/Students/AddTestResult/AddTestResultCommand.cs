using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.AddTestResult;

public record AddTestResultRequest(
    int ProgramId,
    string ProgramName,
    string Rank,
    string Result,
    string Notes,
    DateTime TestDate
);

public record AddTestResultCommand(
    int StudentId,
    int ProgramId,
    string ProgramName,
    string Rank,
    string Result,
    string Notes,
    DateTime TestDate
) : IRequest<AddTestResultResponse>;