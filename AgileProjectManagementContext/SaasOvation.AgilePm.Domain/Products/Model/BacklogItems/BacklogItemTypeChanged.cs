using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemTypeChanged: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public BacklogItemType BacklogItemType { get; private set; }

        public BacklogItemTypeChanged(TenantId tenantId, BacklogItemId backlogItemId, BacklogItemType backlogItemType) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.BacklogItemType = backlogItemType;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}