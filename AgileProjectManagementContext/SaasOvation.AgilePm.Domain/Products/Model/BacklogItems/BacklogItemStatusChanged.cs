using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemStatusChanged: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public BacklogItemStatus BacklogItemStatus { get; private set; }

        public BacklogItemStatusChanged(TenantId tenantId, BacklogItemId backlogItemId,
            BacklogItemStatus backlogItemStatus) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.BacklogItemStatus = backlogItemStatus;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}