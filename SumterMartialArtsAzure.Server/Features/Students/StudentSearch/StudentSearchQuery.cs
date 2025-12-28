using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;

public record StudentSearchQuery(string SearchTerm)
    : IRequest<List<GetStudentSearchResponse>>;