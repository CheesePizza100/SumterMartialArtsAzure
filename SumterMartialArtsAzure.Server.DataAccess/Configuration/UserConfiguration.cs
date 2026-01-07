using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Username)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Role)
            .HasConversion<int>();

        builder.Property(u => u.MustChangePassword)
            .IsRequired();

        builder.HasIndex(u => u.Username).IsUnique();
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.StudentId);
        builder.HasIndex(u => u.InstructorId);

        // Relationship to Student (optional)
        builder.HasOne(u => u.Student)
            .WithOne()
            .HasForeignKey<User>(u => u.StudentId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);

        // Relationship to Instructor (optional)
        builder.HasOne(u => u.Instructor)
            .WithOne()
            .HasForeignKey<User>(u => u.InstructorId)
            .OnDelete(DeleteBehavior.Restrict)
            .IsRequired(false);
    }
}