using MediatR;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Logout;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, LogoutCommandResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly ICurrentUserService _currentUserService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogoutCommandHandler(
        AppDbContext dbContext,
        ICurrentUserService currentUserService,
        IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _currentUserService = currentUserService;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<LogoutCommandResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        // If not authenticated, return empty Guid (audit will be skipped)
        var userId = _currentUserService.IsAuthenticated()
            ? _currentUserService.GetUserId()
            : Guid.Empty;

        return Task.FromResult(new LogoutCommandResponse(userId));
    }
}