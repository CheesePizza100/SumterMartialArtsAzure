using MediatR;
using System.Diagnostics;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

/// <summary>
/// Logs all domain events that are published through MediatR
/// This wraps ALL INotificationHandler executions
/// </summary>
public class DomainEventLoggingBehavior<TNotification> : IPipelineBehavior<TNotification, Unit>
    where TNotification : INotification
{
    private readonly ILogger<DomainEventLoggingBehavior<TNotification>> _logger;

    public DomainEventLoggingBehavior(ILogger<DomainEventLoggingBehavior<TNotification>> logger)
    {
        _logger = logger;
    }

    public async Task<Unit> Handle(
        TNotification notification,
        RequestHandlerDelegate<Unit> next,
        CancellationToken cancellationToken)
    {
        var notificationName = typeof(TNotification).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "Domain Event Published: {NotificationName} - {@Notification}",
            notificationName,
            notification
        );

        try
        {
            var result = await next();

            stopwatch.Stop();

            _logger.LogInformation(
                "Domain Event Handled: {NotificationName} in {ElapsedMilliseconds}ms",
                notificationName,
                stopwatch.ElapsedMilliseconds
            );

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "Error handling Domain Event: {NotificationName} after {ElapsedMilliseconds}ms",
                notificationName,
                stopwatch.ElapsedMilliseconds
            );

            throw;
        }
    }
}