namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms
{
    public record GetProgramsResponse(
        int Id,
        string Name,
        string Description,
        string AgeGroup,
        string ImageUrl,
        string Duration,
        string Schedule,
        List<int> InstructorIds
    );
}
