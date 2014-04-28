namespace SaasOvation.Collaboration.Application.Forums.Data {
    public interface IForumCommandResult {
        void SetResultingForumId(string forumId);
        void SetResultingDiscussionId(string discussionId);
    }
}