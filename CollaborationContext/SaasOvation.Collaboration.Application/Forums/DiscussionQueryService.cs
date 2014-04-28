using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SaasOvation.Collaboration.Application.Forums.Data;
using SaasOvation.Common.Port.Adapter.Persistence;

namespace SaasOvation.Collaboration.Application.Forums {
    public class DiscussionQueryService: AbstractQueryService {
        public DiscussionQueryService(string connectionString): base(connectionString) {}

        public IList<DiscussiondData> GetAllDiscussionsDataByForum(string tenantId, string forumId) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<DiscussiondData>(
                    "select * from dbo.VwDiscussions where TenantId=@tenantId and ForumId=@forumId",
                    new { tenantId, forumId }).ToList();
            }
        }

        public DiscussiondData GetDiscussionDataById(string tenantId, string discussionId) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<DiscussiondData>(
                    "select * from dbo.VwDiscussions where TenantId=@tenantId and DiscussionId=@discussionId",
                    new { tenantId, discussionId }).SingleOrDefault();
            }
        }

        public string GetDiscussionIdByExclusiveOwner(string tenantId, string exclusiveOwner) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<string>(
                    "select ForumId from dbo.VwDiscussions where TenantId=@tenantId and ExclusiveOwner=@exclusiveOwner",
                    new { tenantId, exclusiveOwner }).SingleOrDefault();
            }
        }

        public DiscussionPostsData GetDiscussionPostsDataById(string tenantId, string discussionId) {
            using (DbConnection connection = this.GetConnection()) {
                DiscussionPostsData discussionPostsData = null;
                connection.Query<DiscussionPostsData, PostData, DiscussionPostsData>(
                    @"select discussion.DiscussionId, discussion.ForumId, discussion.TenantId, discussion.Subject,discussion.ExclusiveOwner,
	                    discussion.Closed, discussion.AuthorEmailAddress, discussion.AuthorIdentity, discussion.AuthorName,
	                    post.PostId, post.DiscussionId, post.ForumId, post.TenantId, post.AuthorEmailAddress, post.AuthorIdentity,
	                    post.AuthorName, post.Body, post.ChangedOn, post.CreatedOn, post.ReplyToPostId, post.Subject
                    from dbo.VwDiscussions as discussion left join dbo.VwPosts as post on discussion.DiscussionId = post.DiscussionId
                    where discussion.TenantId=@tenantId and discussion.DiscussionId=@discussionId",
                    (discussion, post) => {
                        if (discussionPostsData == null) {
                            discussionPostsData = discussion;
                        }
                        if (discussionPostsData.Posts == null) {
                            discussionPostsData.Posts = new HashSet<PostData>();
                        }
                        discussionPostsData.Posts.Add(post);
                        return discussionPostsData;
                    }, new { tenantId, discussionId }, splitOn : "PostId");

                return discussionPostsData;
            }
        }
    }
}