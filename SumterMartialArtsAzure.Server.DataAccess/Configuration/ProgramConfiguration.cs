using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class ProgramConfiguration : IEntityTypeConfiguration<Program>
{
    public void Configure(EntityTypeBuilder<Program> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Name).IsRequired();
        builder.Property(p => p.Description);
        builder.Property(p => p.AgeGroup);
        builder.Property(p => p.Details);
        builder.Property(p => p.Duration);
        builder.Property(p => p.Schedule);
        builder.Property(p => p.ImageUrl);

        builder.HasMany(p => p.Instructors)
            .WithMany(i => i.Programs)
            .UsingEntity(j => j.ToTable("ProgramInstructors"));
    }
}