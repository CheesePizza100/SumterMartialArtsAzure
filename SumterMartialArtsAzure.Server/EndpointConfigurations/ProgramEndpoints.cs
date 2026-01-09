using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class ProgramEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Programs - Public
        var programs = api.MapGroup("/programs");
        GetProgramsEndpoint.MapEndpoint(programs);
        GetProgramByIdEndpoint.MapEndpoint(programs);
    }
}