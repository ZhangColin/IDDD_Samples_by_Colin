
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Notifications {
    public class PublishedNotificationTracker: ConcurrencySafeEntity {
        public PublishedNotificationTracker(string typeName) {}

        protected override IEnumerable<object> GetIdentityComponents() {
            throw new System.NotImplementedException();
        }
    }
}