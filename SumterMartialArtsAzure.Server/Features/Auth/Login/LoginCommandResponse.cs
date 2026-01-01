namespace SumterMartialArtsAzure.Server.Api.Features.Auth.Login;

public record LoginCommandResponse(string Token, string Username, Guid UserId, string Role);