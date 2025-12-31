using MediatR;
using System.Diagnostics;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

public class LoggingNotificationHandlerDecorator<TNotification> : INotificationHandler<TNotification>
    where TNotification : INotification
{
    private readonly INotificationHandler<TNotification> _inner;
    private readonly ILogger<LoggingNotificationHandlerDecorator<TNotification>> _logger;

    public LoggingNotificationHandlerDecorator(
        INotificationHandler<TNotification> inner,
        ILogger<LoggingNotificationHandlerDecorator<TNotification>> logger)
    {
        _inner = inner;
        _logger = logger;
    }

    public async Task Handle(TNotification notification, CancellationToken cancellationToken)
    {
        var handlerName = _inner.GetType().Name;
        var eventName = typeof(TNotification).Name;
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "{HandlerName} handling {EventName}: {@Event}",
            handlerName,
            eventName,
            notification
        );

        try
        {
            await _inner.Handle(notification, cancellationToken);

            stopwatch.Stop();

            _logger.LogInformation(
                "{HandlerName} successfully handled {EventName} in {ElapsedMs}ms",
                handlerName,
                eventName,
                stopwatch.ElapsedMilliseconds
            );
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(
                ex,
                "{HandlerName} failed handling {EventName} after {ElapsedMs}ms",
                handlerName,
                eventName,
                stopwatch.ElapsedMilliseconds
            );

            // Don't rethrow - domain event handlers should not break the main flow
        }
    }
}