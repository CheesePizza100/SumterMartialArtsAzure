using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetStudentDetail;

public record GetStudentDetailQuery(int StudentId) : IRequest<GetMyStudentsResponse>;