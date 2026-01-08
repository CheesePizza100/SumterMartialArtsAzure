using SumterMartialArtsAzure.Server.Api.Features.Instructors.CreateInstructorLogin;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyProfile;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetMyStudents;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetStudentDetail;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordAttendance;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.RecordTestResult;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.UpdateProgramNotes;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class InstructorEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Instructors - Public
        var instructors = api.MapGroup("/instructors");
        GetInstructorsEndpoint.MapEndpoint(instructors);
        GetInstructorByIdEndpoint.MapEndpoint(instructors);
        GetInstructorAvailabilityEndpoint.MapEndpoint(instructors);

        // Instructors - Instructor Role Only
        var instructorsAuth = api.MapGroup("/instructors")
            .RequireAuthorization("InstructorOrAdmin");
        GetInstructorProfileEndpoint.MapEndpoint(instructorsAuth);
        InstructorRecordTestEndpoint.MapEndpoint(instructorsAuth);
        InstructorRecordAttendanceEndpoint.MapEndpoint(instructorsAuth);
        InstructorUpdateNotesEndpoint.MapEndpoint(instructorsAuth);
        GetMyStudentsEndpoint.MapEndpoint(instructorsAuth);
        GetStudentDetailEndpoint.MapEndpoint(instructorsAuth);

        // Instructors - Admin Only
        var instructorsAdmin = api.MapGroup("/instructors")
            .RequireAuthorization("AdminOnly");
        CreateInstructorLoginEndpoint.MapEndpoint(instructorsAdmin);
    }
}