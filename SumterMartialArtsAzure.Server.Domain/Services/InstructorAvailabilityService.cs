using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Domain.Services;

public class InstructorAvailabilityService
{
    private readonly BusinessHours _businessHours;

    public InstructorAvailabilityService(BusinessHours businessHours)
    {
        _businessHours = businessHours;
    }

    public bool IsAvailable(
        Instructor instructor,
        LessonTime requestedTime,
        int? excludeRequestId = null)
    {
        if (!_businessHours.IsWithinOperatingHours(requestedTime))
            return false;

        if (instructor.HasClassConflict(requestedTime))
            return false;

        if (instructor.HasBookingConflict(requestedTime, excludeRequestId))
            return false;

        return true;
    }
}