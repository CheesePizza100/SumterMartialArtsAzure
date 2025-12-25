namespace SumterMartialArtsAzure.Server.Domain;

public class ProgramInstructor
{
    public int ProgramId { get; set; }
    public Program Program { get; set; } = null!;

    public int InstructorId { get; set; }
    public Instructor Instructor { get; set; } = null!;
}