using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.StudentSearch;

public record GetStudentSearchResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    List<TestHistoryDto> TestHistory
);