using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SumterMartialArtsAzure.Server.DataAccess;
using SumterMartialArtsAzure.Server.Domain.Common;
using System.Reflection;

namespace SumterMartialArtsAzure.Server.Services.Telemetry;

public static class TelemetryServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEventTelemetry(this IServiceCollection services, Assembly domainEventsAssembly)
    {
        var domainEventTypes = domainEventsAssembly
            .GetTypes()
            .Where(t => typeof(IDomainEvent).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var eventType in domainEventTypes)
        {
            var notificationType = typeof(DomainEventNotification<>).MakeGenericType(eventType);
            var handlerType = typeof(DomainEventTelemetryHandler<>).MakeGenericType(eventType);
            var serviceType = typeof(INotificationHandler<>).MakeGenericType(notificationType);

            services.AddTransient(serviceType, handlerType);
        }

        return services;
    }
}
