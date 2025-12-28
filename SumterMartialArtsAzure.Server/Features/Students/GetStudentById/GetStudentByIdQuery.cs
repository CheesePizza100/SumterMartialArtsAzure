using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

public record GetStudentByIdQuery(int Id)
    : IRequest<GetStudentByIdResponse>;