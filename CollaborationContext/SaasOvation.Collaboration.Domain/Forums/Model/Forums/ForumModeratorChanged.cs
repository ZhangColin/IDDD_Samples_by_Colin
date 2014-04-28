using System;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Forums {
    public class ForumModeratorChanged: IDomainEvent {
        public TenantId TenantId { get; set; }
        public ForumId ForumId { get; set; }
        public Moderator Moderator { get; set; }
        public string ExclusiveOwner { get; set; }

        public ForumModeratorChanged(TenantId tenantId, ForumId forumId, Moderator moderator, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.Moderator = moderator;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}