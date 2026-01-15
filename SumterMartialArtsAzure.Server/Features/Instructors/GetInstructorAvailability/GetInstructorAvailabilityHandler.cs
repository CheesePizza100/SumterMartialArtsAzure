using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;

public class GetInstructorAvailabilityHandler
    : IRequestHandler<
        GetInstructorAvailabilityQuery,
        GetInstructorAvailabilityResponse?>
{
    private readonly AppDbContext _db;

    public GetInstructorAvailabilityHandler(AppDbContext db)
    {
        _db = db;
    }

    public async Task<GetInstructorAvailabilityResponse?> Handle(
        GetInstructorAvailabilityQuery request,
        CancellationToken cancellationToken)
    {
        // The issue is that AvailabilityRules is not a navigation property(entity relationship)
        // it's a JSON-serialized collection stored as a string. You can't use .Include() on it.
        // AvailabilityRules are automatically loaded because they're stored as JSON in the same
        // table row - no need to .Include() them. You only use .Include() for actual entity relationships (like Programs)
        var instructor = await _db.Instructors.AsNoTracking().FirstOrDefaultAsync(i => i.Id == request.InstructorId);
        if (instructor == null) return null;

        // Generate all potential slots during business hours
        var businessHours = BusinessHours.Default;
        var easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
        var todayEastern = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, easternZone).Date;
        var allPotentialSlots = businessHours.GenerateSlots(todayEastern, 30);

        // Get instructor's class schedule times to exclude
        var classScheduleTimes = instructor.ClassSchedule
            .SelectMany(cs => cs.GenerateNextOccurrences(DateTime.Today, 30))
            .ToList();

        // Get booked private lesson times
        var bookedRanges = await _db.PrivateLessonRequests
            .AsNoTracking()
            .Where(r => r.InstructorId == request.InstructorId &&
                        (r.Status == RequestStatus.Approved || r.Status == RequestStatus.Pending))
            .Select(r => r.RequestedLessonTime)
            .ToListAsync();

        // Filter out class times and booked times
        var availableSlots = allPotentialSlots
            .Where(slot => !classScheduleTimes.Any(ct => slot.Overlaps(ct)) &&
                           !bookedRanges.Any(br => slot.Overlaps(br)))
            .OrderBy(slot => slot.Start)
            .Select(lt => new AvailableSlotDto(
                lt.Start,
                lt.End,
                (int)lt.Duration.TotalMinutes))
            .ToList();

        return new GetInstructorAvailabilityResponse(availableSlots);
    }
}