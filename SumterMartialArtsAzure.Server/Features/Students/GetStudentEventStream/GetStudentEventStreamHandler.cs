using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentEventStream;

public class GetStudentEventStreamHandler
    : IRequestHandler<GetStudentEventStreamQuery, List<GetStudentEventStreamResponse>>
{
    private readonly AppDbContext _dbContext;

    public GetStudentEventStreamHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<GetStudentEventStreamResponse>> Handle(
        GetStudentEventStreamQuery request,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.StudentProgressionEvents
            .Where(e => e.StudentId == request.StudentId && e.ProgramId == request.ProgramId)
            .OrderBy(e => e.Version)
            .Select(e => new GetStudentEventStreamResponse(
                e.EventId,
                e.EventType,
                e.OccurredAt,
                e.Version,
                e.EventData
            ))
            .ToListAsync(cancellationToken);

        return events;
    }
}