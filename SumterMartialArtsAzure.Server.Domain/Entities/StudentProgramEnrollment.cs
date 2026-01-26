using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.Domain.Entities;

public class StudentProgramEnrollment
{
    public int Id { get; private set; }
    public int StudentId { get; private set; }
    public int ProgramId { get; private set; }
    public string ProgramName { get; private set; } = string.Empty;
    public string CurrentRank { get; private set; } = string.Empty;
    public DateTime EnrolledDate { get; private set; }
    public DateTime? LastTestDate { get; private set; }
    public string? InstructorNotes { get; private set; }
    public bool IsActive { get; private set; }

    // Value object
    public StudentAttendance Attendance { get; private set; }

    // Navigation properties
    public Student Student { get; private set; } = null!;
    // Note: We store ProgramId but don't navigate to Program to maintain aggregate boundary

    // EF Core constructor
    private StudentProgramEnrollment()
    {
        Attendance = StudentAttendance.Create(0, 0);
    }

    // Factory method
    internal static StudentProgramEnrollment Create(
        int studentId,
        int programId,
        string programName,
        string initialRank)
    {
        return new StudentProgramEnrollment
        {
            StudentId = studentId,
            ProgramId = programId,
            ProgramName = programName,
            CurrentRank = initialRank,
            EnrolledDate = DateTime.UtcNow,
            IsActive = true
        };
    }

    // Business methods
    public void RecordAttendance(int classesAttended)
    {
        Attendance = Attendance.RecordAttendance(classesAttended);
    }

    public string PromoteToRank(string newRank, string notes, DateTime testDate)
    {
        var previousRank = CurrentRank;

        CurrentRank = newRank;
        InstructorNotes = notes;
        LastTestDate = testDate;

        return previousRank;
    }

    public void UpdateNotes(string notes)
    {
        InstructorNotes = notes;
    }

    internal void Deactivate()
    {
        IsActive = false;
    }
}