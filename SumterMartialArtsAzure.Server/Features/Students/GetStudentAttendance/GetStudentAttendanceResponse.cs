namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;

public record GetStudentAttendanceResponse(int Last30Days, int Total, int AttendanceRate);