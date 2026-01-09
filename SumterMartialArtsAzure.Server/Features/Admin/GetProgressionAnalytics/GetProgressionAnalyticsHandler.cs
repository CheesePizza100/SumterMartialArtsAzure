using System.Text.Json;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Entities;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics;

public class GetProgressionAnalyticsHandler
    : IRequestHandler<GetProgressionAnalyticsQuery, GetProgressionAnalyticsResponse>
{
    private readonly AppDbContext _dbContext;

    public GetProgressionAnalyticsHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GetProgressionAnalyticsResponse> Handle(
            GetProgressionAnalyticsQuery request,
            CancellationToken cancellationToken)
    {
        var query = _dbContext.StudentProgressionEvents.AsQueryable();

        // Filter by program if specified
        if (request.ProgramId.HasValue)
        {
            query = query.Where(e => e.ProgramId == request.ProgramId.Value);
        }

        var allEvents = await query.ToListAsync(cancellationToken);

        // Total students enrolled
        var enrollmentEvents = allEvents
            .Where(e => e.EventType == "EnrollmentEventData")
            .GroupBy(e => new { e.StudentId, e.ProgramId })
            .Count();

        // Test statistics
        var testEvents = allEvents
            .Where(e => e.EventType == "TestAttemptEventData")
            .Select(e => new
            {
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                Data = JsonSerializer.Deserialize<TestAttemptEventData>(e.EventData)
            })
            .ToList();

        var totalTests = testEvents.Count;
        var passedTests = testEvents.Count(t => t.Data?.Passed == true);
        var failedTests = testEvents.Count(t => t.Data?.Passed == false);
        var passRate = totalTests > 0 ? (double)passedTests / totalTests * 100 : 0;

        // Promotion statistics
        var promotionEvents = allEvents
            .Where(e => e.EventType == "PromotionEventData")
            .Select(e => new
            {
                e.StudentId,
                e.ProgramId,
                e.OccurredAt,
                Data = JsonSerializer.Deserialize<PromotionEventData>(e.EventData)
            })
            .ToList();

        var totalPromotions = promotionEvents.Count;

        // Average time to blue belt (example - you can expand this)
        var blueBeltPromotions = promotionEvents
            .Where(p => p.Data?.ToRank == "Blue Belt")
            .ToList();

        var avgDaysToBlue = 0.0;
        if (blueBeltPromotions.Any())
        {
            var times = new List<double>();
            foreach (var promotion in blueBeltPromotions)
            {
                // Find enrollment event for this student/program
                var enrollment = allEvents
                    .Where(e => e.EventType == "EnrollmentEventData"
                             && e.StudentId == promotion.StudentId
                             && e.ProgramId == promotion.ProgramId)
                    .OrderBy(e => e.OccurredAt)
                    .FirstOrDefault();

                if (enrollment != null)
                {
                    var days = (promotion.OccurredAt - enrollment.OccurredAt).TotalDays;
                    times.Add(days);
                }
            }

            if (times.Any())
                avgDaysToBlue = times.Average();
        }

        // Most active testing months
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

        // Rank distribution (current state)
        var rankDistribution = await GetCurrentRankDistribution(request.ProgramId, cancellationToken);

        return new GetProgressionAnalyticsResponse(
            enrollmentEvents,
            totalTests,
            passedTests,
            failedTests,
            Math.Round(passRate, 2),
            totalPromotions,
            Math.Round(avgDaysToBlue, 0),
            testsByMonth,
            rankDistribution
        );
    }
    private async Task<List<RankDistribution>> GetCurrentRankDistribution(
        int? programId,
        CancellationToken cancellationToken)
    {
        // Get all active enrollments
        var enrollmentsQuery = _dbContext.Set<StudentProgramEnrollment>()
            .Where(e => e.IsActive);

        // Filter by program if specified
        if (programId.HasValue)
        {
            enrollmentsQuery = enrollmentsQuery.Where(e => e.ProgramId == programId.Value);
        }

        // Get the data first, then group in memory
        var enrollments = await enrollmentsQuery
            .Select(e => e.CurrentRank)
            .ToListAsync(cancellationToken);

        // Group and count in memory
        var distribution = enrollments
            .GroupBy(rank => rank)
            .Select(g => new RankDistribution(g.Key, g.Count()))
            .OrderByDescending(r => r.Count)
            .ToList();

        return distribution;
    }
}