using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SaasOvation.Common.Notifications {
    public class NotificationLog {
        private readonly ReadOnlyCollection<Notification> _notifications;

        public NotificationLog(string notificationLogId, string nextNotificationLogId, string previousNotificationLogId,
            IEnumerable<Notification> notifications, bool isArchived) {
            this.NotificationLogId = notificationLogId;
            this.NextNotificationLogId = nextNotificationLogId;
            this.PreviousNotificationLogId = previousNotificationLogId;
            this._notifications = new ReadOnlyCollection<Notification>(notifications.ToArray());
            this.IsArchived = isArchived;
        }

        public string NotificationLogId { get; private set; }
        public string NextNotificationLogId { get; private set; }
        public string PreviousNotificationLogId { get; private set; }
        public bool IsArchived { get; private set; }

        public ReadOnlyCollection<Notification> Notifications {
            get { return this._notifications; }
        }

        public int TotalNotifications {
            get { return this.Notifications.Count; }
        }

        public NotificationLogId DecodedNotificationLogId {
            get { return new NotificationLogId(this.NotificationLogId); }
        }

        public NotificationLogId DecodedNextNotificationLogId {
            get { return new NotificationLogId(this.NextNotificationLogId); }
        }

        public bool HasNextNotificationLog {
            get { return this.NextNotificationLogId != null; }
        }

        public NotificationLogId DecodedPreviousNotificationLogId {
            get { return new NotificationLogId(this.PreviousNotificationLogId); }
        }

        public bool HasPreviousNotificationLog {
            get { return this.PreviousNotificationLogId != null; }
        }
    }
}