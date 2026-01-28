using MediatR;
using Microsoft.ApplicationInsights;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Services.Telemetry;

public class DomainEventTelemetryHandler<TEvent> : INotificationHandler<DomainEventNotification<TEvent>>
    where TEvent : IDomainEvent
{
    private readonly TelemetryClient _telemetry;
    private readonly IEnumerable<IDomainEventTelemetryEnricher<TEvent>> _enrichers;

    public DomainEventTelemetryHandler(TelemetryClient telemetry, IEnumerable<IDomainEventTelemetryEnricher<TEvent>> enrichers)
    {
        _telemetry = telemetry;
        _enrichers = enrichers;
    }

    public Task Handle(DomainEventNotification<TEvent> notification, CancellationToken ct)
    {
        var domainEvent = notification.DomainEvent;
        var eventType = typeof(TEvent).Name;

        var properties = new Dictionary<string, string>
        {
            ["OccurredOn"] = domainEvent.OccurredOn.ToString("O"),
            ["EventType"] = eventType
        };

        foreach (var enricher in _enrichers)
        {
            enricher.Enrich(domainEvent, properties);
        }

        _telemetry.TrackEvent(eventType, properties);

        return Task.CompletedTask;
    }
}