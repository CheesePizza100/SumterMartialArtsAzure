namespace SumterMartialArtsAzure.Server.Api.Features.Auth;

public record LoginCommandResponse(string Token, string Username, Guid UserId, string Role);