using SaasOvation.Collaboration.Domain.Collaborators;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Forums.Repository;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Application.Forums {
    public class PostApplicationService {
        private readonly IPostRepository _postRepository;
        private readonly IForumRepository _forumRepository;
        private readonly ICollaboratorService _collaboratorService;

        public PostApplicationService(IPostRepository postRepository, IForumRepository forumRepository,
            ICollaboratorService collaboratorService) {
            this._postRepository = postRepository;
            this._forumRepository = forumRepository;
            this._collaboratorService = collaboratorService;
        }

        public void ModeratePost(string tenantId, string forumId, string postId, string moderatorId, string subject,
            string bodyText) {
            Forum forum = this._forumRepository.Get(new TenantId(tenantId), new ForumId(forumId));
            Moderator moderator = this._collaboratorService.GetModeratorFrom(new TenantId(tenantId), moderatorId);
            Post post = this._postRepository.Get(new TenantId(tenantId), new PostId(postId));
            forum.ModeratePost(post, moderator, subject, bodyText);
            this._postRepository.Save(post);
        }
    }
}