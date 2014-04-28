using System;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Forums {
    public class ForumDescriptionChanged: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ForumId FormId { get; private set; }
        public string Description { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public ForumDescriptionChanged(TenantId tenantId, ForumId formId, string description, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.FormId = formId;
            this.Description = description;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}