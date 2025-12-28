namespace SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;

public record GetStudentSearchResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    AttendanceDto Attendance,
    List<TestHistoryDto> TestHistory
);
public record ProgramEnrollmentDto(
    string Name,
    string Rank,
    DateTime EnrolledDate,
    DateTime? LastTest,
    string? TestNotes
);
public record AttendanceDto(
    int Last30Days,
    int Total,
    int AttendanceRate
);

public record TestHistoryDto(
    DateTime Date,
    string Program,
    string Rank,
    string Result,
    string Notes
);