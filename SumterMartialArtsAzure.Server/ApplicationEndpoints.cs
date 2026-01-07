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
using SumterMartialArtsAzure.Server.Api.Features.Students.AddTestResult;
using SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudent;
using SumterMartialArtsAzure.Server.Api.Features.Students.CreateStudentLogin;
using SumterMartialArtsAzure.Server.Api.Features.Students.EnrollInProgram;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetMyProfile;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetProgressionAnalytics;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentEventStream;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentRankAtDate;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;
using SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateMyContactInfo;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

namespace SumterMartialArtsAzure.Server.Api;

public static class ApplicationEndpoints
{
    public static void MapEndpoints(this IEndpointRouteBuilder app)
    {
        var api = app.MapGroup("/api");

        MapProgramEndpoints(api);
        MapInstructorEndpoints(api);
        MapStudentEndpoints(api);
        MapPrivateLessonEndpoints(api);
        MapAuthorizationEndpoints(api);
    }

    private static void MapProgramEndpoints(RouteGroupBuilder api)
    {
        var programs = api.MapGroup("/programs");
        GetProgramsEndpoint.MapEndpoint(programs);
        GetProgramByIdEndpoint.MapEndpoint(programs);
    }

    private static void MapInstructorEndpoints(RouteGroupBuilder api)
    {
        var instructors = api.MapGroup("/instructors");
        GetInstructorsEndpoint.MapEndpoint(instructors);
        GetInstructorByIdEndpoint.MapEndpoint(instructors);
        GetInstructorAvailabilityEndpoint.MapEndpoint(instructors);
        GetInstructorProfileEndpoint.MapEndpoint(instructors);
        InstructorRecordTestEndpoint.MapEndpoint(instructors);
        InstructorRecordAttendanceEndpoint.MapEndpoint(instructors);
        InstructorUpdateNotesEndpoint.MapEndpoint(instructors);
        GetMyStudentsEndpoint.MapEndpoint(instructors);
        GetStudentDetailEndpoint.MapEndpoint(instructors);
        CreateInstructorLoginEndpoint.MapEndpoint(instructors);
    }

    private static void MapStudentEndpoints(RouteGroupBuilder api)
    {
        var students = api.MapGroup("/students");
        GetStudentsEndpoint.MapEndpoint(students);
        GetStudentByIdEndpoint.MapEndpoint(students);
        StudentSearchEndpoint.MapEndpoint(students);
        UpdateStudentEndpoint.MapEndpoint(students);
        CreateStudentEndpoint.MapEndpoint(students);
        GetStudentAttendanceEndpoint.MapEndpoint(students);
        AddTestResultEndpoint.MapEndpoint(students);
        GetStudentEventStreamEndpoint.MapEndpoint(students);
        GetStudentRankAtDateEndpoint.MapEndpoint(students);
        EnrollInProgramEndpoint.MapEndpoint(students);
        CreateStudentLoginEndpoint.MapEndpoint(students);
        GetProgressionAnalyticsEndpoint.MapEndpoint(students);
        UpdateProgramNotesEndpoint.MapEndpoint(students);
    }

    private static void MapPrivateLessonEndpoints(RouteGroupBuilder api)
    {
        var privateLessons = api.MapGroup("/private-lessons");
        GetPrivateLessonsEndpoint.MapEndpoint(privateLessons);
        SubmitPrivateLessonRequestEndpoint.MapEndpoint(privateLessons);
        UpdatePrivateLessonRequestStatusEndpoint.MapEndpoint(privateLessons);
    }

    private static void MapAuthorizationEndpoints(RouteGroupBuilder api)
    {
        var auth = api.MapGroup("/auth");
        LoginEndpoint.MapEndpoint(auth);
        LogoutEndpoint.MapEndpoint(auth);
        GetMyProfileEndpoint.MapEndpoint(auth);
        UpdateMyContactInfoEndpoint.MapEndpoint(auth);
        ChangePasswordEndpoint.MapEndpoint(auth);
    }
}