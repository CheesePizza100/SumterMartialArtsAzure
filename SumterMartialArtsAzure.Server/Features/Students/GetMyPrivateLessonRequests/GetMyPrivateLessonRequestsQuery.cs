using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;

public record GetMyPrivateLessonRequestsQuery(int StudentId)
    : IRequest<List<PrivateLessonRequestResponse>>;