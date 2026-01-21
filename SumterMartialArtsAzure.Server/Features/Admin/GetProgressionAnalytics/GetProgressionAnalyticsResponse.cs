using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.AnalyticsResults;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics;

public record GetProgressionAnalyticsResponse(
    int TotalEnrollments,
    int TotalTests,
    int PassedTests,
    int FailedTests,
    double PassRate,
    int TotalPromotions,
    List<RankProgression> RankProgressions,
    List<MonthlyTestActivity> MostActiveTestingMonths,
    List<RankDistribution> CurrentRankDistribution
);
public record MonthlyTestActivity(
    int Year,
    int Month,
    int TestCount
);

public record RankDistribution(
    string Rank,
    int Count
);