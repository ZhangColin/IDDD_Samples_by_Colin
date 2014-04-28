using System;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Posts {
    public class PostedToDiscussion: IDomainEvent {
        public TenantId TenantIdId { get; private set; }
        public ForumId ForumId { get; private set; }
        public DiscussionId DiscussionId { get; private set; }
        public PostId PostId { get; private set; }
        public Author Author { get; private set; }
        public string Subject { get; private set; }
        public string BodyText { get; private set; }
        public PostId ReplyToPostId { get; private set; }

        public PostedToDiscussion(TenantId tenantIdId, ForumId forumId, DiscussionId discussionId, PostId postId,
            Author author, string subject, string bodyText, PostId replyToPostId) {
            this.TenantIdId = tenantIdId;
            this.ForumId = forumId;
            this.DiscussionId = discussionId;
            this.PostId = postId;
            this.Author = author;
            this.Subject = subject;
            this.BodyText = bodyText;
            this.ReplyToPostId = replyToPostId;
        }

        public int EventVersion { get; set; }
        public DateTime OccurredOn { get; set; }
    }
}