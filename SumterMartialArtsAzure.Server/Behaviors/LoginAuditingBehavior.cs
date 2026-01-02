using MediatR;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Login;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        var auditLog = AuditLog.Create(
            response.UserId,
            response.Username,
            AuditActions.UserLoggedIn,
            "User",
            response.UserId.ToString(),
            _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return response;
    }
}