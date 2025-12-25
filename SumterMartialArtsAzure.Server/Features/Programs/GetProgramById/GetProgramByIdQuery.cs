namespace SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;

using MediatR;

public record GetProgramByIdQuery(int Id)
    : IRequest<GetProgramByIdResponse?>;
