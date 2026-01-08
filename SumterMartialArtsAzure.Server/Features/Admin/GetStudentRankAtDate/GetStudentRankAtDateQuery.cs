using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentRankAtDate;

public record GetStudentRankAtDateQuery(
    int StudentId,
    int ProgramId,
    DateTime AsOfDate
) : IRequest<GetStudentRankAtDateResponse>;