using System;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductReleaseScheduled: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ReleaseId ReleaseId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DateTime Starts { get; private set; }
        public DateTime Ends { get; private set; }

        public ProductReleaseScheduled(TenantId tenantId, ProductId productId, ReleaseId releaseId, string name,
            string description, DateTime starts, DateTime ends) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.ReleaseId = releaseId;
            this.Name = name;
            this.Description = description;
            this.Starts = starts;
            this.Ends = ends;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}