using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.AnalyticsResults;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.Visitor;

public interface IAnalyticsResultVisitor
{
    void Visit(EnrollmentCountResult result);
    void Visit(TestStatisticsResult result);
    void Visit(PromotionCountResult result);
    void Visit(AverageTimeToRankResult result);
    void Visit(MonthlyTestActivityResult result);
    void Visit(RankDistributionResult result);
}
public class AnalyticsResponseVisitor : IAnalyticsResultVisitor
{
    private int _enrollmentCount;
    private int _totalTests;
    private int _passedTests;
    private int _failedTests;
    private double _passRate;
    private int _totalPromotions;
    private double _avgDaysToBlue;
    private List<MonthlyTestActivity> _monthlyActivity = new();
    private List<RankDistribution> _rankDistribution = new();

    public void Visit(EnrollmentCountResult result)
    {
        _enrollmentCount = result.Count;
    }

    public void Visit(TestStatisticsResult result)
    {
        _totalTests = result.TotalTests;
        _passedTests = result.PassedTests;
        _failedTests = result.FailedTests;
        _passRate = result.PassRate;
    }

    public void Visit(PromotionCountResult result)
    {
        _totalPromotions = result.Count;
    }

    public void Visit(AverageTimeToRankResult result)
    {
        _avgDaysToBlue = result.AverageDays;
    }

    public void Visit(MonthlyTestActivityResult result)
    {
        _monthlyActivity = result.Activity;
    }

    public void Visit(RankDistributionResult result)
    {
        _rankDistribution = result.Distribution;
    }

    public GetProgressionAnalyticsResponse ProduceResult()
    {
        return new GetProgressionAnalyticsResponse(
            _enrollmentCount,
            _totalTests,
            _passedTests,
            _failedTests,
            _passRate,
            _totalPromotions,
            _avgDaysToBlue,
            _monthlyActivity,
            _rankDistribution
        );
    }
}