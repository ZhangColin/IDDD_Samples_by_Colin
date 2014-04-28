using System;
using SaasOvation.Common.Notifications;

namespace SaasOvation.AgilePm.Application.Notifications {
    public class NotificationApplicationService {
        private readonly INotificationPublisher _notificationPublisher;

        public NotificationApplicationService(INotificationPublisher notificationPublisher) {
            this._notificationPublisher = notificationPublisher;
        }

        public void PublishNotifications() {
            ApplicationServiceLifeCycle.Begin(false);
            try {
                this._notificationPublisher.PublishNotifications();
                ApplicationServiceLifeCycle.Success();
            }
            catch(Exception ex) {
               ApplicationServiceLifeCycle.Fail(ex); 
            }
        }
    }
}