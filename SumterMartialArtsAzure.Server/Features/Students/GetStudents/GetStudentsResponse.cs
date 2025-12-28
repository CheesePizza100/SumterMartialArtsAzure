namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public record GetStudentsResponse(
    int Id,
    string Name,
    string Description,
    string AgeGroup,
    string ImageUrl,
    string Duration,
    string Schedule,
    List<int> InstructorIds
);