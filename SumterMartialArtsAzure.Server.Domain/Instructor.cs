using SumterMartialArtsAzure.Server.Domain.Services;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Domain;

public class Instructor
{
    private readonly List<Program> _programs = new();
    private readonly List<string> _achievements = new();
    private readonly List<AvailabilityRule> _classSchedule = new();
    private readonly List<PrivateLessonRequest> _privateLessonRequests = new();

    public int Id { get; set; }
    public required string Name { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Rank { get; set; }
    public string Bio { get; set; }
    public string PhotoUrl { get; set; }

    public IReadOnlyCollection<Program> Programs => _programs.AsReadOnly();
    public IReadOnlyCollection<string> Achievements => _achievements.AsReadOnly();
    public IReadOnlyCollection<AvailabilityRule> ClassSchedule => _classSchedule.AsReadOnly();
    public IReadOnlyCollection<PrivateLessonRequest> PrivateLessonRequests => _privateLessonRequests.AsReadOnly();

    /// <summary>
    /// Returns the next available lesson slots based on rules and existing bookings
    /// </summary>
    public IEnumerable<LessonTime> GetAvailableSlots(DateTime from, int count)
    {
        // Generate all possible slots from rules
        var allCandidates = _classSchedule
            .SelectMany(r => r.GenerateNextOccurrences(from, count * 2))
            .OrderBy(lt => lt.Start);

        // Get already scheduled lessons
        var scheduledSlots = _privateLessonRequests
            .Where(r => r.Status.IsApproved)
            .Select(r => r.RequestedLessonTime)
            .ToList();

        // Filter out overlapping slots
        return allCandidates
            .Where(candidate => !scheduledSlots.Any(scheduled => scheduled.Overlaps(candidate)))
            .Take(count);
    }

    public bool IsAvailable(LessonTime requestedTime)
    {
        var service = new InstructorAvailabilityService(BusinessHours.Default);
        return service.IsAvailable(this, requestedTime);
    }

    public bool IsAvailableForUpdate(LessonTime requestedTime, int requestId)
    {
        var service = new InstructorAvailabilityService(BusinessHours.Default);
        return service.IsAvailable(this, requestedTime, requestId);
    }

    /// <summary>
    /// Checks if a specific time slot is available
    /// </summary>
    private bool IsAvailableExcluding(LessonTime requestedTime, int? excludeRequestId)
    {
        var withinBusinessHours = BusinessHours.Default.IsWithinOperatingHours(requestedTime);

        if (!withinBusinessHours) return false;

        // Check if it conflicts with the instructor's class schedule
        var conflictsWithClass = _classSchedule.Any(rule =>
        {
            if (!rule.DaysOfWeek.Contains(requestedTime.Start.DayOfWeek))
                return false;

            var classStart = requestedTime.Start.Date.Add(rule.StartTime);
            var classEnd = classStart.Add(rule.Duration);
            var classTime = new LessonTime(classStart, classEnd);

            return requestedTime.Overlaps(classTime);
        });

        if (conflictsWithClass) return false;

        // Check for conflicts with approved private lessons
        var scheduledSlots = _privateLessonRequests
            .Where(r => r.Status.IsApproved &&
                        (!excludeRequestId.HasValue || r.Id != excludeRequestId.Value))
            .Select(r => r.RequestedLessonTime)
            .ToList();
        return !requestedTime.ConflictsWith(scheduledSlots);
    }

    public bool HasClassConflict(LessonTime requestedTime)
    {
        var requestedDay = requestedTime.Start.DayOfWeek;

        return _classSchedule.Any(rule =>
        {
            if (!rule.DaysOfWeek.Contains(requestedDay))
                return false;

            var classStart = requestedTime.Start.Date.Add(rule.StartTime);
            var classEnd = classStart.Add(rule.Duration);
            var classTime = new LessonTime(classStart, classEnd);

            return requestedTime.Overlaps(classTime);
        });
    }

    public bool HasBookingConflict(LessonTime requestedTime, int? excludeRequestId = null)
    {
        var scheduledSlots = _privateLessonRequests
            .Where(r => r.Status.IsApproved &&
                        (!excludeRequestId.HasValue || r.Id != excludeRequestId.Value))
            .Select(r => r.RequestedLessonTime)
            .ToList();

        return requestedTime.ConflictsWith(scheduledSlots);
    }

    public void AddProgram(Program program)
    {
        if (program == null)
            throw new ArgumentNullException(nameof(program));

        if (_programs.Any(p => p.Id == program.Id))
            throw new InvalidOperationException($"Instructor already teaches {program.Name}");

        _programs.Add(program);
    }

    public void RemoveProgram(Program program)
    {
        if (program == null)
            throw new ArgumentNullException(nameof(program));

        _programs.Remove(program);
    }

    public void AddAchievement(string achievement)
    {
        if (string.IsNullOrWhiteSpace(achievement))
            throw new ArgumentException("Achievement cannot be empty", nameof(achievement));

        if (_achievements.Contains(achievement))
            throw new InvalidOperationException("Achievement already exists");

        _achievements.Add(achievement);
    }

    public void SetClassSchedule(IEnumerable<AvailabilityRule> schedule)
    {
        if (schedule == null)
            throw new ArgumentNullException(nameof(schedule));

        _classSchedule.Clear();
        _classSchedule.AddRange(schedule);
    }

    public void AddPrivateLessonRequest(PrivateLessonRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        _privateLessonRequests.Add(request);
    }

    public void AddAchievements(List<string> achievements)
    {
        _achievements.AddRange(achievements);
    }

    public void AddAvailabilityRules(List<AvailabilityRule> availabilityRules)
    {
        _classSchedule.AddRange(availabilityRules);
    }
}