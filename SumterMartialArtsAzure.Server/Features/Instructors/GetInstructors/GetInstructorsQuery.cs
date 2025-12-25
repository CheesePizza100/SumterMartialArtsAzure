using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;

public record GetInstructorsQuery : IRequest<List<GetInstructorsResponse>>;