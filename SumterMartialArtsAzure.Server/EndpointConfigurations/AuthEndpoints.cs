using SumterMartialArtsAzure.Server.Api.Features.Auth.ChangePassword;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Login;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Logout;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class AuthEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Auth - Public
        var auth = api.MapGroup("/auth");
        LoginEndpoint.MapEndpoint(auth);
        LogoutEndpoint.MapEndpoint(auth);

        // Auth - Authenticated Only
        var authProtected = api.MapGroup("/auth")
            .RequireAuthorization();
        ChangePasswordEndpoint.MapEndpoint(authProtected);
    }
}