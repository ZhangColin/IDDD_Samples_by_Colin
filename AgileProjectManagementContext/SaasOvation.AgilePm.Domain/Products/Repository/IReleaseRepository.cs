using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Products.Model.Releases;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Products.Repository {
    public interface IReleaseRepository {
        ICollection<Release> GetAllProductReleases(TenantId tenantId, ProductId productId);
        ReleaseId GetNextIdentity();
        Release Get(TenantId tenantId, ReleaseId releaseId);
        void Remove(Release release);
        void RemoveAll(IEnumerable<Release> releases);
        void Save(Release release);
        void SaveAll(IEnumerable<Release> releases);
    }
}