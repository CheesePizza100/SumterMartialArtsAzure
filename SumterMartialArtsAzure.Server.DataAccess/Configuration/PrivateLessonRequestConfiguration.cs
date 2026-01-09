using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class PrivateLessonRequestConfiguration : IEntityTypeConfiguration<PrivateLessonRequest>
{
    public void Configure(EntityTypeBuilder<PrivateLessonRequest> builder)
    {
        builder.HasKey(p => p.Id);

        // Explicitly configure RequestStatus conversion
        builder.Property(p => p.Status)
            .HasConversion(
                v => v.Value,
                v => RequestStatus.FromValue(v))
            .IsRequired();

        // Configure LessonTime as owned entity
        builder.OwnsOne(p => p.RequestedLessonTime, lt =>
        {
            lt.Property(t => t.Start).HasColumnName("RequestedStart").IsRequired();
            lt.Property(t => t.End).HasColumnName("RequestedEnd").IsRequired();
        });

        // Relationship to Instructor
        builder.HasOne(p => p.Instructor)
            .WithMany(i => i.PrivateLessonRequests)
            .HasForeignKey(p => p.InstructorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.StudentName).IsRequired().HasMaxLength(200);
        builder.Property(p => p.StudentEmail).IsRequired().HasMaxLength(200);
        builder.Property(p => p.StudentPhone).HasMaxLength(50);
        builder.Property(p => p.Notes).HasMaxLength(1000);
        builder.Property(p => p.RejectionReason).HasMaxLength(1000).IsRequired(false);
        builder.Property(p => p.CreatedAt).IsRequired();
    }
}