namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;

public record PrivateLessonRequestResponse(
    int Id,
    int InstructorId,
    string InstructorName,
    DateTime RequestedStart,
    DateTime RequestedEnd,
    string Status,
    string? Notes,
    string? RejectionReason,
    DateTime CreatedAt
);