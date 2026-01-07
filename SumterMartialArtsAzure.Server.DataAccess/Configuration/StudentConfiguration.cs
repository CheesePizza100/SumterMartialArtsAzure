using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        // Enrollments collection
        builder.HasMany(s => s.ProgramEnrollments)
            .WithOne(e => e.Student)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Test history collection
        builder.HasMany(s => s.TestHistory)
            .WithOne(t => t.Student)
            .HasForeignKey(t => t.StudentId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure backing fields for encapsulated collections
        builder.Metadata
            .FindNavigation(nameof(Student.ProgramEnrollments))!
            .SetField("_programEnrollments");

        builder.Metadata
            .FindNavigation(nameof(Student.TestHistory))!
            .SetField("_testHistory");
    }
}