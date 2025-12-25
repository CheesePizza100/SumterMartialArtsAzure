namespace SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;

public record GetInstructorAvailabilityResponse(
    List<AvailableSlotDto> AvailableSlots
);

public record AvailableSlotDto(
    DateTime Start,
    DateTime End,
    int DurationMinutes
);