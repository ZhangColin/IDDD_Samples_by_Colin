using System;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.Products {
    public class ProductBacklogItemPlanned : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public ProductId ProductId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public string Summary { get; private set; }
        public string Category { get; private set; }
        public BacklogItemType BacklogItemType { get; private set; }
        public StoryPoints StoryPoints { get; private set; }

        public ProductBacklogItemPlanned(TenantId tenantId, ProductId productId, BacklogItemId backlogItemId,
            string summary, string category, BacklogItemType backlogItemType, StoryPoints storyPoints) {
            this.TenantId = tenantId;
            this.ProductId = productId;
            this.BacklogItemId = backlogItemId;
            this.Summary = summary;
            this.Category = category;
            this.BacklogItemType = backlogItemType;
            this.StoryPoints = storyPoints;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}