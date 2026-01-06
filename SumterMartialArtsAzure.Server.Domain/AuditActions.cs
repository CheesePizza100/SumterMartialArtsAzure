namespace SumterMartialArtsAzure.Server.Domain;

public static class AuditActions
{
    // User/Auth actions
    public const string UserLoggedIn = "User.LoggedIn";
    public const string UserLoggedOut = "User.LoggedOut";
    public const string UserCreated = "User.Created";
    public const string UserUpdated = "User.Updated";
    public const string UserDeactivated = "User.Deactivated";
    public const string PasswordChanged = "Password.Changed";

    // Student actions
    public const string StudentCreated = "Student.Created";
    public const string StudentLoginCreated = "Student.Login.Created";
    public const string StudentUpdated = "Student.Updated";
    public const string StudentDeactivated = "Student.Deactivated";
    public const string StudentEnrolled = "Student.Enrolled";
    public const string StudentUnenrolled = "Student.Unenrolled";
    public const string AttendanceRecorded = "Student.Attendance.Recorded";
    public const string StudentUpdatedOwnProfile = "Student.Updated.Own.Profile";

    // Program actions
    public const string ProgramCreated = "Program.Created";
    public const string ProgramUpdated = "Program.Updated";
    public const string ProgramDeleted = "Program.Deleted";

    // Instructor actions
    public const string InstructorCreated = "Instructor.Created";
    public const string InstructorLoginCreated = "Instructor.Login.Created";
    public const string InstructorUpdated = "Instructor.Updated";
    public const string InstructorDeleted = "Instructor.Deleted";
    public const string InstructorRecordedTest = "Instructor.Recorded.Test";
    public const string InstructorRecordedAttendance = "Instructor.Recorded.Attendance";
    public const string InstructorUpdatedNotes = "Instructor.Updated.Notes";

    public const string TestResultAdded = "TestResult.Added";
    public const string ProgramNotesUpdated = "ProgramNotes.Updated";
}