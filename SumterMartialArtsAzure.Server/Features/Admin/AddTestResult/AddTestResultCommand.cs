using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.AddTestResult;

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
) : IRequest<AddTestResultResponse>, IAuditableCommand
{
    public string Action => AuditActions.TestResultAdded;
    public string EntityType => "TestResult";
}