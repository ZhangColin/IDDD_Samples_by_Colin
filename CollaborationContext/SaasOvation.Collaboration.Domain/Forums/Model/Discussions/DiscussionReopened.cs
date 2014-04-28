using System;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Discussions {
    public class DiscussionReopened: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ForumId ForumId { get; private set; }
        public DiscussionId DiscussionId { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public DiscussionReopened(TenantId tenantId, ForumId forumId, DiscussionId discussionId, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.DiscussionId = discussionId;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}