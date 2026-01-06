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
        GetProgramsEndpoint.MapEndpoint(app);
        GetProgramByIdEndpoint.MapEndpoint(app);
        GetInstructorsEndpoint.MapEndpoint(app);
        GetInstructorByIdEndpoint.MapEndpoint(app);
        GetInstructorAvailabilityEndpoint.MapEndpoint(app);
        GetPrivateLessonsEndpoint.MapEndpoint(app);
        SubmitPrivateLessonRequestEndpoint.MapEndpoint(app);
        UpdatePrivateLessonRequestStatusEndpoint.MapEndpoint(app);
        AddTestResultEndpoint.MapEndpoint(app);
        GetStudentAttendanceEndpoint.MapEndpoint(app);
        GetStudentByIdEndpoint.MapEndpoint(app);
        GetStudentsEndpoint.MapEndpoint(app);
        StudentSearchEndpoint.MapEndpoint(app);
        UpdateProgramNotesEndpoint.MapEndpoint(app);
        UpdateStudentEndpoint.MapEndpoint(app);
        GetProgressionAnalyticsEndpoint.MapEndpoint(app);
        GetStudentEventStreamEndpoint.MapEndpoint(app);
        GetStudentRankAtDateEndpoint.MapEndpoint(app);
        CreateStudentEndpoint.MapEndpoint(app);
        CreateStudentLoginEndpoint.MapEndpoint(app);
        EnrollInProgramEndpoint.MapEndpoint(app);
        LoginEndpoint.MapEndpoint(app);
        LogoutEndpoint.MapEndpoint(app);
        GetMyProfileEndpoint.MapEndpoint(app);
        UpdateMyContactInfoEndpoint.MapEndpoint(app);
        ChangePasswordEndpoint.MapEndpoint(app);
        CreateInstructorLoginEndpoint.MapEndpoint(app);
        GetInstructorProfileEndpoint.MapEndpoint(app);
        GetMyStudentsEndpoint.MapEndpoint(app);
        GetStudentDetailEndpoint.MapEndpoint(app);
        InstructorRecordTestEndpoint.MapEndpoint(app);
        InstructorRecordAttendanceEndpoint.MapEndpoint(app);
        InstructorUpdateNotesEndpoint.MapEndpoint(app);
    }
}