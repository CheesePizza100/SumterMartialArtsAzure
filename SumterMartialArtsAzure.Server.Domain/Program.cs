using System.ComponentModel.DataAnnotations;

namespace SumterMartialArtsAzure.Server.Domain;

public class Program
{
    public int Id { get; set; }
    [Required] public string Name { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    public string AgeGroup { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public string Duration { get; set; } = string.Empty;
    public string Schedule { get; set; } = string.Empty;

    // Many-to-many relationship
    public List<Instructor> Instructors { get; set; } = new();
}