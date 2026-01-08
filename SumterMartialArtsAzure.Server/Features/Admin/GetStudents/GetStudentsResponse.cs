using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudents;

public record GetStudentsResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    bool HasLogin,
    List<ProgramEnrollmentDto> Programs,
    List<TestHistoryDto> TestHistory
);
