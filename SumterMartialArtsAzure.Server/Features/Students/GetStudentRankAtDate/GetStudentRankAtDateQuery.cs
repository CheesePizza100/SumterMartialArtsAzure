using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentRankAtDate;

public record GetStudentRankAtDateQuery(
    int StudentId,
    int ProgramId,
    DateTime AsOfDate
) : IRequest<GetStudentRankAtDateResponse>;