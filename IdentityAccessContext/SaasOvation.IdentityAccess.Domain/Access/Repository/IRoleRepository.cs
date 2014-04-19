using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using SaasOvation.IdentityAccess.Domain.Access.Model;
using SaasOvation.IdentityAccess.Domain.Identity.Model.Tenant;

namespace SaasOvation.IdentityAccess.Domain.Access.Repository {
    public interface IRoleRepository {
        void Add(Role role);
        ICollection<Role> AllRoles(TenantId tenantId);
        Role RoleNamed(TenantId tenantId, string roleName);
        void Remove(Role role);
    }
}