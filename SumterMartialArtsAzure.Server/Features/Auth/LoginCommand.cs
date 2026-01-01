using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Features.Auth;

public record LoginRequest(string UserName, string Email);

public record LoginCommand(string Username, string Password)
    : IRequest<LoginCommandResponse>;