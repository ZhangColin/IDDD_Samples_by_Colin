using SaasOvation.Collaboration.Domain.Forums.Model.Discussions;
using SaasOvation.Collaboration.Domain.Tenants;

namespace SaasOvation.Collaboration.Domain.Forums.Repository {
    public interface IDiscussionRepository {
        Discussion Get(TenantId tenantId, DiscussionId discussionId);

        DiscussionId GetNextIdentity();

        void Save(Discussion discussion);
    }
}