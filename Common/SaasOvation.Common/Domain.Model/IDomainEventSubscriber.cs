using System;

namespace SaasOvation.Common.Domain.Model {
    public interface IDomainEventSubscriber<in T> where T: IDomainEvent {
        void HandleEvent(T domainEvent);
        Type SubscribedToEventType();
    }
}