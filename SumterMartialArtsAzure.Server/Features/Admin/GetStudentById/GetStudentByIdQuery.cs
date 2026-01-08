using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentById;

public record GetStudentByIdQuery(int Id)
    : IRequest<GetStudentByIdResponse>;