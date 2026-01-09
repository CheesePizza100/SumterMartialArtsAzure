namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentRankAtDate;

public record GetStudentRankAtDateResponse(
    string Rank,
    DateTime? EnrolledDate,
    DateTime? LastTestDate,
    string? LastTestNotes,
    int TotalEventsProcessed
);