namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;

public record UpdatePrivateLessonStatusResponse(
    bool Success,
    string? Message = null
);