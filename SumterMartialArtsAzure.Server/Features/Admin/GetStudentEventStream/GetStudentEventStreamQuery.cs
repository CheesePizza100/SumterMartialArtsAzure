using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentEventStream;

public record GetStudentEventStreamQuery(
    int StudentId,
    int ProgramId
) : IRequest<List<GetStudentEventStreamResponse>>;