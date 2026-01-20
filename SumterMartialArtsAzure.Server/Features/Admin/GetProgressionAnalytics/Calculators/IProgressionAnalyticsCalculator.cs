using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.AnalyticsResults;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Entities;
using SumterMartialArtsAzure.Server.Domain.Events;

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
    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var testEvents = await events
            .AsNoTracking()
            .Where(e => e.EventType == nameof(TestAttemptEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .ToListAsync(cancellationToken);

        var testData = testEvents
            .Select(e => JsonSerializer.Deserialize<TestAttemptEventData>(e.EventData))
            .Where(data => data != null)
            .ToList();

        var totalTests = testData.Count;
        var passedTests = testData.Count(t => t!.Passed);
        var failedTests = testData.Count(t => !t!.Passed);
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
    private readonly string _targetRank;

    public AverageTimeToRankCalculator(string targetRank = "Blue Belt")
    {
        _targetRank = targetRank;
    }

    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var allEvents = await events
            .AsNoTracking()
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .Where(e => e.EventType == nameof(PromotionEventData) ||
                       e.EventType == nameof(EnrollmentEventData))
            .OrderBy(e => e.OccurredAt)
            .ToListAsync(cancellationToken);

        var promotions = allEvents
            .Where(e => e.EventType == nameof(PromotionEventData))
            .Select(e => new
            {
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                Data = JsonSerializer.Deserialize<PromotionEventData>(e.EventData)
            })
            .Where(p => p.Data?.ToRank == _targetRank)
            .ToList();

        if (!promotions.Any())
            return new AverageTimeToRankResult(0.0, _targetRank);

        var times = promotions
            .Select(promotion =>
            {
                var enrollment = allEvents
                    .Where(e => e.EventType == nameof(EnrollmentEventData)
                             && e.StudentId == promotion.StudentId
                             && e.ProgramId == promotion.ProgramId)
                    .OrderBy(e => e.OccurredAt)
                    .FirstOrDefault();

                return enrollment != null
                    ? (promotion.OccurredAt - enrollment.OccurredAt).TotalDays
                    : (double?)null;
            })
            .Where(days => days.HasValue)
            .Select(days => days!.Value)
            .ToList();

        var avgDays = times.Any() ? Math.Round(times.Average(), 0) : 0.0;

        return new AverageTimeToRankResult(avgDays, _targetRank);
    }
}

public class MonthlyTestActivityCalculator : IProgressionAnalyticsCalculator
{
    public async Task<IAnalyticsResult> Calculate(
        IQueryable<StudentProgressionEventRecord> events,
        int? programId,
        CancellationToken cancellationToken)
    {
        var testEvents = await events
            .AsNoTracking()
            .Where(e => e.EventType == nameof(TestAttemptEventData))
            .Where(e => !programId.HasValue || e.ProgramId == programId.Value)
            .Select(e => e.OccurredAt)
            .ToListAsync(cancellationToken);

        var testsByMonth = testEvents
            .GroupBy(t => new { t.Year, t.Month })
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
