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

//using SumterMartialArtsAzure.Server.Api.Features.Students.GetStudentById;

namespace SumterMartialArtsAzure.Server.Api.EndpointConfigurations;

public static class AdminEndpoints
{
    public static void Map(RouteGroupBuilder api)
    {
        // Admin - Student Management
        var adminStudents = api.MapGroup("/admin/students")
            .RequireAuthorization("AdminOnly");
        //GetStudentByIdEndpoint.MapEndpoint(adminStudents);
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
}