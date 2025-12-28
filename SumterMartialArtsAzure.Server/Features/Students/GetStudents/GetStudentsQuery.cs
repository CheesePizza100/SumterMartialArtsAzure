using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;

public record GetStudentsQuery
    : IRequest<List<GetStudentsResponse>>;
