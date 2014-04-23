using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events.Sourcing {
    public class DispatchableDomainEvent {
        public long EventId { get; private set; }
        public IDomainEvent DomainEvent { get; private set; }

        public DispatchableDomainEvent(long eventId, IDomainEvent domainEvent) {
            this.EventId = eventId;
            this.DomainEvent = domainEvent;
        }
    }
}