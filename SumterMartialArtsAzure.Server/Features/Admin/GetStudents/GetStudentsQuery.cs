using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudents;

public record GetStudentsQuery
    : IRequest<List<GetStudentsResponse>>;
