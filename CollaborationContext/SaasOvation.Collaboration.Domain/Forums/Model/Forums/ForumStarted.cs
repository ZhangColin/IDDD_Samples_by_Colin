using System;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Forums {
    public class ForumStarted : IDomainEvent{
        public TenantId TenantId { get; private set; }
        public ForumId ForumId { get; private set; }
        public Creator Creator { get; private set; }
        public Moderator Moderator { get; set; }
        public string Subject { get; private set; }
        public string Description { get; private set; }
        public string ExclusiveOwner { get; private set; }

        public ForumStarted(TenantId tenantId, ForumId forumId, Creator creator, Moderator moderator, string subject,
            string description, string exclusiveOwner) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.Creator = creator;
            this.Moderator = moderator;
            this.Subject = subject;
            this.Description = description;
            this.ExclusiveOwner = exclusiveOwner;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}