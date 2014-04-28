using System;
using System.Collections.Generic;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Forums.Service;
using SaasOvation.Collaboration.Domain.Tenants;
using SaasOvation.Common.Domain.Model;

namespace SaasOvation.Collaboration.Domain.Forums.Model.Forums {
    public class Forum : EventSourcedRootEntity {
        private TenantId _tenantId;
        private ForumId _forumId;
        private Creator _creator;
        private Moderator _moderator;
        private string _subject;
        private string _description;
        private string _exclusiveOwner;
        private bool _closed;

        public Forum(IEnumerable<IDomainEvent> eventStream, int streamVersion): base(eventStream, streamVersion) {}

        public Forum(TenantId tenantId, ForumId forumId, Creator creator, Moderator moderator, string subject,
            string description, string exclusiveOwner) {
            AssertionConcern.NotNull(tenantId, "The tenant must be provided.");
            AssertionConcern.NotNull(forumId, "The forum id must be provided.");
            AssertionConcern.NotNull(creator, "The creator must be provided.");
            AssertionConcern.NotNull(moderator, "The moderator must be provided.");
            AssertionConcern.NotEmpty(subject, "The subject must be provided.");
            AssertionConcern.NotEmpty(description, "The description must be provided.");

            this.Apply(new ForumStarted(tenantId, forumId, creator, moderator, subject, description, exclusiveOwner));
        }

        public ForumId ForumId {
            get { return this._forumId; }
        }

        private void When(ForumStarted e) {
            this._tenantId = e.TenantId;
            this._forumId = e.ForumId;
            this._creator = e.Creator;
            this._moderator = e.Moderator;
            this._subject = e.Subject;
            this._description = e.Description;
            this._exclusiveOwner = e.ExclusiveOwner;
        }

        private void AssertOpen() {
            if(this._closed) {
                throw new InvalidOperationException("Forum is closed");
            }
        }

        private void AssertClosed() {
            if(!this._closed) {
                throw new InvalidOperationException("Forum is open");
            }
        }

        public void AssignModerator(Moderator moderator) {
            this.AssertOpen();
            AssertionConcern.NotNull(moderator, "The moderator must be provided.");
            this.Apply(new ForumModeratorChanged(this._tenantId, this._forumId, moderator, this._exclusiveOwner));
        }

        private void When(ForumModeratorChanged e) {
            this._moderator = e.Moderator;
        }

        public void ChangeDescription(string description) {
            this.AssertOpen();
            AssertionConcern.NotEmpty(description, "The description must be provided.");
            this.Apply(new ForumDescriptionChanged(this._tenantId, this._forumId, description, this._exclusiveOwner));
        }

        private void When(ForumDescriptionChanged e) {
            this._description = e.Description;
        }

        public void ChangeSubject(string subject) {
            this.AssertOpen();
            AssertionConcern.NotEmpty(subject, "The subject must be provided.");
            this.Apply(new ForumSubjectChanged(this._tenantId, this._forumId, subject, this._exclusiveOwner));
        }

        private void When(ForumSubjectChanged e) {
            this._subject = e.Subject;
        }

        public void Close() {
            this.AssertOpen();
            this.Apply(new ForumClosed(this._tenantId, this._forumId, this._exclusiveOwner));
        }

        private void When(ForumClosed e) {
            this._closed = true;
        }

        public void ModeratePost(Post post, Moderator moderator, string subject, string bodyText) {
            this.AssertOpen();
            AssertionConcern.NotNull(post, "Post may not be null.");
            AssertionConcern.Equals(this._forumId, post.ForumId, "Not a post of this forum.");
            AssertionConcern.True(this.IsModerateBy(moderator), "Not the moderator of this forum.");
            post.AlterPostContent(subject, bodyText);
        }

        public void ReOpen() {
            this.AssertClosed();
            this.Apply(new ForumReopened(this._tenantId, this._forumId, this._exclusiveOwner));
        }

        private void When(ForumReopened e) {
            this._closed = false;
        }

        public Discussion StartDiscussionFor(ForumIdentityService forumIdentityService, Author author, string subject,
            string exclusiveOwner=null) {
            this.AssertOpen();
            return new Discussion(this._tenantId, this._forumId, forumIdentityService.GetNextDiscussionId(), author,
                subject, exclusiveOwner);
        }

        public bool IsModerateBy(Moderator moderator) {
            return this._moderator.Equals(moderator);
        }

        protected override IEnumerable<object> GetIdentityComponents() {
            yield return this._tenantId;
            yield return this._forumId;
        }


    }
}