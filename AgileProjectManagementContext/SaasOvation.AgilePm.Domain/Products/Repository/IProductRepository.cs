using System.Collections.Generic;
using SaasOvation.AgilePm.Domain.Products.Model.Products;
using SaasOvation.AgilePm.Domain.Tenants;

namespace SaasOvation.AgilePm.Domain.Products.Repository {
    public interface IProductRepository {
        ICollection<Product> GetAllByTenant(TenantId tenantId);
        ProductId GetNextIdentity();
        Product GetByDiscussionInitiationId(TenantId tenantId, string discussionInitiationId);
        Product Get(TenantId tenantId, ProductId productId);
        void Remove(Product product);
        void RemoveAll(IEnumerable<Product> products);
        void Save(Product product);
        void SaveAll(IEnumerable<Product> products);
    }
}