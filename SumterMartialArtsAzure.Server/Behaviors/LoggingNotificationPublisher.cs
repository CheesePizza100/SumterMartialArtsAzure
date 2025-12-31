using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SumterMartialArtsAzure.Server.Api.Behaviors;

public class LoggingNotificationPublisher : INotificationPublisher
{
    private readonly ILogger<LoggingNotificationPublisher> _logger;

    public LoggingNotificationPublisher(ILogger<LoggingNotificationPublisher> logger)
    {
        _logger = logger;
    }

    public async Task Publish(IEnumerable<NotificationHandlerExecutor> handlerExecutors, INotification notification, CancellationToken cancellationToken)
    {
        var eventName = notification.GetType().Name;

        foreach (var handler in handlerExecutors)
        {
            var handlerName = handler.HandlerInstance.GetType().Name;
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation(
                "{HandlerName} handling {EventName}: {@Event}",
                handlerName,
                eventName,
                notification
            );

            try
            {
                await handler.HandlerCallback(notification, cancellationToken);

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
            }
        }
    }
}