using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;

public record GetInstructorProfileQuery : IRequest<InstructorProfileResponse>;
