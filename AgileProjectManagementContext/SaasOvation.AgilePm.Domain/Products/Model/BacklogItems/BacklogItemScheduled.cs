using System;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemScheduled: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public ReleaseId ReleaseId { get; private set; }

        public BacklogItemScheduled(TenantId tenantId, BacklogItemId backlogItemId, ReleaseId releaseId) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.ReleaseId = releaseId;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}