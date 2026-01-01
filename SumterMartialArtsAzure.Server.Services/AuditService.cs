using Microsoft.AspNetCore.Http;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.Services;

public interface IAuditService
{
    Task LogAsync(string action, string entityType, string entityId, object? additionalData = null);
}
public class AuditService : IAuditService
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditService(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task LogAsync(string action, string entityType, string entityId, object? additionalData = null)
    {
        if (!_currentUserService.IsAuthenticated() || _currentUserService.GetUserId() == Guid.Empty)
            return;

        var auditLog = AuditLog.Create(
            _currentUserService.GetUserId(),
            _currentUserService.GetUsername() ?? "unknown",
            action,
            entityType,
            entityId,
            _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            additionalData
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync();
    }
}