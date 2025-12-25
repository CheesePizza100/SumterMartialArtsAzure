using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;

public record GetInstructorAvailabilityQuery(int InstructorId)
    : IRequest<GetInstructorAvailabilityResponse?>;