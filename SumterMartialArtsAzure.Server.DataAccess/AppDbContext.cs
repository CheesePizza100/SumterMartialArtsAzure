using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using SumterMartialArtsAzure.Server.Domain;
using SumterMartialArtsAzure.Server.Domain.Common;
using SumterMartialArtsAzure.Server.Domain.Entities;
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
    public DbSet<User> Users => Set<User>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<EmailTemplate> EmailTemplates => Set<EmailTemplate>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Ignore<RequestStatus>();
        modelBuilder.Ignore<LessonStatus>();
        modelBuilder.Ignore<AvailabilityRule>();

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
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