namespace SumterMartialArtsAzure.Server.Api.Features.Students.Shared;

public record AttendanceDto(
    int Last30Days,
    int Total,
    int AttendanceRate
);