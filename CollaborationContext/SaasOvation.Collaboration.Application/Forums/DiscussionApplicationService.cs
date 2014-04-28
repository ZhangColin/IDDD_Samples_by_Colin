using SaasOvation.Collaboration.Application.Forums.Data;
using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Forums.Repository;
using SaasOvation.Collaboration.Domain.Forums.Service;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Application.Forums {
    public class DiscussionApplicationService {
        private readonly IDiscussionRepository _discussionRepository;
        private readonly ForumIdentityService _forumIdentityService;
        private readonly IPostRepository _postRepository;
        private readonly ICollaboratorService _collaboratorService;

        public DiscussionApplicationService(IDiscussionRepository discussionRepository,
            ForumIdentityService forumIdentityService, IPostRepository postRepository,
            ICollaboratorService collaboratorService) {
            this._discussionRepository = discussionRepository;
            this._forumIdentityService = forumIdentityService;
            this._postRepository = postRepository;
            this._collaboratorService = collaboratorService;
        }

        public void CloseDiscussion(string tenantId, string discussionId, string authorId, string subject,
            string bodyText, IDiscussionCommandResult discussionCommandResult) {
            Discussion discussion = this._discussionRepository.Get(new TenantId(tenantId),
                new DiscussionId(discussionId));
            Author author = this._collaboratorService.GetAuthorFrom(new TenantId(tenantId), authorId);
            Post post = discussion.Post(this._forumIdentityService, author, subject, bodyText);
            this._postRepository.Save(post);

            discussionCommandResult.SetResultingDiscussionId(discussionId);
            discussionCommandResult.SetResultingPostId(post.PostId.Id);
        }

        public void PostToDiscussionInReplyTo(string tenantId, string discussionId, string replyToPostId,
            string authorId, string subject, string bodyText, IDiscussionCommandResult discussionCommandResult) {
            Discussion discussion = this._discussionRepository.Get(new TenantId(tenantId),
                new DiscussionId(discussionId));
            Author author = this._collaboratorService.GetAuthorFrom(new TenantId(tenantId), authorId);
            Post post = discussion.Post(this._forumIdentityService, author, subject, bodyText, new PostId(replyToPostId));
            this._postRepository.Save(post);

            discussionCommandResult.SetResultingDiscussionId(discussionId);
            discussionCommandResult.SetResultingPostId(post.PostId.Id);
            discussionCommandResult.SetResultingInReplyToPostId(replyToPostId);
        }

        public void ReOpenDiscussion(string tenantId, string discussionId) {
            Discussion discussion = this._discussionRepository.Get(new TenantId(tenantId),
                new DiscussionId(discussionId));
            discussion.ReOpen();
            this._discussionRepository.Save(discussion);
        }
    }
}