using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics;

public record GetProgressionAnalyticsQuery(
    int? ProgramId // Optional - filter by specific program
) : IRequest<GetProgressionAnalyticsResponse>;