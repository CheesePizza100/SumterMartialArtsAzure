using MediatR;
using SumterMartialArtsAzure.Server.Api.Auditing;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

public class AuditingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IAuditableCommand
    where TResponse : IAuditableResponse
{
    private readonly IAuditService _auditService;
    private readonly ICurrentUserService _currentUserService;

    public AuditingBehavior(IAuditService auditService, ICurrentUserService currentUserService)
    {
        _auditService = auditService;
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        // Only audit if we have a valid entity
        if (!string.IsNullOrEmpty(response.EntityId) && response.EntityId != Guid.Empty.ToString())
        {
            await _auditService.LogAsync(
                request.Action,
                request.EntityType,
                response.EntityId,
                response.GetAuditDetails()
            );
        }

        return response;
    }
}