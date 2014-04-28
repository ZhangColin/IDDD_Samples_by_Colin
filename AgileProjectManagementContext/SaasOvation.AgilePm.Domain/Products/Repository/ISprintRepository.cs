using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Products.Model.Sprints;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Products.Repository {
    public interface ISprintRepository {
        Sprint Get(TenantId tenantId, SprintId sprintId);
        ICollection<Sprint> GetAllProductSprints(TenantId tenantId, ProductId productId);
        SprintId GetNextIdentity();
        void Remove(Sprint sprint);
        void RemoveAll(IEnumerable<Sprint> sprints);
        void Save(Sprint sprint);
        void SaveAll(IEnumerable<Sprint> sprints);
    }
}