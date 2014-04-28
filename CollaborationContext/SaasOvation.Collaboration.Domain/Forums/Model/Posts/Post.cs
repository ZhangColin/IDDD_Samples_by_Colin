using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Posts {
    public class Post : EventSourcedRootEntity {
        private TenantId _tenantId;
        private ForumId _forumId;
        private DiscussionId _discussionId;
        private PostId _postId;
        private Author _author;
        private string _subject;
        private string _bodyText;
        private PostId _replyToPostId;

        public ForumId ForumId {
            get { return this._forumId; }
        }

        public PostId PostId {
            get { return this._postId; }
        }

        public Post(IEnumerable<IDomainEvent> eventStream, int streamVersion): base(eventStream, streamVersion) {}

        public Post(TenantId tenantId, ForumId forumId, DiscussionId discussionId, PostId postId, Author author, 
            string subject, string bodyText, PostId replyToPostId=null) {
            AssertionConcern.NotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.NotNull(forumId, "The forum id must be provided.");
            AssertionConcern.NotNull(discussionId, "The discussion id must be provided.");
            AssertionConcern.NotNull(postId, "The post id must be provided.");
            AssertionConcern.NotNull(author, "The author must be provided.");

            this.AssertPostContent(subject, bodyText);

            this.Apply(new PostedToDiscussion(tenantId, forumId, discussionId, postId, author, subject, bodyText, replyToPostId));
        }

        private void When(PostedToDiscussion e) {
            this._tenantId = e.TenantIdId;
            this._forumId = e.ForumId;
            this._discussionId = e.DiscussionId;
            this._postId = e.PostId;
            this._author = e.Author;
            this._subject = e.Subject;
            this._bodyText = e.BodyText;
            this._replyToPostId = e.ReplyToPostId;
        }

        private void AssertPostContent(string subject, string bodyText) {
            AssertionConcern.NotEmpty(subject, "The subject must be provided.");
            AssertionConcern.NotEmpty(bodyText, "The body text must be provided.");
        }

        internal void AlterPostContent(string subject, string bodyText) {
            this.AssertPostContent(subject, bodyText);
            this.Apply(new PostedContentAltered(this._tenantId, this._forumId, this._discussionId, this._postId, subject, bodyText));
        }

        private void When(PostedContentAltered e) {
            this._subject = e.Subject;
            this._bodyText = e.BodyText;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this._tenantId;
            yield return this._forumId;
            yield return this._discussionId;
            yield return this._postId;
        }
    }
}