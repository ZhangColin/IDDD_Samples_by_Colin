using System.Collections.Generic;

namespace SaasOvation.Common.Domain.Model {
    public abstract class EventSourcedRootEntity: EntityWithCompositeId {
        private readonly List<IDomainEvent> _mutatingEvents;
        private readonly int _unmutatedVersion;

        protected EventSourcedRootEntity() {
            _mutatingEvents = new List<IDomainEvent>();
        }

        protected EventSourcedRootEntity(IEnumerable<IDomainEvent> eventStream, int streamVersion): this() {
            foreach(IDomainEvent domainEvent in eventStream) {
                When(domainEvent);
            }
            _unmutatedVersion = streamVersion;
        }

        protected int MutatedVersion {
            get { return _unmutatedVersion + 1; }
        }

        protected int UnmutatedVersion {
            get { return _unmutatedVersion; }
        }

        public IList<IDomainEvent> GetMutatingEvents() {
            return _mutatingEvents.ToArray();
        }

        private void When(IDomainEvent e) {
            (this as dynamic).Apply(e);
        }

        protected void Apply(IDomainEvent domainEvent) {
            _mutatingEvents.Add(domainEvent);
            When(domainEvent);
        }
    }
}