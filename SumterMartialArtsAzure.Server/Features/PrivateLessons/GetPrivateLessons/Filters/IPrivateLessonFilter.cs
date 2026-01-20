using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons.Filters;

public interface IPrivateLessonFilter
{
    string FilterName { get; }
    IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query);
}
public class PendingLessonsFilter : IPrivateLessonFilter
{
    public string FilterName => "Pending";

    public IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query)
    {
        return query.Where(r => r.Status == RequestStatus.Pending);
    }
}

public class RecentLessonsFilter : IPrivateLessonFilter
{
    public string FilterName => "Recent";

    public IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query)
    {
        return query.Where(r => r.CreatedAt >= DateTime.UtcNow.AddDays(-30));
    }
}

public class AllLessonsFilter : IPrivateLessonFilter
{
    public string FilterName => "All";

    public IQueryable<PrivateLessonRequest> Apply(IQueryable<PrivateLessonRequest> query)
    {
        return query;
    }
}