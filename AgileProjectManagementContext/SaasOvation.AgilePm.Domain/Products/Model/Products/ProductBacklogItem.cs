using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductBacklogItem : Entity {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public int Ordering { get; private set; }

        public ProductBacklogItem(TenantId tenantId, ProductId productId, BacklogItemId backlogItemId, int ordering) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.BacklogItemId = backlogItemId;
            this.Ordering = ordering;
        }

        internal void ReorderFrom(BacklogItemId id, int ordering) {
            if(this.BacklogItemId.Equals(id)) {
                this.Ordering = ordering;
            }
            else if(this.Ordering>=ordering) {
                this.Ordering = this.Ordering + 1;
            }
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this.TenantId;
            yield return this.ProductId;
            yield return this.BacklogItemId;
        }

        public override string ToString() {
            return "ProductBacklogItem [TenantId=" + TenantId + ", ProductId=" + ProductId + ", BacklogItemId="
                + BacklogItemId + ", Ordering=" + Ordering + "]";
        }
    }
}