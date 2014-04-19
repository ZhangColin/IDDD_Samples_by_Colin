using System;
using System.Collections.Generic;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.IdentityAccess.Domain.Identity.Model.User {
    public class Enablement: ValueObject {
        public bool Enabled { get; private set; }
        public DateTime? StartDate { get; private set; }
        public DateTime? EndDate { get; private set; }

        protected Enablement() { }

        public Enablement(bool enabled, DateTime? startDate, DateTime? endDate) {
            this.Enabled = enabled;
            this.StartDate = startDate;
            this.EndDate = endDate;
            if (startDate > endDate) {
                throw new InvalidOperationException("Enablement start and/or end date is invalid.");
            }
        }

        public bool IsEnablementEnabled() {
            bool enabled = false;
            if (this.Enabled) {
                if (!this.IsTimeExpired()) {
                    enabled = true;
                }
            }

            return enabled;
        }

        public bool IsTimeExpired() {
            bool timeExpired = false;

            if (this.StartDate != null && this.EndDate != null) {
                DateTime now = DateTime.Now;
                if (now < this.StartDate || now > this.EndDate) {
                    timeExpired = true;
                }
            }

            return timeExpired;
        }

        protected override IEnumerable<object> GetEqualityComponents() {
            yield return Enabled;
            yield return StartDate;
            yield return EndDate;
        }

        public override string ToString() {
            return "Enablement [enabled=" + Enabled + ", endDate=" + EndDate + ", startDate=" + StartDate + "]";
        }

        public static Enablement IndefiniteEnablement() {
            return new Enablement(true, null, null);
        }
    }
}