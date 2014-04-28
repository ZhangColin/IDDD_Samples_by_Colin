using System.IO;
using SaasOvation.Collaboration.Application.Forums.Data;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Forums.Repository;
using SaasOvation.Collaboration.Domain.Forums.Service;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Application.Forums {
    public class ForumApplicationService {
        private readonly ForumQueryService _forumQueryService;
        private readonly IForumRepository _forumRepository;
        private readonly ForumIdentityService _forumIdentityService;
        private readonly DiscussionQueryService _discussionQueryService;
        private readonly IDiscussionRepository _discussionRepository;
        private readonly ICollaboratorService _collaboratorService;

        public ForumApplicationService(ForumQueryService forumQueryService, IForumRepository forumRepository,
            ForumIdentityService forumIdentityService, DiscussionQueryService discussionQueryService,
            IDiscussionRepository discussionRepository, ICollaboratorService collaboratorService) {
            this._forumQueryService = forumQueryService;
            this._forumRepository = forumRepository;
            this._forumIdentityService = forumIdentityService;
            this._discussionQueryService = discussionQueryService;
            this._discussionRepository = discussionRepository;
            this._collaboratorService = collaboratorService;
        }

        public void AssignModeratorToForum(string tenantId, string forumId, string moderatorId) {
            TenantId tenant = new TenantId(tenantId);
            Forum forum = this._forumRepository.Get(tenant, new ForumId(forumId));
            Moderator moderator = this._collaboratorService.GetModeratorFrom(tenant, moderatorId);
            forum.AssignModerator(moderator);
            this._forumRepository.Save(forum);
        }

        public void ChangeForumDescription(string tenantId, string forumId, string description) {
            Forum forum = this._forumRepository.Get(new TenantId(tenantId), new ForumId(forumId));
            forum.ChangeDescription(description);
            this._forumRepository.Save(forum);
        }

        public void ChangeForumSubject(string tenantId, string forumId, string subject) {
            Forum forum = this._forumRepository.Get(new TenantId(tenantId), new ForumId(forumId));
            forum.ChangeSubject(subject);
            this._forumRepository.Save(forum);
        }

        public void CloseForum(string tenantId, string forumId) {
            Forum forum = this._forumRepository.Get(new TenantId(tenantId), new ForumId(forumId));
            forum.Close();
            this._forumRepository.Save(forum);
        }
        
        public void ReOpenForum(string tenantId, string forumId) {
            Forum forum = this._forumRepository.Get(new TenantId(tenantId), new ForumId(forumId));
            forum.ReOpen();
            this._forumRepository.Save(forum);
        }

        public void StartForum(string tenantId, string creatorId, string moderatorId, string subject, string description,
            IForumCommandResult result = null) {
            Forum forum = StartNewForum(new TenantId(tenantId), creatorId, moderatorId, subject, description, null);
            if(result!=null) {
                result.SetResultingForumId(forum.ForumId.Id);
            }
        }

        public void StartExclusiveForum(string tenantId, string exclusiveOwner, string creatorId, string moderatorId,
            string subject, string description, IForumCommandResult result = null) {
            TenantId tenant = new TenantId(tenantId);
            Forum forum = null;
            string forumId = this._forumQueryService.GetForumIdByExclusiveOwner(tenantId, exclusiveOwner);
            if(forumId!=null) {
                forum = this._forumRepository.Get(tenant, new ForumId(forumId));
            }

            if(forum==null) {
                forum = StartNewForum(tenant, creatorId, moderatorId, subject, description, exclusiveOwner);
            }

            if(result!=null) {
                result.SetResultingForumId(forum.ForumId.Id);
            }
        }

        public void StartExclusiveForumWithDiscussion(string tenantId, string exclusiveOwner, string creatorId,
            string moderatorId, string authorId, string forumSubject, string forumDescription, string discussionSubject,
            IForumCommandResult result = null) {
            TenantId tenant = new TenantId(tenantId);
            Forum forum = null;
            string forumId = this._forumQueryService.GetForumIdByExclusiveOwner(tenantId, exclusiveOwner);
            if (forumId != null) {
                forum = this._forumRepository.Get(tenant, new ForumId(forumId));
            }

            if (forum == null) {
                forum = StartNewForum(tenant, creatorId, moderatorId, forumSubject, forumDescription, exclusiveOwner);
            }

            Discussion discussion = null;

            string discussionId = this._discussionQueryService.GetDiscussionIdByExclusiveOwner(tenantId, exclusiveOwner);
            if(discussionId!=null) {
                discussion = this._discussionRepository.Get(tenant, new DiscussionId(discussionId));
            }

            if(discussion==null) {
                Author author = this._collaboratorService.GetAuthorFrom(tenant, authorId);
                discussion = forum.StartDiscussionFor(this._forumIdentityService, author, discussionSubject,
                    exclusiveOwner);
                this._discussionRepository.Save(discussion);
            }

            if (result != null) {
                result.SetResultingForumId(forum.ForumId.Id);
                result.SetResultingDiscussionId(discussion.DiscussionId.Id);
            }
        }

        private Forum StartNewForum(TenantId tenantId, string creatorId, string moderatorId, string subject,
            string description, string exclusiveOwner) {
            Creator creator = this._collaboratorService.GetCreatorFrom(tenantId, creatorId);
            Moderator moderator = this._collaboratorService.GetModeratorFrom(tenantId, moderatorId);

            Forum forum = new Forum(tenantId, this._forumRepository.GetNextIdentity(), creator, moderator, subject,
                description, exclusiveOwner);
            this._forumRepository.Save(forum);

            return forum;
        }
    }
}