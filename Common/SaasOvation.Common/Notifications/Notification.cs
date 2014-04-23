using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Notifications {
    [Serializable]
    public class Notification: ValueObject {
        public long NotificationId { get; protected set; }
        public IDomainEvent DomainEvent { get; protected set; }
        public DateTime OccurredOn { get; protected set; }
        public int Version { get; private set; }

        public Notification(long notificationId, IDomainEvent domainEvent) {
            AssertionConcern.NotNull(domainEvent, "The event is required.");

            this.NotificationId = notificationId;
            this.DomainEvent = domainEvent;

            this.OccurredOn = domainEvent.OccurredOn;
            this.Version = domainEvent.EventVersion;
            this.TypeName = domainEvent.GetType().FullName;
        }

        private string _typeName;

        public string TypeName {
            get { return this._typeName; }
            protected set {
                AssertionConcern.NotEmpty(value, "The type name is required.");
                AssertionConcern.Length(value, 100, "The type name must be 100 characters or less.");

                this._typeName = value;
            }
        }

        public TEvent GetEvent<TEvent>() where TEvent: IDomainEvent {
            return (TEvent)this.DomainEvent;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.NotificationId;
        }

        public override string ToString() {
            return "Notification [event=" + DomainEvent + ", " +
                "NotificationId=" + NotificationId + ", " +
                "OccurredOn=" + OccurredOn + ", " +
                "TypeName=" + TypeName + ", " +
                "Version=" + Version + "]";
        }
    }
}