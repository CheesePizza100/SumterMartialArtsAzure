using SumterMartialArtsAzure.Server.Api.Features.Admin.AddTestResult;
using SumterMartialArtsAzure.Server.Api.Features.Admin.CreateStudent;
using SumterMartialArtsAzure.Server.Api.Features.Admin.CreateStudentLogin;
using SumterMartialArtsAzure.Server.Api.Features.Admin.EnrollInProgram;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetProgressionAnalytics;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentAttendance;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentEventStream;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudentRankAtDate;
using SumterMartialArtsAzure.Server.Api.Features.Admin.GetStudents;
using SumterMartialArtsAzure.Server.Api.Features.Admin.StudentSearch;
using SumterMartialArtsAzure.Server.Api.Features.Admin.UpdateStudent;
using SumterMartialArtsAzure.Server.Api.Features.Auth.ChangePassword;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Login;
using SumterMartialArtsAzure.Server.Api.Features.Auth.Logout;
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
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetMyPrivateLessonRequests;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;

namespace SumterMartialArtsAzure.Server.Api;

public static class ApplicationEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");
        MapProgramEndpoints(api);
        MapInstructorEndpoints(api);
        MapStudentEndpoints(api);
        MapAdminEndpoints(api);
        MapPrivateLessonEndpoints(api);
        MapAuthorizationEndpoints(api);
    }

    private static void MapProgramEndpoints(RouteGroupBuilder api)
    {
        // Programs - Public
        var programs = api.MapGroup("/programs");
        GetProgramsEndpoint.MapEndpoint(programs);
        GetProgramByIdEndpoint.MapEndpoint(programs);
    }

    private static void MapInstructorEndpoints(RouteGroupBuilder api)
    {
        // Instructors - Public
        var instructors = api.MapGroup("/instructors");
        GetInstructorsEndpoint.MapEndpoint(instructors);
        GetInstructorByIdEndpoint.MapEndpoint(instructors);
        GetInstructorAvailabilityEndpoint.MapEndpoint(instructors);

        // Instructors - Instructor Role Only
        var instructorsAuth = api.MapGroup("/instructors").RequireAuthorization("InstructorOrAdmin");
        GetInstructorProfileEndpoint.MapEndpoint(instructorsAuth);
        InstructorRecordTestEndpoint.MapEndpoint(instructorsAuth);
        InstructorRecordAttendanceEndpoint.MapEndpoint(instructorsAuth);
        InstructorUpdateNotesEndpoint.MapEndpoint(instructorsAuth);
        GetMyStudentsEndpoint.MapEndpoint(instructorsAuth);
        GetStudentDetailEndpoint.MapEndpoint(instructorsAuth);

        // Instructors - Admin Only
        var instructorsAdmin = api.MapGroup("/instructors").RequireAuthorization("AdminOnly");
        CreateInstructorLoginEndpoint.MapEndpoint(instructorsAdmin);
    }

    private static void MapStudentEndpoints(RouteGroupBuilder api)
    {
        // Students - Student Portal (viewing own data)
        var studentsAuth = api.MapGroup("/students").RequireAuthorization("StudentOrAdmin");
        GetMyPrivateLessonRequestsEndpoint.MapEndpoint(studentsAuth);
        //GetMyStudentProfileEndpoint.MapEndpoint(studentsAuth); // /api/students/me
        // Add other student portal endpoints here later
    }

    private static void MapAdminEndpoints(RouteGroupBuilder api)
    {
        // Admin - Student Management
        var adminStudents = api.MapGroup("/admin/students").RequireAuthorization("AdminOnly");
        GetStudentByIdEndpoint.MapEndpoint(adminStudents);
        GetStudentsEndpoint.MapEndpoint(adminStudents);
        StudentSearchEndpoint.MapEndpoint(adminStudents);
        UpdateStudentEndpoint.MapEndpoint(adminStudents);
        CreateStudentEndpoint.MapEndpoint(adminStudents);
        GetStudentAttendanceEndpoint.MapEndpoint(adminStudents);
        AddTestResultEndpoint.MapEndpoint(adminStudents);
        GetStudentEventStreamEndpoint.MapEndpoint(adminStudents);
        GetStudentRankAtDateEndpoint.MapEndpoint(adminStudents);
        EnrollInProgramEndpoint.MapEndpoint(adminStudents);
        CreateStudentLoginEndpoint.MapEndpoint(adminStudents);
        GetProgressionAnalyticsEndpoint.MapEndpoint(adminStudents);
    }

    private static void MapPrivateLessonEndpoints(RouteGroupBuilder api)
    {
        // Private Lessons - Public
        var privateLessons = api.MapGroup("/private-lessons");
        SubmitPrivateLessonRequestEndpoint.MapEndpoint(privateLessons);

        // Private Lessons - Admin/Instructor Only
        var privateLessonsAuth = api.MapGroup("/private-lessons").RequireAuthorization("InstructorOrAdmin");
        GetPrivateLessonsEndpoint.MapEndpoint(privateLessonsAuth);
        UpdatePrivateLessonRequestStatusEndpoint.MapEndpoint(privateLessonsAuth);
    }

    private static void MapAuthorizationEndpoints(RouteGroupBuilder api)
    {
        // Auth - Public
        var auth = api.MapGroup("/auth");
        LoginEndpoint.MapEndpoint(auth);
        LogoutEndpoint.MapEndpoint(auth);

        // Auth - Authenticated Only
        var authProtected = api.MapGroup("/auth").RequireAuthorization();
        GetMyProfileEndpoint.MapEndpoint(authProtected);
        UpdateMyContactInfoEndpoint.MapEndpoint(authProtected);
        ChangePasswordEndpoint.MapEndpoint(authProtected);
    }
}