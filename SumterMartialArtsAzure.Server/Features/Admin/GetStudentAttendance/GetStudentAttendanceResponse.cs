namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentAttendance;

public record GetStudentAttendanceResponse(int Last30Days, int Total, int AttendanceRate);