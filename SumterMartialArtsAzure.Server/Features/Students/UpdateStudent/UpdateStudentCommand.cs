using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

public record UpdateStudentRequest(
    string? Name,
    string? Email,
    string? Phone
);

public record UpdateStudentCommand(
    int Id,
    string? Name,
    string? Email,
    string? Phone
) : IRequest<UpdateStudentCommandResponse>;