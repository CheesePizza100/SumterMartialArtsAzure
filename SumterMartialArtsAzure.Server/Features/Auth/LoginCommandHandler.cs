using MediatR;
using Microsoft.EntityFrameworkCore;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Services;
using SumterMartialArtsAzure.Server.Services;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth;

public class LoginCommandHandler
    : IRequestHandler<LoginCommand, LoginCommandResponse>
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGeneratorService _tokenGeneratorService;
    private readonly HttpContext _httpContext;
    private readonly AppDbContext _dbContext;

    public LoginCommandHandler(IPasswordHasher passwordHasher, ITokenGeneratorService tokenGeneratorService, HttpContext httpContext, IConfiguration configuration, AppDbContext dbContext)
    {
        _passwordHasher = passwordHasher;
        _tokenGeneratorService = tokenGeneratorService;
        _httpContext = httpContext;
        _dbContext = dbContext;
    }

    public async Task<LoginCommandResponse> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        var user = await _dbContext.Users
            .FirstOrDefaultAsync(u => u.Username == command.Username && u.IsActive);

        if (user == null || !user.VerifyPassword(command.Password, _passwordHasher))
        {
            throw new UnauthorizedAccessException("Invalid credentials");
        }

        user.UpdateLastLogin();
        await _dbContext.SaveChangesAsync();

        var token = _tokenGeneratorService.GenerateToken(user);

        var auditLog = AuditLog.Create(
            user.Id,
            user.Username,
            AuditActions.UserLoggedIn,
            "User",
            user.Id.ToString(),
            _httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown"
        );

        _dbContext.AuditLogs.Add(auditLog);
        await _dbContext.SaveChangesAsync();

        return new LoginCommandResponse(
            token,
            user.Username,
            user.Id,
            user.Role.ToString()
        );
    }
}