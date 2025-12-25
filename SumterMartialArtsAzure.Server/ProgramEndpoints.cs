using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;

namespace SumterMartialArtsAzure.Server.Api;

public static class ProgramEndpoints
{
    public static void MapProgramEndpoints(this IEndpointRouteBuilder app)
    {
        GetProgramsEndpoint.MapEndpoint(app);
        GetProgramByIdEndpoint.MapEndpoint(app);
        GetInstructorsEndpoint.MapEndpoint(app);
        GetInstructorByIdEndpoint.MapEndpoint(app);
        GetInstructorAvailabilityEndpoint.MapEndpoint(app);
        GetPrivateLessonsEndpoint.MapEndpoint(app);
        SubmitPrivateLessonRequestEndpoint.MapEndpoint(app);
        UpdatePrivateLessonRequestStatusEndpoint.MapEndpoint(app);
    }
}