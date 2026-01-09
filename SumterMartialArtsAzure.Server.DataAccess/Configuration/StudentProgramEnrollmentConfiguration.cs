using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SumterMartialArtsAzure.Server.Domain.Entities;

namespace SumterMartialArtsAzure.Server.DataAccess.Configuration;

public class StudentProgramEnrollmentConfiguration : IEntityTypeConfiguration<StudentProgramEnrollment>
{
    public void Configure(EntityTypeBuilder<StudentProgramEnrollment> builder)
    {
        builder.HasKey(e => e.Id);

        // Add StudentAttendance value object
        builder.OwnsOne(e => e.Attendance, attendance =>
        {
            attendance.Property(a => a.Last30Days).HasColumnName("Last30Days");
            attendance.Property(a => a.Total).HasColumnName("TotalClasses");
            attendance.Property(a => a.AttendanceRate).HasColumnName("AttendanceRate");
        });
    }
}