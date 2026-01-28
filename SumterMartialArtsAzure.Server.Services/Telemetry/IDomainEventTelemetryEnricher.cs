using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.Services.Telemetry;

public interface IDomainEventTelemetryEnricher<in TEvent>
    where TEvent : IDomainEvent
{
    void Enrich(TEvent domainEvent, IDictionary<string, string> properties);
}