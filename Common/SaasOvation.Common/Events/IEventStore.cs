using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events {
    public interface IEventStore {
        long CountStoredEvents();
        StoredEvent[] GetAllstoredEventsSince(long storedEventId);
        StoredEvent[] GetAllstoredEventsBetween(long lowStoredEventId, long highStoredEventId);
        StoredEvent Append(IDomainEvent domainEvent);
        void Close();
    }
}