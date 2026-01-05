using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;

public record GetMyProfileQuery : IRequest<StudentProfileResponse>;