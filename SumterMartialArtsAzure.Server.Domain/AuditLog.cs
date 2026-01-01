using System.Text.Json;

namespace SumterMartialArtsAzure.Server.Domain;

public class AuditLog
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string Username { get; private set; } = string.Empty;
    public string Action { get; private set; } = string.Empty;
    public string EntityType { get; private set; } = string.Empty;
    public string EntityId { get; private set; } = string.Empty;
    public string IpAddress { get; private set; } = string.Empty;
    public DateTime Timestamp { get; private set; }
    public string? Details { get; private set; }

    // Navigation property
    public User User { get; private set; } = null!;

    // Private constructor for EF
    private AuditLog() { }

    // Factory method
    public static AuditLog Create(
        Guid userId,
        string username,
        string action,
        string entityType,
        string entityId,
        string ipAddress,
        object? details = null)
    {
        return new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            Username = username,
            Action = action,
            EntityType = entityType,
            EntityId = entityId,
            IpAddress = ipAddress,
            Timestamp = DateTime.UtcNow,
            Details = details != null ? JsonSerializer.Serialize(details) : null
        };
    }
}