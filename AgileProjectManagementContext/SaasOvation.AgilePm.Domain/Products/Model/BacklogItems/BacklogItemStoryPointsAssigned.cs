using System;
using SaasOvation.AgilePm.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.AgilePm.Domain.Products.Model.BacklogItems {
    public class BacklogItemStoryPointsAssigned: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public BacklogItemId BacklogItemId { get; private set; }
        public StoryPoints StoryPoints { get; private set; }

        public BacklogItemStoryPointsAssigned(TenantId tenantId, BacklogItemId backlogItemId, StoryPoints storyPoints) {
            this.TenantId = tenantId;
            this.BacklogItemId = backlogItemId;
            this.StoryPoints = storyPoints;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}