using SaasOvation.Collaboration.Domain.Forums.Model.Forums;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Forums.Repository {
    public interface IForumRepository {
        Forum Get(TenantId tenantId, ForumId forumId);
        ForumId GetNextIdentity();
        void Save(Forum forum);
    }
}