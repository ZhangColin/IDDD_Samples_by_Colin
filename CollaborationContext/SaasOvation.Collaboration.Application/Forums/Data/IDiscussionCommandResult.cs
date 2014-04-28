namespace SaasOvation.Collaboration.Application.Forums.Data {
    public interface IDiscussionCommandResult {
        void SetResultingDiscussionId(string discussionId);
        void SetResultingPostId(string postId);
        void SetResultingInReplyToPostId(string replyToPostId);
    }
}