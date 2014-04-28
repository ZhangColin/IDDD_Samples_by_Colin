using System;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemDiscussionRequested: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public bool IsRequested { get; private set; }

        public BacklogItemDiscussionRequested(TenantId tenantId, ProductId productId, BacklogItemId backlogItemId, bool isRequested) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.BacklogItemId = backlogItemId;
            this.IsRequested = isRequested;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}