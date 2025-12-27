using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SumterMartialArtsAzure.Server.Domain.Entities;

public class TestResult
{
    public int Id { get; private set; }
    public int StudentId { get; private set; }
    public int ProgramId { get; private set; }
    public string ProgramName { get; private set; } = string.Empty;
    public DateTime TestDate { get; private set; }
    public string RankAchieved { get; private set; } = string.Empty;
    public string Result { get; private set; } = string.Empty; // "Pass" or "Fail"
    public string Notes { get; private set; } = string.Empty;

    // Navigation property
    public Student Student { get; private set; } = null!;

    // EF Core constructor
    private TestResult() { }

    // Factory method
    internal static TestResult Create(
        int studentId,
        int programId,
        string programName,
        string rankAchieved,
        string result,
        string notes,
        DateTime testDate)
    {
        if (result != "Pass" && result != "Fail")
            throw new ArgumentException("Result must be 'Pass' or 'Fail'", nameof(result));

        return new TestResult
        {
            StudentId = studentId,
            ProgramId = programId,
            ProgramName = programName,
            TestDate = testDate,
            RankAchieved = rankAchieved,
            Result = result,
            Notes = notes
        };
    }
}