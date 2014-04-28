using System;
using SaasOvation.AgilePm.Domain.Discussions;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductCreated : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public ProductOwnerId ProductOwnerId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DiscussionAvailability Availability { get; private set; }

        public ProductCreated(TenantId tenantId, ProductId productId, ProductOwnerId productOwnerId, string name,
            string description, DiscussionAvailability availability) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.ProductOwnerId = productOwnerId;
            this.Name = name;
            this.Description = description;
            this.Availability = availability;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}