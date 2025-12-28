using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public record UpdateStudentCommandResponse(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<ProgramEnrollmentDto> Programs,
    AttendanceDto Attendance,
    List<TestHistoryDto> TestHistory
);