namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public sealed class LessonTime
{
    public DateTime Start { get; }
    public DateTime End { get; }

    public LessonTime(DateTime start, DateTime end)
    {
        if (end <= start)
            throw new ArgumentException("End time must be after start time.");

        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool Overlaps(LessonTime other)
    {
        return Start < other.End && End > other.Start;
    }

    public bool ConflictsWith(IEnumerable<LessonTime> others) =>
        others.Any(Overlaps);

    public bool Contains(DateTime moment) =>
        Start <= moment && moment < End;

    public override bool Equals(object? obj) =>
        obj is LessonTime other && Start == other.Start && End == other.End;

    public override int GetHashCode() => HashCode.Combine(Start, End);

    public override string ToString() =>
        $"{Start:g} - {End:t} ({Duration.TotalMinutes} min)";
}