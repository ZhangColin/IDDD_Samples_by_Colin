using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Identity.Repository {
    public interface ITenantRepository {
        TenantId GetNextIdentity();
        void Remove(Tenant tenant);
        void Add(Tenant tenant);
        Tenant Get(TenantId tenantId);
        Tenant GetByName(string name);
    }
}