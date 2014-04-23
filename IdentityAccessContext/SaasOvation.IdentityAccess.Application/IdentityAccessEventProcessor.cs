using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Events;

namespace SaasOvation.IdentityAccess.Application {
    public class IdentityAccessEventProcessor {
        private readonly IEventStore _eventStore;

        public IdentityAccessEventProcessor(IEventStore eventStore) {
            this._eventStore = eventStore;
        }

        public void Listen() {
            DomainEventPublisher.Instance.Subscribe<IDomainEvent>(domainEvent=>this._eventStore.Append(domainEvent));
        }
    }
}