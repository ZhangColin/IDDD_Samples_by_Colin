namespace SaasOvation.Common.Domain.Model {
    public interface IValidationNotificationHandler {
        void HandleError(string notificationMessage);

        void HandleError(string notification, object obj);

        void HandleInfo(string notificationMessage);

        void HandleInfo(string notification, object obj);

        void HandleWarning(string notificationMessage);

        void HandleWarning(string notification, object obj); 
    }
}