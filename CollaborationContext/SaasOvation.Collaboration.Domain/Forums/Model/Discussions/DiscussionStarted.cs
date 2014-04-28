using System;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Discussions {
    public class DiscussionStarted: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ForumId ForumId { get; private set; }
        public DiscussionId DiscussionId { get; private set; }
        public Author Author { get; private set; }
        public string Subject { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public DiscussionStarted(TenantId tenantId, ForumId forumId, DiscussionId discussionId, Author author,
            string subject, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.DiscussionId = discussionId;
            this.Author = author;
            this.Subject = subject;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}