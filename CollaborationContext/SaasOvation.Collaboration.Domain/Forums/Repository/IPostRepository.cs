using SaasOvation.Collaboration.Domain.Forums.Model.Posts;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Forums.Repository {
    public interface IPostRepository {
        Post Get(TenantId tenantId, PostId postId);
        PostId GetNextIdentity();
        void Save(Post post); 
    }
}