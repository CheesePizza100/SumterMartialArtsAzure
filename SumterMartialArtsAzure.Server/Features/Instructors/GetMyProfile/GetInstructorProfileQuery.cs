using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;

public record GetInstructorProfileQuery : IRequest<InstructorProfileResponse>;

public record InstructorProfileResponse(
    int Id,
    string Name,
    string Email,
    string Rank,
    string Bio,
    string PhotoUrl,
    List<ProgramDto> Programs,
    List<AvailabilityRuleDto> ClassSchedule
);

public record ProgramDto(int Id, string Name);

public record AvailabilityRuleDto(
    string DaysOfWeek,
    TimeSpan StartTime,
    TimeSpan Duration
);