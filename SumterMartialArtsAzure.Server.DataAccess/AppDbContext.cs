using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Events;
using SumterMartialArtsAzure.Server.Domain.ValueObjects;

namespace SumterMartialArtsAzure.Server.DataAccess;

public class AppDbContext : DbContext
{
    private readonly IMediator _mediator;

    public AppDbContext(DbContextOptions<AppDbContext> options, IMediator mediator)
        : base(options)
    {
        _mediator = mediator;
    }

    public DbSet<Program> Programs => Set<Program>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<PrivateLessonRequest> PrivateLessonRequests => Set<PrivateLessonRequest>();
    public DbSet<Student> Students => Set<Student>();
    public DbSet<StudentProgressionEventRecord> StudentProgressionEvents => Set<StudentProgressionEventRecord>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<RequestStatus>();
        modelBuilder.Ignore<LessonStatus>();
        modelBuilder.Ignore<AvailabilityRule>();

        // Program entity
        modelBuilder.Entity<Program>(b =>
        {
            b.HasKey(p => p.Id);

            b.Property(p => p.Name).IsRequired();
            b.Property(p => p.Description);
            b.Property(p => p.AgeGroup);
            b.Property(p => p.Details);
            b.Property(p => p.Duration);
            b.Property(p => p.Schedule);
            b.Property(p => p.ImageUrl);

            b.HasMany(p => p.Instructors)
                .WithMany(i => i.Programs)
                .UsingEntity(j => j.ToTable("ProgramInstructors"));
        });

        // Instructor entity
        modelBuilder.Entity<Instructor>(b =>
        {
            b.HasKey(i => i.Id);

            b.Property(i => i.Name).IsRequired();
            b.Property(i => i.Rank);
            b.Property(i => i.Bio);
            b.Property(i => i.PhotoUrl);

            // Store AvailabilityRules as JSON with comparer
            var classScheduleProperty = b.Property<List<AvailabilityRule>>("_classSchedule")
                .HasColumnName("ClassSchedule")
                .HasConversion(
                    v => System.Text.Json.JsonSerializer.Serialize(v, (System.Text.Json.JsonSerializerOptions?)null),
                    v => System.Text.Json.JsonSerializer.Deserialize<List<Domain.ValueObjects.AvailabilityRule>>(v, (System.Text.Json.JsonSerializerOptions?)null) ?? new List<Domain.ValueObjects.AvailabilityRule>());

            classScheduleProperty.Metadata.SetValueComparer(
                new ValueComparer<List<Domain.ValueObjects.AvailabilityRule>>(
                    (c1, c2) => c1!.SequenceEqual(c2!),
                    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                    c => c.ToList()));

            classScheduleProperty.HasColumnType("nvarchar(max)");

            // Store Achievements as JSON with comparer
            var achievementsProperty = b.Property<List<string>>("_achievements")
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
            b.Navigation(e => e.Programs)
                .HasField("_programs");

            b.Navigation(e => e.PrivateLessonRequests)
                .HasField("_privateLessonRequests");

            // Many-to-many with Programs
            b.HasMany(i => i.Programs)
                .WithMany(p => p.Instructors);
        });

        modelBuilder.Entity<PrivateLessonRequest>(b =>
        {
            b.HasKey(p => p.Id);

            // Explicitly configure RequestStatus conversion
            b.Property(p => p.Status)
                .HasConversion(
                    v => v.Value,
                    v => RequestStatus.FromValue(v))
                .IsRequired();

            // Configure LessonTime as owned entity
            b.OwnsOne(p => p.RequestedLessonTime, lt =>
            {
                lt.Property(t => t.Start).HasColumnName("RequestedStart").IsRequired();
                lt.Property(t => t.End).HasColumnName("RequestedEnd").IsRequired();
            });

            // Relationship to Instructor
            b.HasOne(p => p.Instructor)
                .WithMany(i => i.PrivateLessonRequests)
                .HasForeignKey(p => p.InstructorId)
                .OnDelete(DeleteBehavior.Restrict);

            b.Property(p => p.StudentName).IsRequired().HasMaxLength(200);
            b.Property(p => p.StudentEmail).IsRequired().HasMaxLength(200);
            b.Property(p => p.StudentPhone).HasMaxLength(50);
            b.Property(p => p.Notes).HasMaxLength(1000);
            b.Property(p => p.RejectionReason).HasMaxLength(1000).IsRequired(false);
            b.Property(p => p.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(s => s.Id);

            // Owned value object
            entity.OwnsOne(s => s.Attendance, attendance =>
            {
                attendance.Property(a => a.Last30Days).HasColumnName("Attendance_Last30Days");
                attendance.Property(a => a.Total).HasColumnName("Attendance_Total");
                attendance.Property(a => a.AttendanceRate).HasColumnName("Attendance_Rate");
            });

            // Enrollments collection
            entity.HasMany(s => s.ProgramEnrollments)
                .WithOne(e => e.Student)
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            // Test history collection
            entity.HasMany(s => s.TestHistory)
                .WithOne(t => t.Student)
                .HasForeignKey(t => t.StudentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configure backing fields for encapsulated collections
        modelBuilder.Entity<Student>()
            .Metadata
            .FindNavigation(nameof(Student.ProgramEnrollments))!
            .SetField("_programEnrollments");

        modelBuilder.Entity<Student>()
            .Metadata
            .FindNavigation(nameof(Student.TestHistory))!
            .SetField("_testHistory");

        // Event Store configuration
        modelBuilder.Entity<StudentProgressionEventRecord>(entity =>
        {
            entity.HasKey(e => e.EventId);

            entity.HasIndex(e => new { e.StudentId, e.ProgramId, e.Version })
                .HasDatabaseName("IX_StudentProgression_Stream");

            entity.HasIndex(e => e.OccurredAt)
                .HasDatabaseName("IX_StudentProgression_OccurredAt");

            entity.Property(e => e.EventType).HasMaxLength(100).IsRequired();
            entity.Property(e => e.EventData).IsRequired();
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entitiesWithEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToList();

        var domainEvents = entitiesWithEvents
            .SelectMany(e => e.DomainEvents)
            .ToList();

        entitiesWithEvents.ForEach(e => e.ClearDomainEvents());

        var result = await base.SaveChangesAsync(cancellationToken);

        // Wrap domain events in MediatR notifications
        foreach (var domainEvent in domainEvents)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
            var notification = Activator.CreateInstance(notificationType, domainEvent);
            await _mediator.Publish(notification!, cancellationToken);
        }

        return result;
    }
}