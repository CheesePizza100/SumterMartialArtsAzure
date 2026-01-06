using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;

public record GetMyStudentsQuery : IRequest<List<InstructorStudentDto>>;
public record InstructorStudentDto(
    int Id,
    string Name,
    string Email,
    string Phone,
    List<StudentProgramDto> Programs,
    List<TestHistoryDto> TestHistory
);
public record StudentProgramDto(
    int ProgramId,
    string ProgramName,
    string CurrentRank,
    DateTime EnrolledDate,
    DateTime? LastTestDate,
    string? InstructorNotes,
    AttendanceDto Attendance
);