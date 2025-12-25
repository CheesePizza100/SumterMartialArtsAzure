using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;

public record GetPrivateLessonsQuery(
    string Filter = "Pending"
) : IRequest<List<GetPrivateLessonsResponse>>;