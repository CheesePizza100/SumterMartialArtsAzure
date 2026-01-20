using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentRankAtDate;

public class GetStudentRankAtDateHandler
    : IRequestHandler<GetStudentRankAtDateQuery, GetStudentRankAtDateResponse>
{
    private readonly IEnumerable<IEventProjector> _eventProjectors;
    private readonly AppDbContext _dbContext;

    public GetStudentRankAtDateHandler(IEnumerable<IEventProjector> eventProjectors, AppDbContext dbContext)
    {
        _eventProjectors = eventProjectors;
        _dbContext = dbContext;
    }

    public async Task<GetStudentRankAtDateResponse> Handle(
        GetStudentRankAtDateQuery request,
        CancellationToken cancellationToken)
    {
        // Get all events up to the specified date
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.StudentId == request.StudentId
                        && e.ProgramId == request.ProgramId
                        && e.OccurredAt <= request.AsOfDate)
            .OrderBy(e => e.Version)
            .ToListAsync(cancellationToken);

        if (!events.Any())
            return null;

        // Replay events to reconstruct state
        var state = new StudentProgressionState();

        foreach (var evt in events)
        {
            var eventApplier = _eventProjectors.Single(x => x.EventType == evt.EventType);
            eventApplier.Project(evt, state);
        }

        return new GetStudentRankAtDateResponse(
            state.CurrentRank,
            state.EnrolledDate,
            state.LastTestDate,
            state.LastTestNotes,
            events.Count
        );
    }
}