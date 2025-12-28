namespace SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

public record ProgramEnrollmentDto(
    string Name,
    string Rank,
    DateTime EnrolledDate,
    DateTime? LastTest,
    string? TestNotes
);