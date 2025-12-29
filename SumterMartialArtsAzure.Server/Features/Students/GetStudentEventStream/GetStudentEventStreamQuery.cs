using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentEventStream;

public record GetStudentEventStreamQuery(
    int StudentId,
    int ProgramId
) : IRequest<List<GetStudentEventStreamResponse>>;