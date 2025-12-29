using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;

public record GetStudentAttendanceQuery(int StudentId, int ProgramId)
    : IRequest<GetStudentAttendanceResponse>;