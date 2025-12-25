namespace SumterMartialArtsAzure.Server.Domain.ValueObjects;

public sealed class BusinessHours
{
    public DayOfWeek[] OperatingDays { get; }
    public TimeSpan OpenTime { get; }
    public TimeSpan CloseTime { get; }
    public TimeSpan SlotDuration { get; }

    public BusinessHours(
        DayOfWeek[] operatingDays,
        TimeSpan openTime,
        TimeSpan closeTime,
        TimeSpan slotDuration)
    {
        if (operatingDays.Length == 0)
            throw new ArgumentException("Must specify at least one operating day", nameof(operatingDays));
        if (closeTime <= openTime)
            throw new ArgumentException("Close time must be after open time");
        if (slotDuration <= TimeSpan.Zero)
            throw new ArgumentException("Slot duration must be positive");

        OperatingDays = operatingDays;
        OpenTime = openTime;
        CloseTime = closeTime;
        SlotDuration = slotDuration;
    }

    // Default dojo hours: Mon-Sat 8am-9pm, 1-hour slots
    public static BusinessHours Default => new(
        new[] { DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday,
            DayOfWeek.Thursday, DayOfWeek.Friday, DayOfWeek.Saturday },
        TimeSpan.FromHours(08),
        TimeSpan.FromHours(21),
        TimeSpan.FromHours(1)
    );

    public bool IsWithinOperatingHours(LessonTime requestedTime)
    {
        var requestedDay = requestedTime.Start.DayOfWeek;
        var requestedTimeOfDay = requestedTime.Start.TimeOfDay;
        var requestedEndTimeOfDay = requestedTime.End.TimeOfDay;

        return OperatingDays.Contains(requestedDay) &&
               requestedTimeOfDay >= OpenTime &&
               requestedEndTimeOfDay <= CloseTime;
    }

    public IEnumerable<LessonTime> GenerateSlots(DateTime fromDate, int daysAhead)
    {
        var slots = new List<LessonTime>();
        var endDate = fromDate.AddDays(daysAhead);

        for (var date = fromDate.Date; date < endDate; date = date.AddDays(1))
        {
            if (!OperatingDays.Contains(date.DayOfWeek))
                continue;

            var currentTime = OpenTime;
            while (currentTime.Add(SlotDuration) <= CloseTime)
            {
                var start = date.Add(currentTime);
                var end = start.Add(SlotDuration);
                slots.Add(new LessonTime(start, end));
                currentTime = currentTime.Add(SlotDuration);
            }
        }

        return slots;
    }
}