using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Forums.Repository;

namespace SaasOvation.Collaboration.Domain.Forums.Service {
    public class ForumIdentityService {
        private readonly IForumRepository _forumRepository;
        private readonly IDiscussionRepository _discussionRepository;
        private readonly IPostRepository _postRepository;

        public ForumIdentityService(IForumRepository forumRepository, IDiscussionRepository discussionRepository,
            IPostRepository postRepository) {
            this._forumRepository = forumRepository;
            this._discussionRepository = discussionRepository;
            this._postRepository = postRepository;
        }

        public ForumId GetNextForumId() {
            return this._forumRepository.GetNextIdentity();
        }

        public DiscussionId GetNextDiscussionId() {
            return this._discussionRepository.GetNextIdentity();
        }

        public PostId GetNextPostId() {
            return this._postRepository.GetNextIdentity();
        }
    }
}