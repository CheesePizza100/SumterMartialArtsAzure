using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentAttendance;

public record GetStudentAttendanceQuery(int StudentId, int ProgramId)
    : IRequest<GetStudentAttendanceResponse>;