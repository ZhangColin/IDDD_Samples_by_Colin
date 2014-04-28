using System;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Forums {
    public class ForumReopened: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ForumId ForumId { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public ForumReopened(TenantId tenantId, ForumId forumId, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}