using SaasOvation.Common.Events;
using SaasOvation.Common.Notifications;

namespace SaasOvation.Common.Port.Adapter.Notification {
    public class RabbitMqNotificationPublisher: INotificationPublisher {
        private IEventStore _eventStore;
        private string _exchangeName;

        public void PublishNotifications() {
            throw new System.NotImplementedException();
        }

        public bool InternalOnlyTestConfirmation() {
            throw new System.NotImplementedException();
        }
    }
}