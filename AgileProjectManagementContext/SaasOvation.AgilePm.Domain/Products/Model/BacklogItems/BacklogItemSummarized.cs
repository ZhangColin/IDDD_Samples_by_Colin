using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemSummarized: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public string Summary { get; private set; }

        public BacklogItemSummarized(TenantId tenantId, BacklogItemId backlogItemId, string summary) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.Summary = summary;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}