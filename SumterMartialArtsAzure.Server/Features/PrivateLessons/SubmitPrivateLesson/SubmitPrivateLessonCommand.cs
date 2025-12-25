using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;

public record SubmitPrivateLessonCommand(
    int InstructorId,
    string StudentName,
    string StudentEmail,
    string? StudentPhone,
    DateTime RequestedStart,
    DateTime RequestedEnd,
    string? Notes) : IRequest<SubmitPrivateLessonResponse>;