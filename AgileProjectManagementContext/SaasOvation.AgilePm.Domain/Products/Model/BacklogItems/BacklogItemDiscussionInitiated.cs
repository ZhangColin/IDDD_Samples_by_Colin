using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemDiscussionInitiated: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public BacklogItemDiscussion Discussion { get; private set; }

        public BacklogItemDiscussionInitiated(TenantId tenantId, BacklogItemId backlogItemId, BacklogItemDiscussion discussion) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.Discussion = discussion;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}