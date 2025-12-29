namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentRankAtDate;

public record GetStudentRankAtDateResponse(
    string Rank,
    DateTime? EnrolledDate,
    DateTime? LastTestDate,
    string? LastTestNotes,
    int TotalEventsProcessed
);