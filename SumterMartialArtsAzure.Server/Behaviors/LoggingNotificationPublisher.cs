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
        foreach (var handler in handlerExecutors)
        {
            var stopwatch = Stopwatch.StartNew();

            _logger.LogInformation("Handling {Event}", notification);

            await handler.HandlerCallback(notification, cancellationToken);

            _logger.LogInformation("Handled in {Ms}ms", stopwatch.ElapsedMilliseconds);
        }
    }
}