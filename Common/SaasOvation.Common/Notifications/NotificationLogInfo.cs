namespace SaasOvation.Common.Notifications {
    public class NotificationLogInfo {
        private readonly NotificationLogId _notificationLogId;
        private readonly long _totalLogged;

        public NotificationLogInfo(NotificationLogId notificationLogId, long totalLogged) {
            this._notificationLogId = notificationLogId;
            this._totalLogged = totalLogged;
        }

        public NotificationLogId NotificationLogId {
            get { return this._notificationLogId; }
        }

        public long TotalLogged {
            get { return this._totalLogged; }
        }
    }
}