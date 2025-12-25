using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;

public record GetInstructorByIdQuery(int Id)
    : IRequest<GetInstructorByIdResponse?>;