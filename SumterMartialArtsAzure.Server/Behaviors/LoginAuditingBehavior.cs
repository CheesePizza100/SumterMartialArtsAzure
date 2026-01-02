using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Login;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using System.IdentityModel.Tokens.Jwt;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

public class LoginAuditingBehavior : IPipelineBehavior<LoginCommand, LoginCommandResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginAuditingBehavior(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<LoginCommandResponse> Handle(
        LoginCommand request,
        RequestHandlerDelegate<LoginCommandResponse> next,
        CancellationToken cancellationToken)
    {
        var response = await next();

        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(response.Token);
        var sessionIdClaim = token.Claims.FirstOrDefault(c => c.Type == "SessionId")?.Value;

        if (Guid.TryParse(sessionIdClaim, out var sessionId))
        {
            var auditLog = AuditLog.Create(
                response.UserId,
                sessionId,
                response.Username,
                AuditActions.UserLoggedIn,
                "User",
                response.UserId.ToString(),
                _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown"
            );

            _dbContext.AuditLogs.Add(auditLog);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        return response;
    }
}