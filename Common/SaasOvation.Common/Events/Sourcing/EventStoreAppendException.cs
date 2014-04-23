using System;

namespace SaasOvation.Common.Events.Sourcing {
    public class EventStoreAppendException: EventStoreException {
        public EventStoreAppendException(string message) : base(message) { }
        public EventStoreAppendException(string message, Exception innerException) : base(message, innerException) { }
    }
}