using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Common.Notifications {
    public class NotificationLogId: ValueObject {
        public static string GetEncoded(NotificationLogId notificationLogId) {
            if (notificationLogId != null) {
                return notificationLogId.Encoded;
            }
            return null;
        }

        public static NotificationLogId First(int notificationsPerLog) {
            return new NotificationLogId(0, 0).Next(notificationsPerLog);
        }

        public long Low { get; private set; }
        public long High { get; private set; }

        public NotificationLogId(long lowId, long highId) {
            this.Low = lowId;
            this.High = highId;
        }

        public NotificationLogId(string notificationLogId) {
            string[] pts = notificationLogId.Split(',');
            this.Low = long.Parse(pts[0]);
            this.High = long.Parse(pts[1]);
        }

        public NotificationLogId Next(int notificationsPerLog) {
            long nextLow = this.High + 1;
            long nextHigh = nextLow + notificationsPerLog;
            NotificationLogId next = new NotificationLogId(nextLow, nextHigh);
            if (Equals(next)) {
                next = null;
            }
            return next;
        }

        public NotificationLogId Previous(int notificaitonsPerLog) {
            long previousLow = Math.Max(this.Low - notificaitonsPerLog, 1);
            long previousHigh = previousLow + notificaitonsPerLog - 1;
            NotificationLogId previous = new NotificationLogId(previousLow, previousHigh);
            if (Equals(previous)) {
                previous = null;
            }
            return previous;
        }

        public string Encoded {
            get { return this.Low + "," + this.High; }
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return this.Low;
            yield return this.High;
        }

        public override string ToString() {
            return "NotificationLogId [Low=" + Low + ", High=" + High + "]";
        }
    }
}