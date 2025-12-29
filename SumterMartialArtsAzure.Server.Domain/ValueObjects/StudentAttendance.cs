namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public class StudentAttendance
{
    public int Last30Days { get; private set; }
    public int Total { get; private set; }
    public int AttendanceRate { get; private set; }

    // Private constructor - use factory method
    private StudentAttendance() { }

    // Factory method
    public static StudentAttendance Create(int last30Days, int total)
    {
        if (last30Days < 0)
            throw new ArgumentException("Last 30 days cannot be negative", nameof(last30Days));

        if (total < 0)
            throw new ArgumentException("Total cannot be negative", nameof(total));

        if (last30Days > total)
            throw new ArgumentException("Last 30 days cannot exceed total", nameof(last30Days));

        var rate = total > 0 ? (int)Math.Round((double)last30Days / total * 100) : 0;

        return new StudentAttendance
        {
            Last30Days = last30Days,
            Total = total,
            AttendanceRate = rate
        };
    }

    // Value objects are immutable - return new instance
    public StudentAttendance RecordAttendance(int additionalClasses)
    {
        // Simple logic - in reality you'd track by date to calculate rolling 30 days
        return Create(
            Math.Min(Last30Days + additionalClasses, 30), // Cap at 30
            Total + additionalClasses
        );
    }

    // Value object equality (optional but recommended)
    public override bool Equals(object? obj)
    {
        if (obj is not StudentAttendance other)
            return false;

        return Last30Days == other.Last30Days
               && Total == other.Total
               && AttendanceRate == other.AttendanceRate;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Last30Days, Total, AttendanceRate);
    }
}