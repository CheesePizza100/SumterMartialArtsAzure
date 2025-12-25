using MediatR;
using SumterMartialArtsAzure.Server.Domain.Common;

namespace SumterMartialArtsAzure.Server.DataAccess;

public class DomainEventNotification<TDomainEvent> : INotification
    where TDomainEvent : IDomainEvent
{
    public TDomainEvent DomainEvent { get; }

    public DomainEventNotification(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}