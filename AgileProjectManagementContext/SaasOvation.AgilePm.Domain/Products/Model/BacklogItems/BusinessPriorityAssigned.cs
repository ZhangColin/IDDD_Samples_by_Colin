using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BusinessPriorityAssigned : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public BusinessPrionrity BusinessPrionrity { get; private set; }

        public BusinessPriorityAssigned(TenantId tenantId, BacklogItemId backlogItemId,
            BusinessPrionrity businessPrionrity) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.BusinessPrionrity = businessPrionrity;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}