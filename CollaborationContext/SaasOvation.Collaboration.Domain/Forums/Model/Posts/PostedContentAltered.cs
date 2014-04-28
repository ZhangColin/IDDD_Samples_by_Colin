using System;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Posts {
    public class PostedContentAltered: IDomainEvent {
        public TenantId TenantId { get; private set; }
        public ForumId ForumId { get; private set; }
        public DiscussionId DiscussionId { get; private set; }
        public PostId PostId { get; private set; }
        public string Subject { get; private set; }
        public string BodyText { get; private set; }

        public PostedContentAltered(TenantId tenantId, ForumId forumId, DiscussionId discussionId, PostId postId,
            string subject, string bodyText) {
            this.TenantId = tenantId;
            this.ForumId = forumId;
            this.DiscussionId = discussionId;
            this.PostId = postId;
            this.Subject = subject;
            this.BodyText = bodyText;

            this.EventVersion = 1;
            this.OccurredOn = DateTime.Now;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}