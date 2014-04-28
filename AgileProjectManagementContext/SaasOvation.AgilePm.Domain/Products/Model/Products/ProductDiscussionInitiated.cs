using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductDiscussionInitiated: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ProductDiscussion ProductDiscussion { get; private set; }

        public ProductDiscussionInitiated(TenantId tenantId, ProductId productId, ProductDiscussion productDiscussion) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.ProductDiscussion = productDiscussion;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}