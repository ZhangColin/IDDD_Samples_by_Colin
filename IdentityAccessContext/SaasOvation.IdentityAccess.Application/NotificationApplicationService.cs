using System.Linq.Expressions;
using SaasOvation.Common.Events;
using SaasOvation.Common.Notifications;

namespace SaasOvation.IdentityAccess.Application {
    public class NotificationApplicationService {
        private readonly IEventStore _eventStore;
        private readonly INotificationPublisher _notificationPublisher;

        public NotificationApplicationService(IEventStore eventStore, INotificationPublisher notificationPublisher) {
            this._eventStore = eventStore;
            this._notificationPublisher = notificationPublisher;
        }

        public NotificationLog GetCurrentNotificationLog() {
            return new NotificationLogFactory(this._eventStore).CreateCurrentNotificationLog();
        }

        public NotificationLog GetNotificationLog(string notificationLogId) {
            return new NotificationLogFactory(this._eventStore).CreateNotificationLog(
                new NotificationLogId(notificationLogId));
        }

        public void PublishNotifications() {
            this._notificationPublisher.PublishNotifications();
        }
    }
}