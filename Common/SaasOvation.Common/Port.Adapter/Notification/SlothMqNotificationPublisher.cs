using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using RabbitMQ.Client.Exceptions;
using SaasOvation.Common.Domain.Model;
using SaasOvation.Common.Events;
using SaasOvation.Common.Notifications;
using SaasOvation.Common.Port.Adapter.Messaging.SlothMq;

namespace SaasOvation.Common.Port.Adapter.Notification {
    public class SlothMqNotificationPublisher : INotificationPublisher {
        private readonly IEventStore _eventStore;
        private readonly IPublishedNotificationTrackerStore _publishedNotificationTrackerStore;

        private readonly string _exchangeName;
        private readonly ExchangePublisher _exchangePublisher;

        public SlothMqNotificationPublisher(IEventStore eventStore, 
            IPublishedNotificationTrackerStore publishedNotificationTrackerStore,
            object messagingLocator) {
            this._eventStore = eventStore;
            this._publishedNotificationTrackerStore = publishedNotificationTrackerStore;
            this._exchangeName = messagingLocator.ToString();
            this._exchangePublisher = new ExchangePublisher(this._exchangeName);
        }

        public void PublishNotifications() {
            PublishedNotificationTracker publishedNotificationTracker =
                this._publishedNotificationTrackerStore.PublishedNotificationTracker();
            Notifications.Notification[] notifications =
                this.ListUnpublishedNotifications(publishedNotificationTracker.MostRecentPublishedNotificationId);

            try {
                foreach(Notifications.Notification notification in notifications) {
                    this.Publish(notification);
                }

                this._publishedNotificationTrackerStore.TrackMostRecentPublishedNotification(
                    publishedNotificationTracker, notifications.ToList());
            }
            catch(Exception ex) {
                Console.WriteLine("SLOTH: NotificationPublisher problem: "+ex.Message);
            }
        }

        public bool InternalOnlyTestConfirmation() {
            throw new UnsupportedMethodException("Not supported by production implementation");
        }

        private Notifications.Notification[] ListUnpublishedNotifications(long mostRecentPublishedMessageId) {
            StoredEvent[] storedEvents = this._eventStore.GetAllstoredEventsSince(mostRecentPublishedMessageId);

            Notifications.Notification[] notifications = this.NotificationsFrom(storedEvents);

            return notifications;
        }

        private Notifications.Notification[] NotificationsFrom(StoredEvent[] storedEvents) {
            Notifications.Notification[] notifications = new Notifications.Notification[storedEvents.Length];

            for(int i = 0; i < storedEvents.Length; i++) {
                IDomainEvent domainEvent = storedEvents[i].ToDomainEvent();
                Notifications.Notification notification = new Notifications.Notification(storedEvents[i].EventId,
                    domainEvent);

                notifications[i] = notification;
            }

            return notifications;
        }

        private void Publish(Notifications.Notification notification) {
            this._exchangePublisher.Publish(notification.TypeName, JsonConvert.SerializeObject(notification));
        }
    }
}