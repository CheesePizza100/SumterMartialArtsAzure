using SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class StudentEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Students - Student Portal (viewing own data)
        var studentsAuth = api.MapGroup("/students")
            .RequireAuthorization("StudentOnly");
        GetMyPrivateLessonRequestsEndpoint.MapEndpoint(studentsAuth);
        GetMyProfileEndpoint.MapEndpoint(studentsAuth);
        UpdateMyContactInfoEndpoint.MapEndpoint(studentsAuth);
    }
}