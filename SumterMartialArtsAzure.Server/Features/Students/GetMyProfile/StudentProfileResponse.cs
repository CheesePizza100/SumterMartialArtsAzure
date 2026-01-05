using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;

public record StudentProfileResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    List<TestHistoryDto> TestHistory
);