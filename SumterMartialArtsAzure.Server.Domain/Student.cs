using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Entities;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.Domain;

public class Student : Entity
{
    private readonly List<StudentProgramEnrollment> _programEnrollments = new();

    private readonly List<TestResult> _testHistory = new();

    public int Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string Phone { get; private set; } = string.Empty;
    public bool IsActive { get; private set; } = true;
    public IReadOnlyCollection<StudentProgramEnrollment> ProgramEnrollments => _programEnrollments.AsReadOnly();
    public IReadOnlyCollection<TestResult> TestHistory => _testHistory.AsReadOnly();

    // Value object
    //public StudentAttendance Attendance { get; private set; }

    // EF Core requires parameterless constructor
    private Student()
    {
        //Attendance = StudentAttendance.Create(0, 0);
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

        var student = new Student
        {
            Name = name,
            Email = email,
            Phone = phone,
        };

        student.AddDomainEvent(new StudentCreated
        {
            StudentId = student.Id,
            Name = name,
            Email = email,
            Phone = phone,
            CreatedAt = DateTime.UtcNow
        });

        return student;
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

        AddDomainEvent(new StudentEnrolledInProgram
        {
            StudentId = Id,
            StudentName = Name,
            ProgramId = programId,
            ProgramName = programName,
            InitialRank = initialRank,
            EnrolledAt = DateTime.UtcNow
        });
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
    public void RecordAttendance(int programId, int classesAttended)
    {
        if (classesAttended <= 0)
            throw new ArgumentException("Classes attended must be positive", nameof(classesAttended));

        var enrollment = _programEnrollments
            .FirstOrDefault(e => e.ProgramId == programId && e.IsActive);

        if (enrollment == null)
            throw new InvalidOperationException("Student must be enrolled in program to record attendance");

        enrollment.RecordAttendance(classesAttended);

        AddDomainEvent(new StudentAttendanceRecorded
        {
            StudentId = Id,
            ProgramId = programId,
            StudentName = Name,
            ClassesAttended = classesAttended,
            NewTotal = enrollment.Attendance.Total,
            NewAttendanceRate = enrollment.Attendance.AttendanceRate,
            RecordedAt = DateTime.UtcNow
        });
    }

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
        var hasGoodAttendance = enrollment.Attendance.AttendanceRate >= minimumAttendanceRate;

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

    public void Deactivate()
    {
        if (!IsActive)
            throw new InvalidOperationException("Student is already deactivated");

        IsActive = false;

        // Deactivate all program enrollments
        foreach (var enrollment in _programEnrollments)
        {
            enrollment.Deactivate();
        }

        AddDomainEvent(new StudentDeactivated
        {
            StudentId = Id,
            StudentName = Name,
            DeactivatedAt = DateTime.UtcNow
        });
    }

    public void Reactivate()
    {
        if (IsActive)
            throw new InvalidOperationException("Student is already active");

        IsActive = true;

        AddDomainEvent(new StudentReactivated
        {
            StudentId = Id,
            StudentName = Name,
            ReactivatedAt = DateTime.UtcNow
        });
    }
}