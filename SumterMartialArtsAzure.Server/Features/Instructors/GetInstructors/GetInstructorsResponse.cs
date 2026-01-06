using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public record GetInstructorsResponse(
    int Id,
    string Name,
    string Email,
    string Rank,
    string Bio,
    string PhotoUrl,
    bool HasLogin,
    List<int> ProgramIds
);