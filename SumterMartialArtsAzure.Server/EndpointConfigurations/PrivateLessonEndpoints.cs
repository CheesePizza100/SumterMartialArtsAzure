using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class PrivateLessonEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Private Lessons - Public
        var privateLessons = api.MapGroup("/private-lessons");
        SubmitPrivateLessonRequestEndpoint.MapEndpoint(privateLessons);

        // Private Lessons - Admin/Instructor Only
        var privateLessonsAuth = api.MapGroup("/private-lessons")
            .RequireAuthorization("InstructorOrAdmin");
        GetPrivateLessonsEndpoint.MapEndpoint(privateLessonsAuth);
        UpdatePrivateLessonRequestStatusEndpoint.MapEndpoint(privateLessonsAuth);
    }
}