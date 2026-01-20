using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.Visitor;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics.AnalyticsResults;

public interface IAnalyticsResult
{
    void Accept(IAnalyticsResultVisitor visitor);
}
public record EnrollmentCountResult(int Count) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

public record TestStatisticsResult(
    int TotalTests,
    int PassedTests,
    int FailedTests,
    double PassRate
) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

public record PromotionCountResult(int Count) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

public record RankProgression(string RankName, double AverageDays);

public record AverageTimeToRankResult(List<RankProgression> RankProgressions) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

public record MonthlyTestActivityResult(List<MonthlyTestActivity> Activity) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}

public record RankDistributionResult(List<RankDistribution> Distribution) : IAnalyticsResult
{
    public void Accept(IAnalyticsResultVisitor visitor) => visitor.Visit(this);
}