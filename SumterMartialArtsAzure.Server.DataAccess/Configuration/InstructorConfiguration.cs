using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class InstructorConfiguration : IEntityTypeConfiguration<Instructor>
{
    public void Configure(EntityTypeBuilder<Instructor> builder)
    {
        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name).IsRequired();
        builder.Property(i => i.Email)
            .HasMaxLength(255)
            .IsRequired();
        builder.Property(i => i.Rank);
        builder.Property(i => i.Bio);
        builder.Property(i => i.PhotoUrl);

        // Store AvailabilityRules as JSON with comparer
        var classScheduleProperty = builder.Property<List<AvailabilityRule>>("_classSchedule")
            .HasColumnName("ClassSchedule")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<AvailabilityRule>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<AvailabilityRule>());

        classScheduleProperty.Metadata.SetValueComparer(
            new ValueComparer<List<AvailabilityRule>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        classScheduleProperty.HasColumnType("nvarchar(max)");

        // Store Achievements as JSON with comparer
        var achievementsProperty = builder.Property<List<string>>("_achievements")
            .HasColumnName("Achievements")
            .HasConversion(
                v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                v => System.Text.Json.JsonSerializer.Deserialize<List<string>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<string>());

        achievementsProperty.Metadata.SetValueComparer(
            new ValueComparer<List<string>>(
                (c1, c2) => c1!.SequenceEqual(c2!),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList()));

        achievementsProperty.HasColumnType("nvarchar(max)");

        // Configure navigation properties to use backing fields
        builder.Navigation(e => e.Programs)
            .HasField("_programs");

        builder.Navigation(e => e.PrivateLessonRequests)
            .HasField("_privateLessonRequests");

        // Many-to-many with Programs
        builder.HasMany(i => i.Programs)
            .WithMany(p => p.Instructors);
    }
}