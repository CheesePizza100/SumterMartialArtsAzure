using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordTestResult;

public record InstructorRecordTestRequest(
    int ProgramId,
    string ProgramName,
    string Rank,
    string Result,
    string Notes,
    DateTime TestDate
);

public record InstructorRecordTestCommand(
    int StudentId,
    int ProgramId,
    string ProgramName,
    string Rank,
    string Result,
    string Notes,
    DateTime TestDate
) : IRequest<InstructorRecordTestResponse>, IAuditableCommand
{
    public string Action => AuditActions.InstructorRecordedTest;
    public string EntityType => "TestResult";
}