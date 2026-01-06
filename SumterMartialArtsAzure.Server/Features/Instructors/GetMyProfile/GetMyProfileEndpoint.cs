using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;

public record GetInstructorProfileQuery : IRequest<InstructorProfileResponse>;

public record InstructorProfileResponse(
    int Id,
    string Name,
    string Email,
    string Rank,
    string Bio,
    string PhotoUrl,
    List<ProgramDto> Programs,
    List<AvailabilityRuleDto> ClassSchedule
);

public record ProgramDto(int Id, string Name);

public record AvailabilityRuleDto(
    string DaysOfWeek,
    TimeSpan StartTime,
    TimeSpan Duration
);

public class GetInstructorProfileHandler
    : IRequestHandler<GetInstructorProfileQuery, InstructorProfileResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;

    public GetInstructorProfileHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
    }

    public async Task<InstructorProfileResponse> Handle(
        GetInstructorProfileQuery request,
        CancellationToken cancellationToken)
    {
        var instructorId = _currentUserService.GetInstructorId();

        if (instructorId == null)
            throw new UnauthorizedAccessException("User is not associated with an instructor");

        var instructor = await _dbContext.Instructors
            .Include(i => i.Programs)
            .Include(i => i.ClassSchedule)
            .FirstOrDefaultAsync(i => i.Id == instructorId.Value, cancellationToken);

        if (instructor == null)
            throw new InvalidOperationException("Instructor not found");

        return new InstructorProfileResponse(
            instructor.Id,
            instructor.Name,
            instructor.Email,
            instructor.Rank,
            instructor.Bio,
            instructor.PhotoUrl,
            instructor.Programs
                .Select(p => new ProgramDto(p.Id, p.Name))
                .ToList(),
            instructor.ClassSchedule
                .Select(cs => new AvailabilityRuleDto(
                    string.Join(", ", cs.DaysOfWeek),
                    cs.StartTime,
                    cs.Duration
                )).ToList()
        );
    }
}

// Endpoint
public static class GetInstructorProfileEndpoint
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/instructors/me",
                async (IMediator mediator) =>
                {
                    var query = new GetInstructorProfileQuery();
                    var result = await mediator.Send(query);
                    return Results.Ok(result);
                })
            .RequireAuthorization()
            .WithName("GetInstructorProfile")
            .WithTags("Instructors");
    }
}