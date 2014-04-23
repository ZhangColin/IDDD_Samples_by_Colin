using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Events {
    public class StoredEvent: ValueObject, IEquatable<StoredEvent> {
        private readonly string _typeName;
        private readonly DateTime _occurredOn;
        private readonly string _eventBody;
        private readonly long _eventId;

        public StoredEvent(string typeName, DateTime occurredOn, string eventBody, long eventId) {
            AssertionConcern.NotEmpty(typeName, "The event type name is required.");
            AssertionConcern.Length(typeName, 100, "The event type name must be 100 characters or less.");

            AssertionConcern.NotEmpty(eventBody, "The event body is required.");
            AssertionConcern.Length(eventBody, 65000, "The event body must be 65000 characters or less.");

            this._typeName = typeName;
            this._occurredOn = occurredOn;
            this._eventBody = eventBody;
            this._eventId = eventId;
        }

        public string TypeName {
            get { return this._typeName; }
        }

        public DateTime OccurredOn {
            get { return this._occurredOn; }
        }

        public string EventBody {
            get { return this._eventBody; }
        }

        public long EventId {
            get { return this._eventId; }
        }

        public IDomainEvent ToDomainEvent() {
            return this.ToDomainEvent<IDomainEvent>();
        }

        public TEvent ToDomainEvent<TEvent>() where TEvent: IDomainEvent {
            Type eventType = null;
            try {
                eventType = Type.GetType(this.TypeName);
            }
            catch(Exception ex) {
                throw new InvalidOperationException("Class load error, because: {0}", ex);
            }
            return (TEvent)EventSerializer.Instance.Deserialize(this.EventBody, eventType);
        }

        public bool Equals(StoredEvent other) {
            if(object.ReferenceEquals(this, other)) {
                return true;
            }
            if(object.ReferenceEquals(null, other)) {
                return false;
            }
            return this._eventId.Equals(other._eventId);
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this._eventId;
        }
    }
}