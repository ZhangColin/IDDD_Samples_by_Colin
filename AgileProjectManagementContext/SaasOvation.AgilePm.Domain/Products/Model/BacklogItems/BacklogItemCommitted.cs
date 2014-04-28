using System;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemCommitted: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public SprintId SprintId { get; private set; }

        public BacklogItemCommitted(TenantId tenantId, BacklogItemId backlogItemId, SprintId sprintId) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.SprintId = sprintId;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}