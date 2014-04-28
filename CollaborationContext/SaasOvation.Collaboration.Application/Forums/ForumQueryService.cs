using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SaasOvation.Collaboration.Application.Forums.Data;
using SaasOvation.Common.Port.Adapter.Persistence;

namespace SaasOvation.Collaboration.Application.Forums {
    public class ForumQueryService: AbstractQueryService {
        public ForumQueryService(string connectionString): base(connectionString) {}

        public IList<ForumData> GetAllForumsDataByTenant(string tenantId) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<ForumData>(
                    "select * from dbo.VwForums where TenantId=@tenantId",
                    new { tenantId}).ToList();
            }
        } 
        
        public ForumData GetForumDataById(string tenantId, string forumId) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<ForumData>(
                    "select * from dbo.VwForums where TenantId=@tenantId and ForumId=@forumId",
                    new { tenantId, forumId}).SingleOrDefault();
            }
        }

        public ForumDiscussionsData GetForumDiscussionsDataById(string tenantId, string forumId) {
            using (DbConnection connection = this.GetConnection()) {
                ForumDiscussionsData forumDiscussionsData = null;
                connection.Query<ForumDiscussionsData, DiscussiondData, ForumDiscussionsData>(
                    @"select forum.ForumId, forum.TenantId, forum.Closed, forum.CreatorEmailAddress, forum.CreatorIdentity, forum.CreatorName, 
	                    forum.Description, forum.ExclusiveOwner, forum.ModeratorEmailAddress, forum.ModeratorIdentity, forum.ModeratorName,
	                    forum.Subject, 
	                    discussion.DiscussionId, discussion.ForumId, discussion.TenantId, discussion.Subject, discussion.Closed,
	                    discussion.AuthorEmailAddress, discussion.AuthorIdentity, discussion.AuthorName, discussion.ExclusiveOwner
                    from dbo.VwForums as forum left join dbo.VwDiscussions as discussion on discussion.ForumId = forum.ForumId
                    where forum.TenantId=@tenantId and forum.ForumId=@forumId",
                    (forum, discussion) => {
                        if (forumDiscussionsData==null) {
                            forumDiscussionsData = forum;
                        }
                        if (forumDiscussionsData.Discussions == null) {
                            forumDiscussionsData.Discussions = new HashSet<DiscussiondData>();
                        }
                        forumDiscussionsData.Discussions.Add(discussion);
                        return forumDiscussionsData;
                    }, new { tenantId, forumId }, splitOn : "DiscussionId");

                return forumDiscussionsData;
            }
        }

        public string GetForumIdByExclusiveOwner(string tenantId, string exclusiveOwner) {
            using (DbConnection connection = this.GetConnection()) {
                return connection.Query<string>(
                    "select ForumId from dbo.VwForums where TenantId=@tenantId and ExclusiveOwner=@exclusiveOwner",
                    new { tenantId, exclusiveOwner }).SingleOrDefault();
            }
        }
    }
}