using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudent;

public record CreateStudentRequest(
    string Name,
    string Email,
    string Phone
);

public record CreateStudentCommand(string Name, string Email, string Phone)
    : IRequest<GetStudentByIdResponse>;