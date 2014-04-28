using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.BacklogItems;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Products.Repository {
    public interface IBacklogItemRepository {
        ICollection<BacklogItem> GetAllComittedTo(TenantId tenantId, SprintId sprintId);
        ICollection<BacklogItem> GetAllScheduledFor(TenantId tenantId, ReleaseId releaseId);
        ICollection<BacklogItem> GetAllOutstanding(TenantId tenantId, ProductId productId);
        ICollection<BacklogItem> GetAll(TenantId tenantId, ProductId productId);
        BacklogItem Get(TenantId tenantId, BacklogItemId backlogItemId);
        BacklogItemId GetNextIdentity();
        void Remove(BacklogItem backlogItem);
        void RemoveAll(IEnumerable<BacklogItem> backlogItems);
        void Save(BacklogItem backlogItem);
        void SaveAll(IEnumerable<BacklogItem> backlogItems);
    }
}