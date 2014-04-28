using System;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemUnscheduled: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public ReleaseId UnscheduledReleaseId { get; private set; }

        public BacklogItemUnscheduled(TenantId tenantId, BacklogItemId backlogItemId, ReleaseId unscheduledReleaseId) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.UnscheduledReleaseId = unscheduledReleaseId;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}