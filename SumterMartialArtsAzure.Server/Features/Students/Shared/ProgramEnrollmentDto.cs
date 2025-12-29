namespace SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

public record ProgramEnrollmentDto(
    int ProgramId,
    string Name,
    string Rank,
    DateTime EnrolledDate,
    DateTime? LastTest,
    string? TestNotes,
    AttendanceDto Attendance
);