using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;

public record GetMyStudentsQuery : IRequest<List<GetMyStudentsResponse>>;
