using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;

public record UpdateStatusRequest(string Status, string RejectionReason);

public record UpdatePrivateLessonStatusCommand(
    int Id,
    string Status,
    string RejectionReason
) : IRequest<UpdatePrivateLessonStatusResponse>;