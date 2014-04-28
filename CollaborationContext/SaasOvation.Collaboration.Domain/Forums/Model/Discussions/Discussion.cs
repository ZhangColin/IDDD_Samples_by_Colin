using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Forums.Service;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Discussions {
    public class Discussion: EventSourcedRootEntity {
        private TenantId _tenantId;
        private ForumId _forumId;
        private DiscussionId _discussionId;
        private Author _author;
        private string _subject;
        private string _exclusiveOwner;
        private bool _closed;

        public DiscussionId DiscussionId {
            get { return this._discussionId; }
        }

        public Discussion(IEnumerable<IDomainEvent> eventStream, int streamVersion): base(eventStream, streamVersion) {}

        public Discussion(TenantId tenantId, ForumId forumId, DiscussionId discussionId, Author author, string subject, string exclusiveOwner = null) {
            AssertionConcern.NotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.NotNull(forumId, "The forum id must be provided.");
            AssertionConcern.NotNull(discussionId, "The discussion id must be provided.");
            AssertionConcern.NotNull(author, "The author must be provided.");
            AssertionConcern.NotEmpty(subject, "The subject must be provided.");

            this.Apply(new DiscussionStarted(tenantId, forumId, discussionId, author, subject, exclusiveOwner));
        }

        private void When(DiscussionStarted e) {
            this._tenantId = e.TenantId;
            this._forumId = e.ForumId;
            this._discussionId = e.DiscussionId;
            this._author = e.Author;
            this._subject = e.Subject;
            this._exclusiveOwner = e.ExclusiveOwner;
        }

        private void AssertClosed() {
            if(!this._closed) {
                throw new InvalidOperationException("this discussion is already open.");
            }
        }

        public void Close() {
            if(this._closed) {
                throw new InvalidOperationException("This discussion is already closed.");
            }
            this.Apply(new DiscussionClosed(this._tenantId, this._forumId, this._discussionId, this._exclusiveOwner));
        }

        private void When(DiscussionClosed e) {
            this._closed = true;
        }

        public Post Post(ForumIdentityService forumIdentityService, Author author, string subject, string bodyText,
            PostId replyToPostId = null) {
            return new Post(this._tenantId, this._forumId, this._discussionId, forumIdentityService.GetNextPostId(),
                author, subject, bodyText, replyToPostId);
        }

        public void ReOpen() {
            this.AssertClosed();
            this.Apply(new DiscussionReopened(this._tenantId, this._forumId, this._discussionId, this._exclusiveOwner));
        }

        private void When(DiscussionReopened e) {
            this._closed = false;
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this._tenantId;
            yield return this._forumId;
            yield return this._discussionId;
        }
    }
}