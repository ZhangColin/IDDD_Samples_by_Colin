using System;

namespace SaasOvation.Common.Events.Sourcing {
    public class EventStoreException: Exception {
        public EventStoreException(string message): base(message) {}
        public EventStoreException(string message, Exception innerException): base(message, innerException) {}
    }
}