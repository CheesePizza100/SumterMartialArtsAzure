namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentEventStream;

public record GetStudentEventStreamResponse(
    Guid EventId,
    string EventType,
    DateTime OccurredAt,
    int Version,
    string EventData // JSON string
);