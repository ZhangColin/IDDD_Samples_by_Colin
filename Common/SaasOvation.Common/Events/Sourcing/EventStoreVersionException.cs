using System;

namespace SaasOvation.Common.Events.Sourcing {
    public class EventStoreVersionException: EventStoreException {
        public EventStoreVersionException(string message): base(message) {}
        public EventStoreVersionException(string message, Exception innerException): base(message, innerException) {}
    }
}