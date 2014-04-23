namespace SaasOvation.Common.Notifications {
    public interface INotificationPublisher {
        void PublishNotifications();
        bool InternalOnlyTestConfirmation();
    }
}