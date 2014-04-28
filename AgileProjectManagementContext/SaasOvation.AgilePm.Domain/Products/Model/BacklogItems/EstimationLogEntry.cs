using System;
using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class EstimationLogEntry: Entity {
        public TenantId TenantId { get; private set; }
        public TaskId TaskId { get; private set; }
        public DateTime Date { get; private set; }
        public int HoursRemaining { get; private set; }

        public EstimationLogEntry(TenantId tenantId, TaskId taskId, DateTime date, int hoursRemaining) {
            AssertionConcern.NotNull(tenantId, "The tenant id must be provided.");
            AssertionConcern.NotNull(taskId, "The task id must be provided.");

            this.TenantId = tenantId;
            this.TaskId = taskId;
            this.Date = date;
            this.HoursRemaining = hoursRemaining;
        }

        public static DateTime CurrentLogDate {
            get { return DateTime.UtcNow.Date; }
        }

        internal bool IsMatching(DateTime date) {
            return this.Date.Equals(date);
        }

        internal bool UpdateHoursRemainingWhenDateMatches(int hoursRemaining, DateTime date) {
            if(this.IsMatching(date)) {
                this.HoursRemaining = hoursRemaining;
                return true;
            }
            return false;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.TaskId;
            yield return this.Date;
            yield return this.HoursRemaining;
        }
    }
}