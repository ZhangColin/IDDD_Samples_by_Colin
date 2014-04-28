using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Teams.Model;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Teams.Repository {
    public interface IProductOwnerRepository {
        ICollection<ProductOwner> GetAllProductOwners(TenantId tenantId);
        ProductOwner Get(TenantId tenantId, string userName);
        void Remove(ProductOwner owner);
        void RemoveAll(IEnumerable<ProductOwner> owners);
        void Save(ProductOwner owner);
        void SaveAll(IEnumerable<ProductOwner> owners);
    }
}