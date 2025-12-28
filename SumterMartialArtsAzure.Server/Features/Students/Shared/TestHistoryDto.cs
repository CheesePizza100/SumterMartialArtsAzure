namespace SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

public record TestHistoryDto(
    DateTime Date,
    string Program,
    string Rank,
    string Result,
    string Notes
);