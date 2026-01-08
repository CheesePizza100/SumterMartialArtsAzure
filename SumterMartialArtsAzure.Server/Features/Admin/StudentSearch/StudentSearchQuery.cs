using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.StudentSearch;

public record StudentSearchQuery(string SearchTerm)
    : IRequest<List<GetStudentSearchResponse>>;