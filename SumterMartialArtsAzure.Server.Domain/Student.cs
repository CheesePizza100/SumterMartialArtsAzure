using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Entities;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Domain;

public class Student : Entity
{
    private readonly List<StudentProgramEnrollment> _programEnrollments = new();

    private readonly List<TestResult> _testHistory = new();

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;

    public IReadOnlyCollection<StudentProgramEnrollment> ProgramEnrollments => _programEnrollments.AsReadOnly();
    public IReadOnlyCollection<TestResult> TestHistory => _testHistory.AsReadOnly();

    // Value object
    public StudentAttendance Attendance { get; private set; }

    // EF Core requires parameterless constructor
    private Student()
    {
        Attendance = StudentAttendance.Create(0, 0);
    }

    // Factory method for creating new students
    public static Student Create(string name, string email, string phone)
    {
        // Validate
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Student name is required", nameof(name));

        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Student email is required", nameof(email));

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format", nameof(email));

        return new Student
        {
            Name = name,
            Email = email,
            Phone = phone,
            Attendance = StudentAttendance.Create(0, 0)
        };
    }

    /// <summary>
    /// Enroll student in a program
    /// </summary>
    public void EnrollInProgram(int programId, string programName, string initialRank = "Beginner")
    {
        // Business rule: Can't enroll in same program twice
        if (_programEnrollments.Any(e => e.ProgramId == programId && e.IsActive))
            throw new InvalidOperationException($"Student is already enrolled in {programName}");

        var enrollment = StudentProgramEnrollment.Create(
            Id,
            programId,
            programName,
            initialRank
        );

        _programEnrollments.Add(enrollment);
    }

    // <summary>
    /// Withdraw from a program
    /// </summary>
    public void WithdrawFromProgram(int programId)
    {
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        if (enrollment == null)
            throw new InvalidOperationException("Student is not enrolled in this program");

        enrollment.Deactivate();
    }

    /// <summary>
    /// Record a test result
    /// </summary>
    public void RecordTestResult(
        int programId,
        string programName,
        string rankAchieved,
        bool passed,
        string notes,
        DateTime? testDate = null)
    {
        // Business rule: Must be enrolled in program to test
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        if (enrollment == null)
            throw new InvalidOperationException("Student must be enrolled in program to take test");

        // Create test result
        var testResult = TestResult.Create(
            Id,
            programId,
            programName,
            rankAchieved,
            passed ? "Pass" : "Fail",
            notes,
            testDate ?? DateTime.UtcNow
        );

        _testHistory.Add(testResult);

        // If passed, promote student in the enrollment
        if (passed)
        {
            enrollment.PromoteToRank(rankAchieved, notes, testDate ?? DateTime.UtcNow);
        }
    }

    /// <summary>
    /// Update instructor notes for a program enrollment
    /// </summary>
    public void UpdateProgramNotes(int programId, string notes)
    {
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        if (enrollment == null)
            throw new InvalidOperationException("Student is not enrolled in this program");

        enrollment.UpdateNotes(notes);
    }

    /// <summary>
    /// Record attendance for classes
    /// </summary>
    public void RecordAttendance(int classesAttended)
    {
        if (classesAttended <= 0)
            throw new ArgumentException("Classes attended must be positive", nameof(classesAttended));

        Attendance = Attendance.RecordAttendance(classesAttended);
    }

    /// <summary>
    /// Update student contact information
    /// </summary>
    public void UpdateContactInfo(string? name = null, string? email = null, string? phone = null)
    {
        if (!string.IsNullOrWhiteSpace(name))
            Name = name;

        if (!string.IsNullOrWhiteSpace(email))
        {
            if (!IsValidEmail(email))
                throw new ArgumentException("Invalid email format", nameof(email));
            Email = email;
        }

        if (!string.IsNullOrWhiteSpace(phone))
            Phone = phone;
    }

    /// <summary>
    /// Get current rank in a specific program
    /// </summary>
    public string GetCurrentRank(int programId)
    {
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        return enrollment?.CurrentRank ?? "Not Enrolled";
    }

    /// <summary>
    /// Check if eligible for testing (business rule example)
    /// </summary>
    public bool IsEligibleForTesting(int programId, int minimumAttendanceRate = 75)
    {
        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        if (enrollment == null)
            return false;

        // Business rules for eligibility:
        // 1. Must be enrolled in program
        // 2. Must have minimum attendance rate
        // 3. Must have been in current rank for minimum time (e.g., 3 months)
        var enrolledLongEnough = enrollment.EnrolledDate <= DateTime.UtcNow.AddMonths(-3);
        var hasGoodAttendance = Attendance.AttendanceRate >= minimumAttendanceRate;

        return enrolledLongEnough && hasGoodAttendance;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}