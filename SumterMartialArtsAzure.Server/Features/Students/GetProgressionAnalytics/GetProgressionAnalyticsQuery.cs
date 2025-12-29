using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Students.GetProgressionAnalytics;

public record GetProgressionAnalyticsQuery(
    int? ProgramId // Optional - filter by specific program
) : IRequest<GetProgressionAnalyticsResponse>;