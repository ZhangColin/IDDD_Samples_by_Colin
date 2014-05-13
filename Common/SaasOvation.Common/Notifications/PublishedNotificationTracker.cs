
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Notifications {
    public class PublishedNotificationTracker: ConcurrencySafeEntity {
        public string TypeName { get; private set; }
        public long MostRecentPublishedNotificationId { get; set; }
        public long PublishedNotificationTrackerId { get; protected set; }

        public PublishedNotificationTracker(string typeName) {
            AssertionConcern.NotEmpty(typeName, "The tracker type name is required.");
            AssertionConcern.Length(typeName, 100, "The tracker type name must be 100 characters or less.");

            this.TypeName = typeName;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.PublishedNotificationTrackerId;
            yield return this.TypeName;
            yield return this.MostRecentPublishedNotificationId;
        }

        public void FailWhenConcurrencyViolation(int version) {
            AssertionConcern.True(version == this.ConcurrencyVersion, "Concurrency Violation: Stale data detected. Entity was already modified.");
        }

        public override string ToString() {
            return "PublishedNotificationTracker [MostRecentPublishedNotificationId=" +
                MostRecentPublishedNotificationId + ", PublishedNotificationTrackerId=" + PublishedNotificationTrackerId +
                ", TypeName=" + TypeName + "]";
        }
    }
}