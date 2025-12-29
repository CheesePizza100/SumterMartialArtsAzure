using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorAvailability;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructorById;
using SumterMartialArtsAzure.Server.Api.Features.Instructors.GetInstructors;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.GetPrivateLessons;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.SubmitPrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.PrivateLessons.UpdatePrivateLesson;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetProgramById;
using SumterMartialArtsAzure.Server.Api.Features.Programs.GetPrograms;
using SumterMartialArtsAzure.Server.Api.Features.Students.AddTestResult;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetProgressionAnalytics;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentAttendance;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentEventStream;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentRankAtDate;
using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudents;
using SumterMartialArtsAzure.Server.Api.Features.Students.StudentSearch;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateProgramNotes;
using SumterMartialArtsAzure.Server.Api.Features.Students.UpdateStudent;

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
    }
}