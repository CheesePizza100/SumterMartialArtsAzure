using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.AnalyticsResults;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Entities;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.Calculators;

public interface IProgressionAnalyticsCalculator
{
    Task<IAnalyticsResult> Calculate(IQueryable<StudentProgressionEventRecord> events, int? programId, CancellationToken cancellationToken);
}
public class EnrollmentCountCalculator : IProgressionAnalyticsCalculator
{
    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var count = await events
            .AsNoTracking()
            .Where(e => e.EventType == nameof(EnrollmentEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .GroupBy(e => new { e.StudentId, e.ProgramId })
            .CountAsync(cancellationToken);

        return new EnrollmentCountResult(count);
    }
}

public class TestStatisticsCalculator : IProgressionAnalyticsCalculator
{
    private readonly IStudentProgressionEventService _eventService;

    public TestStatisticsCalculator(IStudentProgressionEventService eventService)
    {
        _eventService = eventService;
    }
    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var testEvents = await _eventService.GetTestAttemptEvents(programId, cancellationToken);

        var totalTests = testEvents.Count;
        var passedTests = testEvents.Count(t => t.Data.Passed);
        var failedTests = testEvents.Count(t => !t.Data.Passed);
        var passRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;

        return new TestStatisticsResult(
            totalTests,
            passedTests,
            failedTests,
            Math.Round(passRate, 2)
        );
    }
}
public class PromotionCountCalculator : IProgressionAnalyticsCalculator
{
    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var count = await events
            .AsNoTracking()
            .Where(e => e.EventType == nameof(PromotionEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .CountAsync(cancellationToken);

        return new PromotionCountResult(count);
    }
}

public class AverageTimeToRankCalculator : IProgressionAnalyticsCalculator
{
    private readonly IStudentProgressionEventService _eventService;
    private readonly RankProgressionCalculator _rankCalculator;

    public AverageTimeToRankCalculator(
        IStudentProgressionEventService eventService,
        RankProgressionCalculator rankCalculator)
    {
        _eventService = eventService;
        _rankCalculator = rankCalculator;
    }

    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var enrollments = await _eventService.GetEnrollmentEvents(programId, cancellationToken);
        var promotions = await _eventService.GetPromotionEvents(programId, cancellationToken);

        var averagesByRank = promotions
            .GroupBy(p => p.Data.ToRank)
            .Select(rankGroup => _rankCalculator.Calculate(rankGroup, enrollments))
            .ToList();

        return new AverageTimeToRankResult(averagesByRank);
    }
}
public class RankProgressionCalculator
{
    public RankProgression Calculate(
        IGrouping<string, PromotionEvent> rankGroup,
        List<EnrollmentEvent> enrollments)
    {
        var calculator = new TimeToPromotionCalculator();

        var times = rankGroup
            .Select(promotion => calculator.Calculate(promotion, enrollments))
            .Where(days => days.HasValue)
            .Select(days => days!.Value)
            .ToList();

        var avgDays = times.Any() ? Math.Round(times.Average(), 0) : 0.0;
        return new RankProgression(rankGroup.Key, avgDays);
    }
}

public class TimeToPromotionCalculator
{
    public double? Calculate(
        PromotionEvent promotion,
        List<EnrollmentEvent> enrollments)
    {
        var enrollment = enrollments
            .Where(e => e.StudentId == promotion.StudentId && e.ProgramId == promotion.ProgramId)
            .OrderBy(e => e.OccurredAt)
            .FirstOrDefault();

        return enrollment != null
            ? (promotion.OccurredAt - enrollment.OccurredAt).TotalDays
            : null;
    }
}
public class MonthlyTestActivityCalculator : IProgressionAnalyticsCalculator
{
    private readonly IStudentProgressionEventService _eventService;

    public MonthlyTestActivityCalculator(IStudentProgressionEventService eventService)
    {
        _eventService = eventService;
    }

    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var testEvents = await _eventService.GetTestAttemptEvents(programId, cancellationToken);

        var testsByMonth = testEvents
            .GroupBy(t => new { t.OccurredAt.Year, t.OccurredAt.Month })
            .Select(g => new MonthlyTestActivity(
                g.Key.Year,
                g.Key.Month,
                g.Count()
            ))
            .OrderByDescending(m => m.TestCount)
            .Take(6)
            .ToList();

        return new MonthlyTestActivityResult(testsByMonth);
    }
}
public class RankDistributionCalculator : IProgressionAnalyticsCalculator
{
    private readonly AppDbContext _dbContext;

    public RankDistributionCalculator(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var enrollmentsQuery = _dbContext.Set<StudentProgramEnrollment>()
            .AsNoTracking()
            .Where(e => e.IsActive);

        if (programId.HasValue)
        {
            enrollmentsQuery = enrollmentsQuery.Where(e => e.ProgramId == programId.Value);
        }

        var enrollments = await enrollmentsQuery
            .Select(e => e.CurrentRank)
            .ToListAsync(cancellationToken);

        var distribution = enrollments
            .GroupBy(rank => rank)
            .Select(g => new RankDistribution(g.Key, g.Count()))
            .OrderByDescending(r => r.Count)
            .ToList();

        return new RankDistributionResult(distribution);
    }
}
