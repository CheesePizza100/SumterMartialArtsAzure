using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms
{
    public record GetProgramsQuery
        : IRequest<List<GetProgramsResponse>>;
}