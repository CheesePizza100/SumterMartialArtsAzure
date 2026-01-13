namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class ApplicationEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");
        ProgramEndpoints.Map(api);
        InstructorEndpoints.Map(api);
        StudentEndpoints.Map(api);
        AdminEndpoints.Map(api);
        PrivateLessonEndpoints.Map(api);
        AuthEndpoints.Map(api);
        EmailTemplatesEndpoints.Map(api);
    }
}