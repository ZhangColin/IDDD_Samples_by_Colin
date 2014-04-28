using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using Dapper;
using SaasOvation.Collaboration.Application.Forums.Data;
using SaasOvation.Common.Port.Adapter.Persistence;

namespace SaasOvation.Collaboration.Application.Forums {
    public class PostQueryService: AbstractQueryService {
        public PostQueryService(string connectionString): base(connectionString) {}

        public IList<PostData> GetAllPostsDataByDiscussion(string tenantId, string discussionId) {
            using(DbConnection connection = this.GetConnection()) {
                return connection.Query<PostData>(
                    "select * from dbo.VwPosts where TenantId=@tenantId and DiscussionId=@discussionId",
                    new {tenantId, discussionId}).ToList();
            }
        } 
        
        public PostData GetPostDataById(string tenantId, string postId) {
            using(DbConnection connection = this.GetConnection()) {
                return connection.Query<PostData>(
                    "select * from dbo.VwPosts where TenantId=@tenantId and PostId=@postId",
                    new {tenantId, postId}).SingleOrDefault();
            }
        } 
    }
}