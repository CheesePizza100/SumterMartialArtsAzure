using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;

public record GetInstructorByIdResponse
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Email { get; init; }
    public string Rank { get; init; }
    public string Bio { get; init; }
    public string? PhotoUrl { get; init; }
    public List<string> Achievements { get; init; }
    public List<ClassScheduleDto> ClassSchedule { get; init; }
    public List<ProgramDto> Programs { get; init; }

    public GetInstructorByIdResponse(Instructor instructor)
    {
        Id = instructor.Id;
        Name = instructor.Name;
        Email = instructor.Email;
        Rank = instructor.Rank;
        Bio = instructor.Bio;
        PhotoUrl = instructor.PhotoUrl;
        Achievements = instructor.Achievements?.ToList() ?? new List<string>();
        ClassSchedule = instructor.ClassSchedule?.Select(ar => new ClassScheduleDto
        {
            DaysOfWeek = ar.DaysOfWeek.Select(d => (int)d).ToList(),
            StartTime = ar.StartTime,
            Duration = ar.Duration
        }).ToList() ?? new List<ClassScheduleDto>();
        Programs = instructor.Programs?.Select(p => new ProgramDto
        (
            p.Id,
            p.Name,
            p.Description
        )).ToList() ?? new List<ProgramDto>();
    }
}

public record ClassScheduleDto
{
    public List<int> DaysOfWeek { get; init; }
    public TimeSpan StartTime { get; init; }
    public TimeSpan Duration { get; init; }
}
public record ProgramDto(int Id, string Name, string Description);