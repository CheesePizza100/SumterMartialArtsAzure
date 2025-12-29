using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.RecordAttendance;

public record RecordAttendanceRequest(int ProgramId, int ClassesAttended);

public record RecordAttendanceCommand(
    int StudentId,
    int ProgramId,
    int ClassesAttended
) : IRequest<RecordAttendanceCommandResponse>;