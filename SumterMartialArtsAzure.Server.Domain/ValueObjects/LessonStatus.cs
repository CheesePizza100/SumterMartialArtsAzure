using Ardalis.SmartEnum;

namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public sealed class LessonStatus : SmartEnum<LessonStatus>
{
    public static readonly LessonStatus Scheduled = new(nameof(Scheduled), 1);
    public static readonly LessonStatus Cancelled = new(nameof(Cancelled), 2);
    public static readonly LessonStatus Completed = new(nameof(Completed), 3);

    // Private parameterless constructor for EF Core (required for migrations)
    private LessonStatus() : base(string.Empty, 0) { }

    private LessonStatus(string name, int value) : base(name, value) { }

    public bool CanCancel => this == Scheduled;
    public bool CanComplete => this == Scheduled;
}