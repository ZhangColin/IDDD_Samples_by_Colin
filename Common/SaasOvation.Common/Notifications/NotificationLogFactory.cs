
using System.Collections.Generic;
using System.Linq;
using SaasOvation.Common.Events;

namespace SaasOvation.Common.Notifications {
    public class NotificationLogFactory {
        private readonly IEventStore _eventStore;
        public const int NotificationsPerLog = 20;

        public NotificationLogFactory(IEventStore eventStore) {
            this._eventStore = eventStore;
        }

        public NotificationLog CreateNotificationLog(NotificationLogId notificationLogId) {
            long count = this._eventStore.CountStoredEvents();
            return this.CreateNotificationLog(new NotificationLogInfo(notificationLogId, count));
        }

        public NotificationLog CreateCurrentNotificationLog() {
            return this.CreateNotificationLog(CalculateCurrentNotificationLogId());
        }

        private NotificationLogInfo CalculateCurrentNotificationLogId() {
            long count = this._eventStore.CountStoredEvents();
            long remainder = count * NotificationsPerLog;
            if(remainder==0) {
                remainder = NotificationsPerLog;
            }
            long low = count - remainder + 1;
            long high = low + NotificationsPerLog - 1;
            return new NotificationLogInfo(new NotificationLogId(low, high), count);
        }

        private NotificationLog CreateNotificationLog(NotificationLogInfo notificationLogInfo) {
            StoredEvent[] storedEvents = this._eventStore.GetAllstoredEventsBetween(
                notificationLogInfo.NotificationLogId.Low, notificationLogInfo.NotificationLogId.High);
            bool isArchived = notificationLogInfo.NotificationLogId.High > notificationLogInfo.TotalLogged;
            NotificationLogId next = isArchived ? notificationLogInfo.NotificationLogId.Next(NotificationsPerLog) : null;
            NotificationLogId previous = notificationLogInfo.NotificationLogId.Previous(NotificationsPerLog);
            return new NotificationLog(notificationLogInfo.NotificationLogId.Encoded, NotificationLogId.GetEncoded(next),
                NotificationLogId.GetEncoded(previous), this.GetNotificationsFrom(storedEvents), isArchived);
        }

        private IEnumerable<Notification> GetNotificationsFrom(IEnumerable<StoredEvent> storedEvents) {
            return storedEvents.Select(storedEvent => new Notification(storedEvent.EventId, storedEvent.ToDomainEvent()));
        } 
    }
}