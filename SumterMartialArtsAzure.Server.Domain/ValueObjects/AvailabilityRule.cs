namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public sealed class AvailabilityRule
{
    public DayOfWeek[] DaysOfWeek { get; }
    public TimeSpan StartTime { get; }
    public TimeSpan Duration { get; }

    public AvailabilityRule(DayOfWeek[] daysOfWeek, TimeSpan startTime, TimeSpan duration)
    {
        if (daysOfWeek.Length == 0)
            throw new ArgumentException("Must specify at least one day", nameof(daysOfWeek));

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException("Duration must be positive", nameof(duration));

        DaysOfWeek = daysOfWeek;
        StartTime = startTime;
        Duration = duration;
    }

    /// <summary>
    /// Generate concrete LessonTimes for the next N occurrences
    /// </summary>
    public IEnumerable<LessonTime> GenerateNextOccurrences(DateTime from, int count)
    {
        var results = new List<LessonTime>();
        var date = from.Date;

        while (results.Count < count)
        {
            if (DaysOfWeek.Contains(date.DayOfWeek))
            {
                var start = date.Add(StartTime);
                var end = start.Add(Duration);
                results.Add(new LessonTime(start, end));
            }
            date = date.AddDays(1);
        }

        return results;
    }

    public override string ToString() =>
        $"{string.Join(", ", DaysOfWeek)} at {StartTime} for {Duration.TotalMinutes}min";
}