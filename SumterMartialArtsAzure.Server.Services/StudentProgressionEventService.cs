using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Events;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace SumterMartialArtsAzure.Server.Services;

public interface IStudentProgressionEventService
{
    Task<List<EnrollmentEvent>> GetEnrollmentEvents(int? programId, CancellationToken cancellationToken);
    Task<List<PromotionEvent>> GetPromotionEvents(int? programId, CancellationToken cancellationToken);
    Task<List<TestAttemptEvent>> GetTestAttemptEvents(int? programId, CancellationToken cancellationToken);
}

public class StudentProgressionEventService : IStudentProgressionEventService
{
    private readonly AppDbContext _dbContext;

    public StudentProgressionEventService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<EnrollmentEvent>> GetEnrollmentEvents(
        int? programId,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.EventType == nameof(EnrollmentEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .ToListAsync(cancellationToken);

        return events
            .Select(e => new EnrollmentEvent(
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                JsonSerializer.Deserialize<EnrollmentEventData>(e.EventData)!
            ))
            .Where(e => e.Data != null)
            .ToList();
    }

    public async Task<List<PromotionEvent>> GetPromotionEvents(
        int? programId,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.EventType == nameof(PromotionEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .ToListAsync(cancellationToken);

        return events
            .Select(e => new PromotionEvent(
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                JsonSerializer.Deserialize<PromotionEventData>(e.EventData)!
            ))
            .Where(e => e.Data != null)
            .ToList();
    }

    public async Task<List<TestAttemptEvent>> GetTestAttemptEvents(
        int? programId,
        CancellationToken cancellationToken)
    {
        var events = await _dbContext.StudentProgressionEvents
            .AsNoTracking()
            .Where(e => e.EventType == nameof(TestAttemptEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .ToListAsync(cancellationToken);

        return events
            .Select(e => new TestAttemptEvent(
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                JsonSerializer.Deserialize<TestAttemptEventData>(e.EventData)!
            ))
            .Where(e => e.Data != null)
            .ToList();
    }
}