using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Login;

public record LoginRequest(string UserName, string Password);

public record LoginCommand(string Username, string Password)
    : IRequest<LoginCommandResponse>;