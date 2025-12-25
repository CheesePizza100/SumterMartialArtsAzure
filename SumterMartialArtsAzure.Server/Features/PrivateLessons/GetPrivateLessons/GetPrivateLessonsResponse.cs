namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;

public record GetPrivateLessonsResponse(
    int Id,
    int InstructorId,
    string InstructorName,
    string StudentName,
    string StudentEmail,
    string? StudentPhone,
    DateTime RequestedStart,
    DateTime RequestedEnd,
    string Status,
    string? Notes,
    string? RejectionReason,
    DateTime CreatedAt
);