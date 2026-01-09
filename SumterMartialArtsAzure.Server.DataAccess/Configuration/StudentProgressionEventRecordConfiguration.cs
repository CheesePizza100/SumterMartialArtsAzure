using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain.Events;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class StudentProgressionEventRecordConfiguration : IEntityTypeConfiguration<StudentProgressionEventRecord>
{
    public void Configure(EntityTypeBuilder<StudentProgressionEventRecord> builder)
    {
        builder.HasKey(e => e.EventId);

        builder.HasIndex(e => new { e.StudentId, e.ProgramId, e.Version })
            .HasDatabaseName("IX_StudentProgression_Stream");

        builder.HasIndex(e => e.OccurredAt)
            .HasDatabaseName("IX_StudentProgression_OccurredAt");

        builder.Property(e => e.EventType).HasMaxLength(100).IsRequired();
        builder.Property(e => e.EventData).IsRequired();
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
    }
}