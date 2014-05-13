using System.Collections.Generic;

namespace SaasOvation.Common.Notifications {
    public interface IPublishedNotificationTrackerStore {
        PublishedNotificationTracker PublishedNotificationTracker();
        PublishedNotificationTracker PublishedNotificationTracker(string typeName);

        void TrackMostRecentPublishedNotification(PublishedNotificationTracker publishedNotificationTracker,
            List<Notification> notifications);
        string TypeName { get; }
    }
}